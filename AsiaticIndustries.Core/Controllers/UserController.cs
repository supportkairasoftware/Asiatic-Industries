using System;
using System.Web.Mvc;
using System.Web.Security;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AllInternalUsers)]
    public class UserController : BaseController
    {
        private IUserDataProvider _userDataProvider;

        #region User Region

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AllInternalUsers, NgReturnUrl = Constants.NgReturnUrl_MyAccount)]
        public ActionResult MyAccount()
        {
            return RedirectToAction("adduser", "user", new
                {
                    id = Crypto.Encrypt(SessionHelper.UserID.ToString()),
                    id1 = "account"
                });
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AllInternalUsers, NgReturnUrl = Constants.NgReturnUrl_AddUser)]
        public ActionResult AddUser(string id = "",string id1="")
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");

            if (id1 != "account")
            {
                if (SessionHelper.SelectedRole.RoleID != Constants.Role_Admin)
                    return View("AccessDenied");
            }

            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.AddUser(id, SessionHelper.Roles, SessionHelper.SelectedRole);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AllInternalUsers)]
        public JsonResult SaveUser(User model)
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.SaveUser(model, SessionHelper.UserID);
            if (response.IsSuccess && response.Data != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.OnlyAdmin, NgReturnUrl = Constants.NgReturnUrl_ListUser)]
        public ActionResult ManageUser()
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.ManageUser(SessionHelper.Roles);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.OnlyAdmin)]
        public JsonResult GetUserList(UserSearchModel searchParams, int pageSize = 10, int pageIndex = 1, string sortIndex = "", string sortDirection = "")
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.GetUserList(searchParams, pageSize, pageIndex, sortIndex, sortDirection);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.OnlyAdmin)]
        public JsonResult EnabledDisabledUser(string id)
        {
            _userDataProvider = new UserDataProvider();
            long userId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _userDataProvider.EnabledDisabledUser(userId, SessionHelper.UserID);
            return Json(response);
        }

        #endregion User Region

        #region Customer Region

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep, NgReturnUrl = Constants.NgReturnUrl_AddCustomer)]
        public ActionResult AddCustomer(string id = "")
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");

            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.AddCustomer(id);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep)]
        public JsonResult SaveCustomer(Customer model)
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.SaveCustomer(model, SessionHelper.UserID);
            if (response.IsSuccess && response.Data != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep, NgReturnUrl = Constants.NgReturnUrl_ListCustomer)]
        public ActionResult ManageCustomer()
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.ManageCustomer();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep)]
        public JsonResult GetCustomerList(CustomerSearchModel searchParams, int pageSize = 10, int pageIndex = 1, string sortIndex = "", string sortDirection = "")
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.GetCustomerList(searchParams, pageSize, pageIndex, sortIndex, sortDirection);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep)]
        public JsonResult EnabledDisabledCustomer(string id)
        {
            _userDataProvider = new UserDataProvider();
            long customerId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _userDataProvider.EnabledDisabledCustomer(customerId, SessionHelper.UserID);
            return Json(response);
        }

        #endregion Customer Region

        #region USD Master

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_QuoteManager)]
        public JsonResult SaveUSDMaster(CurrentUSDRateHistory model)
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.SaveUSDMaster(model, SessionHelper.UserID);
            if (response.IsSuccess && response.Data != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_QuoteManager, NgReturnUrl = Constants.NgReturnUrl_UsdMaster)]
        public ActionResult USDMaster()
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.ManageUSDMaster();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_QuoteManager)]
        public ActionResult GetUSDMasterList(CurrentUSDRateSearchModel searchParams, int pageSize = 10, int pageIndex = 1, string sortIndex = "", string sortDirection = "")
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.GetUSDMasterList(searchParams, pageSize, pageIndex, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_QuoteManager)]
        public ActionResult GetGraphUSDMasterList(int pageSize = 99999, int pageIndex = 1, string sortIndex = "", string sortDirection = "")
        {
            _userDataProvider = new UserDataProvider();
            ServiceResponse response = _userDataProvider.GetGraphUSDMasterList(pageSize, pageIndex, sortIndex, sortDirection);
            return Json(response);
        }

        #endregion USD Master
    }
}