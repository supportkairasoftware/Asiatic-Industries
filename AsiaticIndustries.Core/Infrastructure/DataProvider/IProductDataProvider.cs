using System.Collections.Generic;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    public interface IProductDataProvider
    {
        #region Product
        ServiceResponse AddProduct(string encryptedProductId);
        ServiceResponse SaveProduct(ProductAddModel model, long userId, long roleId);
        ServiceResponse ManageProduct();
        ServiceResponse GetProductList(ProductSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);
        ServiceResponse EnabledDisabledProduct(string encryptedProductId, long loggedInUserId);
        #endregion

        #region Sample
        ServiceResponse AddSample(string encryptedSampleId);
        ServiceResponse SaveSample(ProductSampleAddModel model, long userId, long roleId);
        ServiceResponse ManageSample();
        ServiceResponse GetSampleList(ProductSampleSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);
        ServiceResponse EnabledDisabledSample(string encryptedSampleId, long loggedInUserId);
        List<ProductDdlModel> GetProductForSearching(string searchText, string ignoreIds, string pageSize, bool isManualAllowed = true);
        #endregion

         #region Quote
        ServiceResponse AddQuote(string encryptedQuoteId,bool isAdmin, long userId);
        ServiceResponse SaveQuote(QuoteAddModel model, long userId, long roleId);
        ServiceResponse ManageQuote();
        ServiceResponse GetQuoteList(QuoteSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);
        ServiceResponse EnabledDisabledQuote(string encryptedQuoteId, long loggedInUserId);
        ServiceResponse GetQuoteProductPriceModel(QuoteProductPriceSearchModel model, long loogedInUserRoleId);
        List<CustomerDdlModel> GetCustomerForSearching(string searchText, string ignoreIds, string pageSize, bool isManualAllowed = true);
        List<ProductSampleDdlModel> GetProductSampleForSearching(string searchText, string ignoreIds, string pageSize, bool isManualAllowed = true);
        #endregion
        

        #region Raw Material

        ServiceResponse AddRawMaterial(string encryptedId);
        ServiceResponse ManageRawMaterial();
        ServiceResponse SaveRawMaterial(RawMaterial model, long loggedInUserId);
        ServiceResponse GetRawMaterialList(RawMaterialSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);
        ServiceResponse EnabledDisabledRawMaterial(long rawMaterialId, long loggedInUserId);
        ServiceResponse GetRawMaterialListForGraph(string encryptedRawMaterialID, int pageSize, int pageIndex, string sortIndex, string sortDirection);

        #endregion Raw Material
    }
}
