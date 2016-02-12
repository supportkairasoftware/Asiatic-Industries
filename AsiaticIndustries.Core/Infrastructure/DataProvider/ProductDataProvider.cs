using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    internal class ProductDataProvider : BaseDataProvider, IProductDataProvider
    {
        #region Product

        public ServiceResponse AddProduct(string encryptedProductId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                long productId = Convert.ToInt64(Crypto.Decrypt(encryptedProductId));
                ProductAddModel model = GetMultipleEntity<ProductAddModel>("GetProductAddModel",
                                                                           new List<SearchValueData>
                                                                               {
                                                                                   new SearchValueData
                                                                                       {
                                                                                           Name = "ProductID",
                                                                                           Value =
                                                                                               Convert.ToString(
                                                                                                   productId),
                                                                                           IsEqual = true
                                                                                       }
                                                                               });
                if (productId != 0)
                {
                    if (model == null || model.Product == null)
                    {
                        response.IsSuccess = false;
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        response.Message = Resource.ExceptionMessage;
                        return response;
                    }
                }


                if (model != null)
                {
                    if (model.Product == null)
                        model.Product = new Product();
                    else
                    {

                        if (model.Product.IsThirdPartyProduct)
                        {
                            model.Product.CrudePricePerKg = model.Product.CrudePrice;
                            model.Product.TempProfitPerKg = model.Product.ProfitPerKg;
                        }
                    }

                    if (model.ListOfProductItem.Count > 0)
                    {
                        foreach (var productItem in model.ListOfProductItem)
                            productItem.StrProductItemID = Convert.ToString(productItem.ProductItemID);


                        List<string> productItemIds = model.ListOfProductItem.Select(m => m.StrProductItemID).ToList();
                        model.OldProductItemIds = productItemIds;
                        model.NewProductItemIds = productItemIds;


                    }

                    response.IsSuccess = true;
                    response.Data = model;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse SaveProduct(ProductAddModel model, long userId, long roleId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {

                bool isEditMode = model.Product.ProductID > 0;


                string customWhere = isEditMode ? string.Format("ProductID!={0}", model.Product.ProductID) : "";

                var searchListValueData = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "ProductName", Value = model.Product.ProductName, IsEqual = true}
                    };
                Product existingProduct = GetEntity<Product>(searchListValueData, customWhere);

                if (existingProduct != null)
                {
                    response.Message =
                        Common.MessageWithTitle(isEditMode ? Resource.UpdateProductFailed : Resource.AddProductFailed,
                                                Resource.ProductExistWithThisProductName);
                }
                else
                {

                    if (!isEditMode)
                    {
                        model.Product.IsActive = true;
                    }

                    if (model.Product.IsThirdPartyProduct)
                    {
                        model.Product.CrudePrice = model.Product.CrudePricePerKg;
                        model.Product.ProfitPerKg = model.Product.TempProfitPerKg;
                    }

                    SaveObject(model.Product, userId);

                    string[] newProductItemIds = new string[] { }, oldProductItemIds = new string[] { };

                    if (model.OldProductItemIds.Count > 0)
                    {
                        oldProductItemIds = model.OldProductItemIds.ToArray();
                    }
                    if (model.NewProductItemIds.Count > 0)
                    {
                        newProductItemIds = model.NewProductItemIds.ToArray();
                    }

                    IEnumerable<string> idsToAdd = newProductItemIds.Except(oldProductItemIds).ToArray();
                    IEnumerable<string> idsToDelete = oldProductItemIds.Except(newProductItemIds).ToArray();

                    // bool updateItemTag = (idsToAdd.Any() || idsToDelete.Any());

                    if (idsToDelete.Any())
                    {
                        List<SearchValueData> searchList = new List<SearchValueData>();
                        SearchValueData searchValueData = new SearchValueData
                            {
                                Name = "ProductID",
                                Value = model.Product.ProductID.ToString()
                            };
                        searchList.Add(searchValueData);

                        //searchValueData = new SearchValueData { Name = "ProductToAdd ", Value = string.Join(",", idsToAdd) };
                        //searchList.Add(searchValueData);

                        searchValueData = new SearchValueData
                            {
                                Name = "ProductToDelete",
                                Value = string.Join(",", idsToDelete)
                            };
                        searchList.Add(searchValueData);
                        GetScalar("SaveProductItems", searchList);
                    }

                    if (idsToAdd.Any())
                    {
                        //List<RawMaterial> rawMaterials = GetEntityList<RawMaterial>(new List<SearchValueData> { new SearchValueData { Name = "IsActive", Value = "1", IsEqual = true } });
                        foreach (var id in idsToAdd)
                        {
                            ProductItem productItem =
                                model.ListOfProductItem.First(c => c.StrProductItemID == Convert.ToString(id).Trim());

                            if (productItem.RawMaterialID != 0)
                            {
                                productItem.ProductID = model.Product.ProductID;
                                //productItem.TotalPrice=productItem.NumberOfKgs
                                SaveEntity(productItem);
                            }
                        }
                    }


                    foreach (var productItem in model.ListOfProductItem)
                    {
                        if (productItem.RawMaterialID != 0)
                        {
                            SaveEntity(productItem);
                        }
                    }


                    response.IsSuccess = true;
                    //response.Data = Crypto.Encrypt(Convert.ToString(model.Product.ProductID));
                    response.Message = Common.MessageWithTitle(Resource.ProductSaveSucceeded,
                                                               Resource.ProductSavedSuccefully);
                }

            }
            catch (Exception ex)
            {
                response.Message = Resource.ExceptionMessage;
                return response;
            }
            return response;
        }

        public ServiceResponse ManageProduct()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                ProductSearchModel searchModel = new ProductSearchModel();
                searchModel.ListOfRawMaterial =
                    GetEntityList<RawMaterial>(new List<SearchValueData>
                        {
                            new SearchValueData {Name = "IsActive", Value = "1", IsEqual = true}
                        });
                response.IsSuccess = true;
                response.Data = searchModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;

        }

        public ServiceResponse GetProductList(ProductSearchModel searchParams, int pageSize, int pageIndex,
                                              string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData
                            {
                                Name = "FromIndex",
                                Value = Convert.ToString(((pageIndex - 1)*pageSize) + 1)
                            },
                        new SearchValueData {Name = "ToIndex", Value = Convert.ToString(pageIndex*pageSize)},
                        new SearchValueData
                            {
                                Name = "SortExpression",
                                Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)
                            },
                        new SearchValueData
                            {
                                Name = "SortType",
                                Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)
                            },
                        new SearchValueData {Name = "ProductName", Value = Convert.ToString(searchParams.ProductName)},
                        new SearchValueData
                            {
                                Name = "RawMaterialID",
                                Value = Convert.ToString(searchParams.RawMaterialID)
                            },
                    };
                List<ProductViewModel> totalData = GetEntityList<ProductViewModel>("GetProductList", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ProductViewModel> product = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = product;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse EnabledDisabledProduct(string encryptedProductId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                long productId = !string.IsNullOrEmpty(encryptedProductId)
                                     ? Convert.ToInt32(Crypto.Decrypt(encryptedProductId))
                                     : 0;

                Product data = GetEntity<Product>(productId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);

                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.ProductEnableSucceeded : Resource.ProductDisableSucceeded,
                        data.IsActive ? Resource.ProductEnabledSuccessfully : Resource.ProductDisabledSuccessfully);

                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                    response.IsSuccess = false;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        #endregion

        #region Sample

        public ServiceResponse AddSample(string encryptedSampleId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                long sampleId = Convert.ToInt64(Crypto.Decrypt(encryptedSampleId));

                ProductSampleAddModel model = new ProductSampleAddModel();
                if (sampleId > 0)
                    model = GetMultipleEntity<ProductSampleAddModel>("GetProductSampleAddModel",
                                                                     new List<SearchValueData>
                                                                         {
                                                                             new SearchValueData
                                                                                 {
                                                                                     Name = "ProductSampleID",
                                                                                     Value = Convert.ToString(sampleId),
                                                                                     IsEqual = true
                                                                                 }
                                                                         });



                if (sampleId != 0)
                {
                    if (model.ProductSample == null)
                    {
                        response.IsSuccess = false;
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        response.Message = Resource.ExceptionMessage;
                        return response;
                    }
                }

                if (model.ProductSample == null)
                    model = new ProductSampleAddModel();


                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse SaveSample(ProductSampleAddModel model, long userId, long roleId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                bool isEditMode = model.ProductSample.ProductSampleID > 0;
                string customWhere = isEditMode
                                         ? string.Format("ProductSampleID!={0}", model.ProductSample.ProductSampleID)
                                         : "";
                var searchListValueData = new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "ReferenceNumber",
                                Value = model.ProductSample.ReferenceNumber,
                                IsEqual = true
                            }
                    };
                ProductSample existingProductSample = GetEntity<ProductSample>(searchListValueData, customWhere);

                if (existingProductSample != null)
                {
                    response.Message =
                        Common.MessageWithTitle(
                            isEditMode ? Resource.UpdateProductSampleFailed : Resource.AddProductSampleFailed,
                            Resource.ProductSampleExistWithThisReferenceNumber);
                }
                else
                {

                    if (!isEditMode)
                    {
                        model.ProductSample.IsActive = true;
                    }

                    SaveObject(model.ProductSample, userId);

                    response.IsSuccess = true;
                    response.Message = Common.MessageWithTitle(Resource.ProductSampleSaveSucceeded,
                                                               Resource.ProductSampleSavedSuccefully);
                }

            }
            catch (Exception ex)
            {
                response.Message = Resource.ExceptionMessage;
                return response;
            }
            return response;
        }

        public ServiceResponse ManageSample()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                ProductSampleSearchModel searchModel = new ProductSampleSearchModel();
                response.IsSuccess = true;
                response.Data = searchModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;

        }

        public ServiceResponse GetSampleList(ProductSampleSearchModel searchParams, int pageSize, int pageIndex,
                                             string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData
                            {
                                Name = "FromIndex",
                                Value = Convert.ToString(((pageIndex - 1)*pageSize) + 1)
                            },
                        new SearchValueData {Name = "ToIndex", Value = Convert.ToString(pageIndex*pageSize)},
                        new SearchValueData
                            {
                                Name = "SortExpression",
                                Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)
                            },
                        new SearchValueData
                            {
                                Name = "SortType",
                                Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)
                            },
                        new SearchValueData {Name = "CustomerName", Value = Convert.ToString(searchParams.CustomerName)},
                        new SearchValueData
                            {
                                Name = "ProductIDs",
                                Value = Convert.ToString(string.Join(",", searchParams.ProductIDs))
                            },
                        new SearchValueData
                            {
                                Name = "ReferenceNumber",
                                Value = Convert.ToString(searchParams.ReferenceNumber)
                            },
                    };
                List<ProductSampleViewModel> totalData = GetEntityList<ProductSampleViewModel>("GetSampleList",
                                                                                               searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<ProductSampleViewModel> product = GetPageInStoredProcResultSet(pageIndex, pageSize, count,
                                                                                    totalData);
                response.Data = product;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse EnabledDisabledSample(string encryptedSampleId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                long sampleId = !string.IsNullOrEmpty(encryptedSampleId)
                                    ? Convert.ToInt32(Crypto.Decrypt(encryptedSampleId))
                                    : 0;

                ProductSample data = GetEntity<ProductSample>(sampleId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);

                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.ProductSampleEnableSucceeded : Resource.ProductSampleDisableSucceeded,
                        data.IsActive
                            ? Resource.ProductSampleEnabledSuccessfully
                            : Resource.ProductSampleDisabledSuccessfully);

                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                    response.IsSuccess = false;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }


        public List<ProductDdlModel> GetProductForSearching(string searchText, string ignoreIds, string pageSize,
                                                            bool isManualAllowed = true)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                string searchKey = "";
                foreach (var id in ignoreIds.Split(','))
                {
                    long n;
                    bool isNumeric = long.TryParse(id, out n);
                    if (isNumeric)
                        searchKey = searchKey + id + ",";
                }

                searchKey = searchKey.Trim(',');

                // string customeWhere = "TagID not in (" + searchKey + ")";
                List<ProductDdlModel> masterProductlist =
                    GetEntityList<ProductDdlModel>("GetProductDdlModel",
                                                   new List<SearchValueData>
                                                       {
                                                           new SearchValueData {Name = "ProductName", Value = searchText}
                                                       })
                        .ToList()
                        .Take(Convert.ToInt16(pageSize))
                        .ToList();
                List<ProductDdlModel> productlist =
                    masterProductlist.Where(c => !searchKey.Contains(c.ProductID.ToString())).ToList();

                foreach (var tag in productlist)
                    tag.StrProductID = Convert.ToString(tag.ProductID);

                if (isManualAllowed && ignoreIds.Split(',').All(c => c != searchText))
                {

                    if (masterProductlist.Any(c => c.ProductName.ToLower() == searchText) == false)
                    {
                        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                        productlist.Insert(0,
                                           new ProductDdlModel
                                               {
                                                   StrProductID = myTI.ToTitleCase(searchText),
                                                   ProductName = myTI.ToTitleCase(searchText)
                                               });
                    }
                }
                response.IsSuccess = true;
                response.Data = productlist;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }



            return (List<ProductDdlModel>)response.Data;
        }

        #endregion

        #region Quote

        public ServiceResponse AddQuote(string encryptedQuoteId, bool isAdmin, long userId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                long quoteId = Convert.ToInt64(Crypto.Decrypt(encryptedQuoteId));

                QuoteAddModel model = new QuoteAddModel();
                //quoteId = 6;
                if (quoteId > 0)
                    model = GetMultipleEntity<QuoteAddModel>("GetQuoteAddModel", new List<SearchValueData>
                        {
                            new SearchValueData
                                {
                                    Name = "QuoteID",
                                    Value = Convert.ToString(quoteId),
                                    IsEqual = true
                                }
                        });



                if (quoteId != 0)
                {
                    if (model.Quote == null)
                    {
                        response.IsSuccess = false;
                        response.ErrorCode = Constants.ErrorCode_NotFound;
                        response.Message = Resource.ExceptionMessage;
                        return response;
                    }
                    if (isAdmin==false)
                    {
                        if (model.Quote.CreatedBy != userId)
                        {
                            response.IsSuccess = false;
                            response.ErrorCode = Constants.ErrorCode_AccessDenied;
                            return response;
                        }
                    }
                    if (model.PrepopulateProductSamples != null)
                    {
                        foreach (var prepopulateProductSample in model.PrepopulateProductSamples)
                            prepopulateProductSample.QuoteID = model.Quote.QuoteID;

                        List<string> groupIDs =
                            model.PrepopulateProductSamples.Select(m => m.StrProductSampleID).ToList();
                        model.OldProductSampleIds = groupIDs;
                        model.NewProductSampleIds = groupIDs;
                    }
                    else
                    {
                        model.OldProductSampleIds = new List<string>();
                        model.NewProductSampleIds = new List<string>();
                    }

                }

                if (model.Quote == null)
                {
                    model = new QuoteAddModel();

                }

                CurrentUSDRateHistory currentUsdRateHistory =
                    GetEntity<CurrentUSDRateHistory>(new List<SearchValueData>
                        {
                            new SearchValueData {Name = "IsActive", Value = "1", IsEqual = true}
                        });
                if (quoteId > 0)
                {
                    model.CurrentUSDRate = model.Quote.USDRate;
                    if (currentUsdRateHistory != null)
                        model.NewUSDRate = currentUsdRateHistory.CurrentUSDRate;
                }
                else
                {

                    if (currentUsdRateHistory != null)
                    {
                        model.NewUSDRate = currentUsdRateHistory.CurrentUSDRate;
                        model.Quote.USDRate = currentUsdRateHistory.CurrentUSDRate;
                        model.CurrentUSDRate = model.Quote.USDRate;
                    }
                }

                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse SaveQuote(QuoteAddModel model, long userId, long roleId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                bool isEditMode = model.Quote.QuoteID > 0;
                if (!isEditMode)
                    model.Quote.IsActive = true;
                model.Quote.USDRate = model.QuoteDetail.USDRate;
                SaveObject(model.Quote, userId);


                string[] newProductSampleIds = new string[] { }, oldProductSampleIds = new string[] { };

                if (model.OldProductSampleIds!=null && model.OldProductSampleIds.Count > 0)
                {
                    oldProductSampleIds = model.OldProductSampleIds.ToArray();
                }
                if (model.NewProductSampleIds != null && model.NewProductSampleIds.Count > 0)
                {
                    newProductSampleIds = model.NewProductSampleIds.ToArray();
                }




                IEnumerable<string> idsToAdd = newProductSampleIds.Except(oldProductSampleIds).ToArray();
                IEnumerable<string> idsToDelete = oldProductSampleIds.Except(newProductSampleIds).ToArray();

                // string.Join(",", idsToDelete)



                GetScalar("SaveQuoteProducts", new List<SearchValueData>
                    {
                        new SearchValueData
                            {
                                Name = "IsPriceWillRevised",
                                Value = Convert.ToString(model.Quote.IsPriceWillRevised),
                                IsEqual = true
                            },
                        new SearchValueData
                            {
                                Name = "DeleteQuoteProducts",
                                Value = string.Join(",", idsToDelete),
                                IsEqual = true
                            },
                        new SearchValueData
                            {
                                Name = "QuoteID",
                                Value = Convert.ToString(model.Quote.QuoteID),
                                IsEqual = true
                            },
                        new SearchValueData
                            {
                                Name = "ProductSampleIDs",
                                Value =
                                    model.Quote.IsPriceWillRevised
                                        ? model.StrPrepopulateProductSamples
                                        : string.Join(",", idsToAdd),
                                IsEqual = true
                            }
                    });



                QuoteDetail quoteDetail = isEditMode ? GetEntity<QuoteDetail>(new List<SearchValueData> { new SearchValueData { Name = "QuoteID", Value = Convert.ToString(model.Quote.QuoteID), IsEqual = true } }) ?? new QuoteDetail() : new QuoteDetail();

                quoteDetail.TotalCrudePerkg = model.QuoteDetail.TotalCrudePerkg;
                quoteDetail.TotalMFGOH = model.QuoteDetail.TotalMFGOH;
                quoteDetail.TotalADMOH = model.QuoteDetail.TotalADMOH;
                quoteDetail.TotalPackingCost = model.QuoteDetail.TotalPackingCost;
                quoteDetail.TotalFreightCost = model.QuoteDetail.TotalFreightCost;
                quoteDetail.TotalForwardingCost = model.QuoteDetail.TotalForwardingCost;
                quoteDetail.TotalInterest = model.QuoteDetail.TotalInterest;
                quoteDetail.TotalProfit = model.QuoteDetail.TotalProfit;
                quoteDetail.TotalNett = model.QuoteDetail.TotalNett;
                quoteDetail.USDRate = model.QuoteDetail.USDRate;
                quoteDetail.TotalNettInUSD = model.QuoteDetail.TotalNettInUSD;
                quoteDetail.Discount = model.QuoteDetail.Discount;
                quoteDetail.DiscountedPrice = model.QuoteDetail.DiscountedPrice;
                quoteDetail.Commission = model.QuoteDetail.Commission;
                quoteDetail.FinalPrice = model.QuoteDetail.FinalPrice;
                quoteDetail.QuoteID = model.Quote.QuoteID;

                SaveEntity(quoteDetail);

                response.IsSuccess = true;
                response.Message = Common.MessageWithTitle(Resource.QuoteSaveSucceeded,
                                                           Resource.QuoteSavedSuccefully);


            }
            catch (Exception ex)
            {
                response.Message = Resource.ExceptionMessage;
                return response;
            }
            return response;
        }

        public ServiceResponse ManageQuote()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                QuoteSearchModel searchModel = new QuoteSearchModel();
                response.IsSuccess = true;
                response.Data = searchModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;

        }

        public ServiceResponse GetQuoteList(QuoteSearchModel searchParams, int pageSize, int pageIndex, string sortIndex,
                                            string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData
                            {
                                Name = "FromIndex",
                                Value = Convert.ToString(((pageIndex - 1)*pageSize) + 1)
                            },
                        new SearchValueData {Name = "ToIndex", Value = Convert.ToString(pageIndex*pageSize)},
                        new SearchValueData{Name = "SortExpression",Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)},
                        new SearchValueData{Name = "SortType",Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)},
                        new SearchValueData {Name = "CustomerName", Value = Convert.ToString(searchParams.CustomerName)},
                        new SearchValueData{Name = "ProductIDs",Value = Convert.ToString(string.Join(",", searchParams.ProductIDs))},
                        new SearchValueData{Name = "ProductSampleIDs",Value = Convert.ToString(string.Join(",", searchParams.ProductSampleIDs))},
                        new SearchValueData{Name = "IsAdmin",Value = Convert.ToString(searchParams.IsAdmin)},
                        new SearchValueData{Name = "CreatedBy",Value = Convert.ToString(searchParams.CreatedBy)}
                        
                    };
                List<QuoteViewModel> totalData = GetEntityList<QuoteViewModel>("GetQuoteList",
                                                                                               searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<QuoteViewModel> product = GetPageInStoredProcResultSet(pageIndex, pageSize, count,
                                                                                    totalData);
                response.Data = product;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse EnabledDisabledQuote(string encryptedQuoteId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();

            try
            {
                long quoteId = !string.IsNullOrEmpty(encryptedQuoteId)
                                   ? Convert.ToInt32(Crypto.Decrypt(encryptedQuoteId))
                                   : 0;

                Quote data = GetEntity<Quote>(quoteId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);

                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.QuoteEnableSucceeded : Resource.QuoteDisableSucceeded,
                        data.IsActive ? Resource.QuoteEnabledSuccessfully : Resource.QuoteDisabledSuccessfully);

                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = Resource.ExceptionMessage;
                    response.IsSuccess = false;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }


        public ServiceResponse GetQuoteProductPriceModel(QuoteProductPriceSearchModel model,long loogedInUserRoleId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                if (model.IsPriceWillRevised)
                {
                    model.QuoteID = 0;
                    
                }
                QuoteProductModel dataModel = GetMultipleEntity<QuoteProductModel>("GetQuoteProductModel",
                                                                                   new List<SearchValueData>
                                                                                       {
                                                                                           new SearchValueData
                                                                                               {
                                                                                                   Name = "ProductSampleID",
                                                                                                   Value =
                                                                                                       Convert.ToString(
                                                                                                           model
                                                                                                               .ProductSampleID)
                                                                                               },
                                                                                           new SearchValueData
                                                                                               {
                                                                                                   Name = "QuoteID",
                                                                                                   Value =
                                                                                                       Convert.ToString(
                                                                                                           model.QuoteID)
                                                                                               }
                                                                                       });

                QuoteProductPriceModel qpPriceModel = new QuoteProductPriceModel();
                if (dataModel != null && dataModel.ProductSample != null)
                {

                    //if (model.IsPriceWillRevised)
                    //{
                    //    CurrentUSDRateHistory currentUsdRateHistory =
                    //        GetEntity<CurrentUSDRateHistory>(new List<SearchValueData>
                    //        {
                    //            new SearchValueData {Name = "IsActive", Value = "1", IsEqual = true}
                    //        });
                    //    if (currentUsdRateHistory != null)
                    //    {
                    //        qpPriceModel.USDRate = currentUsdRateHistory.CurrentUSDRate;
                    //    }
                    //}

                    qpPriceModel.ProductSampleID = dataModel.ProductSample.ProductSampleID;
                    qpPriceModel.ReferenceNumber = dataModel.ProductSample.ReferenceNumber;
                    qpPriceModel.Description = dataModel.ProductSample.Description;
                    qpPriceModel.ProductConstraints = dataModel.ProductSample.ProductConstraints;

                    qpPriceModel.ProductID = dataModel.Product.ProductID;
                    qpPriceModel.ProductName = dataModel.Product.ProductName;


                    qpPriceModel.ProfitPerKg = dataModel.Product.ProfitPerKg;
                    qpPriceModel.MfgOverHeadPrice = dataModel.Product.MfgOverHeadPrice;
                    qpPriceModel.AdminOverHeadPrice = dataModel.Product.AdminOverHeadPrice;


                    if (dataModel.Product.IsThirdPartyProduct)
                    {
                        qpPriceModel.CrudePerKg = dataModel.Product.CrudePrice;
                        qpPriceModel.Profit = dataModel.Product.ProfitPerKg;
                    }
                    else
                    {
                        qpPriceModel.CrudePrice = dataModel.Product.CrudePrice;
                        qpPriceModel.Profit = qpPriceModel.ProfitPerKg;

                        //CrudePerKg MFGOH ADMOH Profit TotalProductCost
                        decimal productContraintsPrice =
                            Math.Round(
                                ((Convert.ToDecimal(qpPriceModel.CrudePrice) * 100) /
                                 Convert.ToDecimal(qpPriceModel.ProductConstraints)), 2);
                        decimal totalRawMaterialPrice = dataModel.ListOfProductRawMaterial.Sum(c => c.TotalPrice);
                        qpPriceModel.TotalRawMaterialsCost = Convert.ToString(totalRawMaterialPrice);
                        qpPriceModel.CrudePerKg = Math.Round(totalRawMaterialPrice / productContraintsPrice, 2).ToString();

                        qpPriceModel.ADMOH =
                            Math.Round(
                                ((Convert.ToDecimal(qpPriceModel.ProductConstraints) *
                                  Convert.ToDecimal(qpPriceModel.AdminOverHeadPrice)) / 100), 2).ToString();
                        qpPriceModel.MFGOH =
                            Math.Round(
                                ((Convert.ToDecimal(qpPriceModel.ProductConstraints) *
                                  Convert.ToDecimal(qpPriceModel.MfgOverHeadPrice)) / 100), 2).ToString();

                    }
                    qpPriceModel.TotalProductCost =
                        Math.Round(
                            Convert.ToDecimal(qpPriceModel.CrudePerKg) + Convert.ToDecimal(qpPriceModel.ADMOH) +
                            Convert.ToDecimal(qpPriceModel.MFGOH) + Convert.ToDecimal(qpPriceModel.Profit), 2)
                            .ToString();



                    if (SessionHelper.UserID != Constants.Role_Admin)
                    {
                        QuoteProductPriceModel newQuotePriceModel = new QuoteProductPriceModel();
                        newQuotePriceModel.ProductID = qpPriceModel.ProductID;
                        newQuotePriceModel.ProductName = qpPriceModel.ProductName;
                        newQuotePriceModel.ProductConstraints = qpPriceModel.ProductConstraints;
                        newQuotePriceModel.ReferenceNumber = qpPriceModel.ReferenceNumber;
                        newQuotePriceModel.TotalProductCost = qpPriceModel.TotalProductCost;

                        response.IsSuccess = true;
                        response.Data = newQuotePriceModel;
                    }


                    response.IsSuccess = true;
                    response.Data = qpPriceModel;
                }

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public List<CustomerDdlModel> GetCustomerForSearching(string searchText, string ignoreIds, string pageSize,
                                                              bool isManualAllowed = true)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                string searchKey = "";
                foreach (var id in ignoreIds.Split(','))
                {
                    long n;
                    bool isNumeric = long.TryParse(id, out n);
                    if (isNumeric)
                        searchKey = searchKey + id + ",";
                }

                searchKey = searchKey.Trim(',');

                // string customeWhere = "TagID not in (" + searchKey + ")";
                List<CustomerDdlModel> masterCustomerlist =
                    GetEntityList<CustomerDdlModel>("GetCustomerDdlModel",
                                                    new List<SearchValueData>
                                                        {
                                                            new SearchValueData
                                                                {
                                                                    Name = "CustomerKeyWord",
                                                                    Value = searchText
                                                                }
                                                        })
                        .ToList()
                        .Take(Convert.ToInt16(pageSize))
                        .ToList();
                List<CustomerDdlModel> customerlist =
                    masterCustomerlist.Where(c => !searchKey.Contains(c.CustomerID.ToString())).ToList();

                foreach (var tag in customerlist)
                    tag.StrCustomerID = Convert.ToString(tag.CustomerID);

                //if (isManualAllowed && ignoreIds.Split(',').All(c => c != searchText))
                //{

                //    if (masterProductlist.Any(c => c.ProductName.ToLower() == searchText) == false)
                //    {
                //        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                //        productlist.Insert(0, new ProductDdlModel { StrProductID = myTI.ToTitleCase(searchText), ProductName = myTI.ToTitleCase(searchText) });
                //    }
                //}
                response.IsSuccess = true;
                response.Data = customerlist;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }



            return (List<CustomerDdlModel>)response.Data;
        }

        public List<ProductSampleDdlModel> GetProductSampleForSearching(string searchText, string ignoreIds,
                                                                        string pageSize, bool isManualAllowed = true)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                string searchKey = "";
                foreach (var id in ignoreIds.Split(','))
                {
                    long n;
                    bool isNumeric = long.TryParse(id, out n);
                    if (isNumeric)
                        searchKey = searchKey + id + ",";
                }

                searchKey = searchKey.Trim(',');

                // string customeWhere = "TagID not in (" + searchKey + ")";
                List<ProductSampleDdlModel> masterProductSamplelist =
                    GetEntityList<ProductSampleDdlModel>("GetProductSampleDdlModel",
                                                         new List<SearchValueData>
                                                             {
                                                                 new SearchValueData
                                                                     {
                                                                         Name = "ProductSampleKeyWord",
                                                                         Value = searchText
                                                                     }
                                                             })
                        .ToList()
                        .Take(Convert.ToInt16(pageSize))
                        .ToList();
                List<ProductSampleDdlModel> productSamplelist =
                    masterProductSamplelist.Where(c => !searchKey.Contains(c.ProductSampleID.ToString())).ToList();

                foreach (var prdSmpl in productSamplelist)
                    prdSmpl.StrProductSampleID = Convert.ToString(prdSmpl.ProductSampleID);

                //if (isManualAllowed && ignoreIds.Split(',').All(c => c != searchText))
                //{
                //    if (masterProductlist.Any(c => c.ProductName.ToLower() == searchText) == false)
                //    {
                //        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                //        productlist.Insert(0, new ProductSampleDdlModel { StrProductSampleID = myTI.ToTitleCase(searchText), ProductName = myTI.ToTitleCase(searchText) });
                //    }
                //}
                response.IsSuccess = true;
                response.Data = productSamplelist;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }



            return (List<ProductSampleDdlModel>)response.Data;
        }

        #endregion

        #region Raw Material

        public ServiceResponse AddRawMaterial(string encryptedId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                long rawMaterialId = Convert.ToInt32(Crypto.Decrypt(encryptedId));
                RawMaterial rawMaterial = GetEntity<RawMaterial>(new List<SearchValueData>
                    {
                        new SearchValueData {Name = "RawMaterialID", Value = rawMaterialId.ToString()}
                    }) ?? new RawMaterial();

                response.IsSuccess = true;
                response.Data = rawMaterial;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse ManageRawMaterial()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            RawMaterialSearchModel searchModel = new RawMaterialSearchModel();
            try
            {
                response.IsSuccess = true;
                response.Data = searchModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse SaveRawMaterial(RawMaterial model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            bool isEditMode = model.RawMaterialID > 0;

            try
            {
                string customWhere = isEditMode ? string.Format("RawMaterialID!={0}", model.RawMaterialID) : "";

                var searchListValueData = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "RawMaterialName", Value = model.RawMaterialName, IsEqual = true}
                    };
                RawMaterial existingRawMaterial = GetEntity<RawMaterial>(searchListValueData, customWhere);
                if (existingRawMaterial != null)
                {
                    response.Message =
                        Common.MessageWithTitle(
                            isEditMode ? Resource.UpdateRawMaterialFailed : Resource.AddRawMaterialFailed,
                            Resource.RawMaterialExistWithThisName);
                }
                else
                {
                    RawMaterial rawMaterial =
                        GetEntity<RawMaterial>(new List<SearchValueData>()
                            {
                                new SearchValueData()
                                    {Name = "RawMaterialID", Value = model.RawMaterialID.ToString(), IsEqual = true}
                            }) ?? new RawMaterial();

                    if (rawMaterial.RawMaterialID > 0 &&
                        (rawMaterial.RawMaterialName != model.RawMaterialName ||
                         Convert.ToDecimal(rawMaterial.RawMaterialPrice) != Convert.ToDecimal(model.RawMaterialPrice)))
                    {
                        RawMaterialHistory rawMaterialHistory = new RawMaterialHistory();
                        rawMaterialHistory.RawMaterialID = rawMaterial.RawMaterialID;
                        rawMaterialHistory.RawMaterialName = rawMaterial.RawMaterialName;
                        rawMaterialHistory.RawMaterialPrice = rawMaterial.RawMaterialPrice;

                        SaveObject(rawMaterialHistory, loggedInUserId);
                    }

                    rawMaterial.RawMaterialName = model.RawMaterialName;
                    rawMaterial.RawMaterialPrice = model.RawMaterialPrice;
                    rawMaterial.IsActive = isEditMode ? rawMaterial.IsActive : true;
                    SaveObject(rawMaterial, loggedInUserId);

                    response.IsSuccess = true;
                    response.Message = Common.MessageWithTitle(Resource.RawMaterialSaveSucceeded,
                                                               Resource.RawMaterialSavedSuccefully);
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse GetRawMaterialList(RawMaterialSearchModel searchParams, int pageSize, int pageIndex,
                                                  string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData
                            {
                                Name = "FromIndex",
                                Value = Convert.ToString(((pageIndex - 1)*pageSize) + 1)
                            },
                        new SearchValueData {Name = "ToIndex", Value = Convert.ToString(pageIndex*pageSize)},
                        new SearchValueData
                            {
                                Name = "SortExpression",
                                Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)
                            },
                        new SearchValueData
                            {
                                Name = "SortType",
                                Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)
                            },
                        new SearchValueData
                            {
                                Name = "RawMaterialName",
                                Value = Convert.ToString(searchParams.RawMaterialName)
                            },
                        new SearchValueData
                            {
                                Name = "RawMaterialPrice",
                                Value = Convert.ToString(searchParams.RawMaterialPrice)
                            }
                    };

                List<RawMaterialViewModel> totalData = GetEntityList<RawMaterialViewModel>("GetRawMaterialList",
                                                                                           searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<RawMaterialViewModel> rawMaterials = GetPageInStoredProcResultSet(pageIndex, pageSize, count,
                                                                                       totalData);
                response.Data = rawMaterials;
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse EnabledDisabledRawMaterial(long rawMaterialId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                RawMaterial data = GetEntity<RawMaterial>(rawMaterialId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);
                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.RawMaterialEnableSucceeded : Resource.RawMaterialDisableSucceeded,
                        data.IsActive
                            ? Resource.RawMaterialEnabledSuccessfully
                            : Resource.RawMaterialDisabledSuccessfully);

                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ExceptionMessage;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse GetRawMaterialListForGraph(string encryptedRawMaterialID, int pageSize, int pageIndex,
                                                  string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>()
                    {
                        new SearchValueData { Name = "FromIndex", Value = Convert.ToString(((pageIndex - 1)*pageSize) + 1) },
                        new SearchValueData {Name = "ToIndex", Value = Convert.ToString(pageIndex*pageSize)}, 
                        new SearchValueData { Name = "SortExpression", Value = string.IsNullOrEmpty(sortIndex) ? "LastUpdatedDate" : Convert.ToString(sortIndex) },
                        new SearchValueData { Name = "SortType", Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection) },
                        new SearchValueData { Name = "RawMaterialID", Value = Convert.ToInt32(Crypto.Decrypt(encryptedRawMaterialID)).ToString() }
                    };

                List<RawMaterialGraphViewModel> totalData = GetEntityList<RawMaterialGraphViewModel>("GetRawMaterialListForGraph", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<RawMaterialGraphViewModel> rawMaterials = GetPageInStoredProcResultSet(pageIndex, pageSize, count,
                                                                                       totalData);
                response.Data = rawMaterials;
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        #endregion Raw Material
    }
}
