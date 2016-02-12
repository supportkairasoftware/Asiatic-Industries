using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models;
using Elmah;

namespace AsiaticIndustries.Core.Controllers
{
    public class BaseController : Controller
    {
        public static bool IsOperationsSuspended = false;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var request = HttpContext.Request;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath == "/" ? "" : HttpRuntime.AppDomainAppVirtualPath);
            ViewBag.BasePath = baseUrl;
        }

        public object GetQueryStringValue(string encryptedQueryString, string queryname)
        {
            if (!string.IsNullOrEmpty(encryptedQueryString))
            {
                string invitation = Crypto.Decrypt(encryptedQueryString);
                if (!invitation.Contains("&"))
                {
                    if (invitation.StartsWith(queryname))
                    {
                        string[] values = invitation.Split('=');
                        return values[1];
                    }
                }
                else
                {
                    string[] invitations = invitation.Split('&');
                    return
                        (from inv in invitations where inv.StartsWith(queryname) select inv.Split('=')).Select(
                            values => values[1]).FirstOrDefault();
                }
            }
            return null;
        }

        public List<T> AddEncryptedURL<T>(List<T> items, string queryName) where T : class, new()
        {
            T itm = new T();
            PropertyInfo queryNameInfo = itm.GetType().GetProperty(queryName);
            PropertyInfo encryptedUrlInfo = itm.GetType().GetProperty(Constants.EncryptedQueryString);
            if (queryNameInfo != null && encryptedUrlInfo != null)
            {
                foreach (T item in items)
                {
                    string encryptedQueryString = Crypto.Encrypt(string.Format("{0}={1}", queryName, queryNameInfo.GetValue(item, null)));
                    encryptedUrlInfo.SetValue(item, encryptedQueryString, null);
                }
            }
            return items;
        }

        public List<T> AddEncryptedURL<T>(List<T> items, string[] queryNames) where T : class, new()
        {
            T itm = new T();
            PropertyInfo encryptedUrlInfo = itm.GetType().GetProperty(Constants.EncryptedQueryString);
            if (encryptedUrlInfo != null)
            {
                foreach (T item in items)
                {
                    string encryptedQueryString = "";
                    foreach (string queryName in queryNames)
                    {
                        PropertyInfo queryNameInfo = itm.GetType().GetProperty(queryName);
                        if (queryNameInfo != null)
                            encryptedQueryString += string.Format("{0}={1}&", queryName, queryNameInfo.GetValue(item, null));
                    }
                    if (!string.IsNullOrEmpty(encryptedQueryString))
                        encryptedUrlInfo.SetValue(item, Crypto.Encrypt(encryptedQueryString.TrimEnd('&')), null);
                }
            }

            return items;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var httpContext = filterContext.HttpContext.ApplicationInstance.Context;
                var signal = ErrorSignal.FromContext(httpContext);
                signal.Raise(filterContext.Exception, httpContext);
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            filterContext.Exception.Message
                        }
                    };
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Redirect(Constants.LoginUrl);

                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = new LinkResponse
                    //    {
                    //        Type = Constants.NotAuthorized,
                    //        Link = "/home",
                    //        StatusCode = "401",
                    //        RequestType = filterContext.HttpContext.Request.RequestType.ToLower()
                    //    },
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                }


                if (filterContext.Exception.GetType() == typeof(HttpException))
                {
                    HttpException exception = filterContext.Exception as HttpException;
                    filterContext.HttpContext.Response.StatusCode = exception.GetHttpCode();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public virtual JsonResult Autocomplete(string table, string textField, string valueField, string text, int pageSize, string SearchCriteriaField1 = "", string SearchCriteriaValue1 = "", string SearchCriteriaField2 = "", string SearchCriteriaValue2 = "")
        {
            BaseDataProvider baseDataProvider = new BaseDataProvider();
            return Json(baseDataProvider.GetAutoCompleteList(table, textField, valueField, text, pageSize, SearchCriteriaField1 != "" ? string.Format(" and {0} like '{1}'", SearchCriteriaField1, SearchCriteriaValue1) : "", SearchCriteriaField2 != "" ? string.Format(" and {0} like '{1}'", SearchCriteriaField2, SearchCriteriaValue2) : ""));
        }

        public ActionResult ShowUserFriendlyPages(ServiceResponse response)
        {
            if (!response.IsSuccess)
            {
                if (response.ErrorCode == Constants.ErrorCode_AccessDenied)
                    return View("AccessDenied");
                if (response.ErrorCode == Constants.ErrorCode_InternalError)
                    return View("InternalError");
                if (response.ErrorCode == Constants.ErrorCode_NotFound)
                    return View("NotFound");
                return View("NotFound");
            }
            return null;
        }
    }
}
