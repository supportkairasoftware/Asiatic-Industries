using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AnonymousPermission)]
    public class SecurityController : BaseController
    {
        private ISecurityDataProvider _securityDataProvider;

        #region Login
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult Index()
        {
            if (SessionHelper.UserID == 0)
            {
                //Constants.
                
                LoginModel login = new LoginModel();
                return View(login);
            }
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public JsonResult Login(LoginModel login)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.CheckLogin(login, false);
            if (response.IsSuccess)
            {
                if (login.IsRemember)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                                                                     login.Email,
                                                                                     DateTime.Now,
                                                                                     DateTime.Now.AddMinutes(
                                                                                         ConfigSettings
                                                                                             .RememberMeDuration),
                                                                                     true,
                                                                                     login.Email,
                                                                                     FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                           encTicket)
                    {
                        Expires = ticket.Expiration
                    };
                    Response.Cookies.Add(httpCookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(login.Email, false);
                }
                SessionValueData sessionValue = (SessionValueData)response.Data;

                SessionHelper.UserID = sessionValue.UserID;
                SessionHelper.Name = sessionValue.Name;
                SessionHelper.FirstName = sessionValue.FirstName;
                SessionHelper.Roles = sessionValue.Roles;
                SessionHelper.SelectedRole = sessionValue.SelectedRole;
            }
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AllInternalUsers)]
        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            const string loggedOutPageUrl = "/security/logout";
            Response.Write("<script language='javascript'>");
            Response.Write("function ClearHistory()");
            Response.Write("{");
            Response.Write(" var backlen=history.length;");
            Response.Write(" history.go(-backlen);");
            Response.Write(" window.location.href='" + loggedOutPageUrl + "'; ");
            Response.Write("}");
            Response.Write("</script>");
            return Redirect("index");
        }


        #endregion

        #region Error Pages

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult InternalError()
        {
            return View();
        }

        #endregion
    }
}
