using System;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        readonly string _accessDeniedUrl = ConfigSettings.SiteBaseUrl + Constants.AccessDeniedUrl;
        readonly string _loginUrl = ConfigSettings.SiteBaseUrl + Constants.LoginUrl;
        public string Permissions { get; set; }
        public string NgReturnUrl { get; set; }
        //public string Module { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            

            var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
           
            if (CheckAllowedActions())
                return;

            var strPermissions = string.IsNullOrEmpty(Permissions) ? new string[] { } : Permissions.Split(',');
            if (SessionHelper.UserID > 0)
            {
                bool isAuthoized =
                   strPermissions.Any(strPermission => SessionHelper.SelectedRole.RoleName.Contains(strPermission));

                if (!isAuthoized)
                    isAuthoized = strPermissions.Contains(Constants.AnonymousLoginPermission);

                if (!isAuthoized && !isAjaxRequest)
                    filterContext.Result = new RedirectResult(_accessDeniedUrl);
                else if (!isAuthoized)
                    RedirectToAction(filterContext, _accessDeniedUrl);
            }

            else
            {
                if (filterContext.HttpContext.Request.CurrentExecutionFilePath != Constants.LoginUrl)
                {
                    bool removeFormsAuthenticationTicket = true;
                    bool isTimeOut = false;
                    if (filterContext.HttpContext.Request.IsAuthenticated)
                    {
                        HttpCookie decryptedCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptedCookie.Value);
                        if (ticket != null)
                        {
                            var identity = new GenericIdentity(ticket.Name);
                            if (identity.IsAuthenticated)
                            {
                                SecurityDataProvider securityDataProvider = new SecurityDataProvider();
                                LoginModel loginModel = new LoginModel { Email = ticket.Name };
                                ServiceResponse response = new ServiceResponse();
                                response = securityDataProvider.CheckLogin(loginModel, true);
                                if (response.IsSuccess)
                                {
                                    SessionValueData sessionValue = (SessionValueData)response.Data;

                                    SessionHelper.UserID = sessionValue.UserID;
                                    SessionHelper.Name = sessionValue.Name;
                                    SessionHelper.FirstName = sessionValue.FirstName;
                                    SessionHelper.Roles = sessionValue.Roles;
                                    SessionHelper.SelectedRole = sessionValue.SelectedRole;
                                    removeFormsAuthenticationTicket = false;
                                }
                                else
                                    isTimeOut = true;
                            }
                            else
                                isTimeOut = true;
                        }
                        else
                            isTimeOut = true;
                    }

                    string szCookieHeader = HttpContext.Current.Request.Headers["Cookie"];
                    if (isTimeOut || (SessionHelper.UserID == 0 && HttpContext.Current.Session.IsNewSession) && (null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") > 0))
                        SessionHelper.IsTimeOutHappened = true;

                    if (removeFormsAuthenticationTicket)
                    {
                        FormsAuthentication.SignOut();
                        string returnUrl = "?returnUrl=" +
                                             (isAjaxRequest
                                                  ? (filterContext.HttpContext.Request.UrlReferrer != null
                                                         ? filterContext.HttpContext.Request.UrlReferrer.LocalPath
                                                         : "")
                                                  : filterContext.HttpContext.Request.CurrentExecutionFilePath +
                                                    (filterContext.HttpContext.Request.QueryString.HasKeys()
                                                         ? "?" + filterContext.HttpContext.Request.QueryString
                                                         : ""));



                        string[] param = Regex.Split(filterContext.HttpContext.Request.CurrentExecutionFilePath, filterContext.ActionDescriptor.ActionName);

                        string additionParam = param.Length > 1 ? param[1] : "";
                        if (filterContext.HttpContext.Request.RequestType.ToLower() == "post")
                            additionParam = "";

                        RedirectToAction(filterContext, string.Format("{0}?returnUrl={1}{2}", _loginUrl, NgReturnUrl, additionParam)); 

                     
                    }
                }
            }
        }

        private void RedirectToAction(AuthorizationContext filterContext, string actionUrl)
        {
            var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();

            if (!isAjaxRequest)
            isAjaxRequest = IsAjaxRequest(HttpContext.Current.Request);
            if (isAjaxRequest)
            {
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                {
                    Data = new LinkResponse
                    {
                        Type = Constants.NotAuthorized,
                        Link = actionUrl,
                        StatusCode = "401",
                        RequestType = filterContext.HttpContext.Request.RequestType.ToLower()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
                HttpContext.Current.Response.Redirect(actionUrl);


            
        }

        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = HttpContext.Current;

            if (request.Headers["Accept"].Contains("application/json"))
                return true;

            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
        private bool CheckAllowedActions()
        {
            string[] strPermissions = string.IsNullOrEmpty(Permissions) ? new string[] { } : Permissions.Split(',');

            if (strPermissions.Contains(Constants.AnonymousPermission))
                return true;

            if (!strPermissions.Any())
                return true;

            return false;
        }

       
    }

    
}
