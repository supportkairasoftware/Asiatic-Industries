using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;
using System.Web.Security;

namespace AsiaticIndustries.Core.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AllLoggedInUserPermission)]
    public class ProductController : BaseController
    {
        private IProductDataProvider _productDataProvider;

        #region Product

        [CustomAuthorize(Permissions = Constants.OnlyAdmin, NgReturnUrl = Constants.NgReturnUrl_AddProduct)]
        public ActionResult AddProduct(string id)
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");


            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.AddProduct(id);

            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.OnlyAdmin)]
        public ActionResult SaveProduct(ProductAddModel model)
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.SaveProduct(model, SessionHelper.UserID,
                                                                        SessionHelper.SelectedRole.RoleID);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.OnlyAdmin, NgReturnUrl = Constants.NgReturnUrl_ListProduct)]
        public ActionResult ManageProduct()
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.ManageProduct();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.OnlyAdmin)]
        public JsonResult GetProductList(ProductSearchModel searchParams, int pageSize = 10, int pageIndex = 1,
                                         string sortIndex = "", string sortDirection = "")
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.GetProductList(searchParams, pageSize, pageIndex, sortIndex,
                                                                           sortDirection);
            return Json(response);
        }


        [CustomAuthorize(Permissions = Constants.OnlyAdmin)]
        public JsonResult EnabledDisabledProduct(string id)
        {
            _productDataProvider = new ProductDataProvider();

            ServiceResponse response = _productDataProvider.EnabledDisabledProduct(id, SessionHelper.UserID);
            return Json(response);
        }

        #endregion Product

        #region Sample

        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant, NgReturnUrl = Constants.NgReturnUrl_AddSample)]
        public ActionResult AddSample(string id)
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");


            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.AddSample(id);

            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant)]
        public ActionResult SaveSample(ProductSampleAddModel model)
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.SaveSample(model, SessionHelper.UserID,
                                                                       SessionHelper.SelectedRole.RoleID);
            return Json(response);
        }


        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant_QuoteManager, NgReturnUrl = Constants.NgReturnUrl_ListSample)]
        public ActionResult ManageSample()
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.ManageSample();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant_QuoteManager)]
        public JsonResult GetSampleList(ProductSampleSearchModel searchParams, int pageSize = 10, int pageIndex = 1,
                                        string sortIndex = "", string sortDirection = "")
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.GetSampleList(searchParams, pageSize, pageIndex, sortIndex,
                                                                          sortDirection);
            return Json(response);
        }


        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant)]
        public JsonResult EnabledDisabledSample(string id)
        {
            _productDataProvider = new ProductDataProvider();

            ServiceResponse response = _productDataProvider.EnabledDisabledSample(id, SessionHelper.UserID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_MarketingRep_LabAssistant)]
        public JsonResult GetProductForSearching(string searchText, string ignoreIds, string pageSize)
        {
            _productDataProvider = new ProductDataProvider();
            List<ProductDdlModel> productList = _productDataProvider.GetProductForSearching(searchText.ToLower(),
                                                                                            ignoreIds.ToLower(),
                                                                                            pageSize, false);
            return Json(productList);
        }

        #endregion

        #region Quote

        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager, NgReturnUrl = Constants.NgReturnUrl_AddQuote)]
        public ActionResult AddQuote(string id)
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");

            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.AddQuote(id,SessionHelper.SelectedRole.RoleID==Constants.Role_Admin,SessionHelper.UserID);

            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public ActionResult SaveQuote(QuoteAddModel model)
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.SaveQuote(model, SessionHelper.UserID,
                                                                      SessionHelper.SelectedRole.RoleID);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager, NgReturnUrl = Constants.NgReturnUrl_ListQuote)]
        public ActionResult ManageQuote()
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.ManageQuote();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public JsonResult GetQuoteList(QuoteSearchModel searchParams, int pageSize = 10, int pageIndex = 1,
                                       string sortIndex = "", string sortDirection = "")
        {
            _productDataProvider = new ProductDataProvider();


            searchParams.IsAdmin = SessionHelper.SelectedRole.RoleID == Constants.Role_Admin;
            searchParams.CreatedBy = SessionHelper.UserID;

            ServiceResponse response = _productDataProvider.GetQuoteList(searchParams, pageSize, pageIndex, sortIndex,
                                                                         sortDirection);
            return Json(response);
        }


        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public JsonResult EnabledDisabledQuote(string id)
        {
            _productDataProvider = new ProductDataProvider();

            ServiceResponse response = _productDataProvider.EnabledDisabledQuote(id, SessionHelper.UserID);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public JsonResult GetQuoteProductPriceModel(QuoteProductPriceSearchModel model)
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.GetQuoteProductPriceModel(model,SessionHelper.SelectedRole.RoleID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public JsonResult GetCustomerForSearching(string searchText, string ignoreIds, string pageSize)
        {
            _productDataProvider = new ProductDataProvider();
            List<CustomerDdlModel> productList = _productDataProvider.GetCustomerForSearching(searchText.ToLower(),
                                                                                              ignoreIds.ToLower(),
                                                                                              pageSize, false);
            return Json(productList);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_QuoteManager)]
        public JsonResult GetProductSampleForSearching(string searchText, string ignoreIds, string pageSize)
        {
            _productDataProvider = new ProductDataProvider();
            List<ProductSampleDdlModel> productList =
                _productDataProvider.GetProductSampleForSearching(searchText.ToLower(), ignoreIds.ToLower(), pageSize,
                                                                  false);
            return Json(productList);
        }

        #endregion
        
        #region Raw Material

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager, NgReturnUrl = Constants.NgReturnUrl_AddRawMaterial)]
        public ActionResult AddRawMaterial(string id = "")
        {
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(Crypto.Decrypt(id)))
                return View("NotFound");

            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.AddRawMaterial(id);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager)]
        public JsonResult SaveRawMaterial(RawMaterial model)
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.SaveRawMaterial(model, SessionHelper.UserID);
            if (response.IsSuccess && response.Data != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager, NgReturnUrl = Constants.NgReturnUrl_ListRawMaterial)]
        public ActionResult ManageRawMaterial()
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.ManageRawMaterial();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager)]
        public JsonResult GetRawMaterialList(RawMaterialSearchModel searchParams, int pageSize = 10, int pageIndex = 1,
                                             string sortIndex = "", string sortDirection = "")
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.GetRawMaterialList(searchParams, pageSize, pageIndex,
                                                                               sortIndex, sortDirection);
            return Json(response);
        }

        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager)]
        public JsonResult EnabledDisabledRawMaterial(string id)
        {
            _productDataProvider = new ProductDataProvider();
            long rawMaterialId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _productDataProvider.EnabledDisabledRawMaterial(rawMaterialId,
                                                                                       SessionHelper.UserID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Only_Admin_RawManager)]
        public JsonResult GetRawMaterialListForGraph(string encryptedRawMaterialID, int pageSize = 10, int pageIndex = 1,
                                             string sortIndex = "", string sortDirection = "")
        {
            _productDataProvider = new ProductDataProvider();
            ServiceResponse response = _productDataProvider.GetRawMaterialListForGraph(encryptedRawMaterialID, pageSize, pageIndex,
                                                                               sortIndex, sortDirection);
            return Json(response);
        }

        #endregion Raw Material
    }
}
