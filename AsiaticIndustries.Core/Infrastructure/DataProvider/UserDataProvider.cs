using System;
using System.Collections.Generic;
using System.Linq;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;
using AsiaticIndustries.Core.Resources;
using PetaPoco;
using System.Text.RegularExpressions;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    public class UserDataProvider : BaseDataProvider, IUserDataProvider
    {
        #region User Region

        public ServiceResponse AddUser(string encryptedId, List<LU_Role> listRole, LU_Role loggedInRole)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                long userId = Convert.ToInt32(Crypto.Decrypt(encryptedId));
                User user = GetEntity<User>(new List<SearchValueData>
                {
                    new SearchValueData {Name = "UserID", Value = userId.ToString()}
                }) ?? new User();

                user.ListOfRole = listRole.Where(c => c.AccessLevel >= loggedInRole.AccessLevel).ToList();

                if (user.UserID > 0)
                {
                    LU_Role lastSlectedRole = listRole.First(c => c.RoleID == user.RoleID);
                    user.RoleName = lastSlectedRole.RoleName;
                }

                response.IsSuccess = true;
                response.Data = user;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse SaveUser(User model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            bool isEditMode = model.UserID > 0;

            try
            {
                string customWhere = isEditMode ? string.Format("UserID!={0}", model.UserID) : "";

                var searchListValueData = new List<SearchValueData> { new SearchValueData { Name = "Email", Value = model.Email, IsEqual = true } };
                User existingUser = GetEntity<User>(searchListValueData, customWhere);
                if (existingUser != null)
                {
                    response.Message = Common.MessageWithTitle(isEditMode ? Resource.UpdateUserFailed : Resource.AddUserFailed, Resource.UserExistWithThisEmail);
                }
                else
                {
                    User user = GetEntity<User>(new List<SearchValueData>() { new SearchValueData() { Name = "UserID", Value = model.UserID.ToString(), IsEqual = true } }) ?? new User();

                    if ((!string.IsNullOrEmpty(model.TempAddPassword) &&
                         model.TempAddPassword == model.ConfirmAddPassword) ||
                        !string.IsNullOrEmpty(model.TempEditPassword) &&
                        model.TempEditPassword == model.ConfirmEditPassword)
                    {
                        PasswordDetail passwordDetail =
                            Common.CreatePassword(isEditMode ? model.TempEditPassword : model.TempAddPassword);
                        user.Password = passwordDetail.Password;
                        user.PasswordSalt = passwordDetail.PasswordSalt;
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.RoleID = model.RoleID;
                    user.HostName = model.HostName;
                    user.IsActive = isEditMode ? user.IsActive : true;

                    SaveObject(user, loggedInUserId);

                    response.IsSuccess = true;
                    if (user.UserID == loggedInUserId)
                        response.Message = Common.MessageWithTitle(Resource.ProfileUpdatedTitle, Resource.ProfileUpdated);
                    else
                        response.Message = Common.MessageWithTitle(Resource.UserSaveSucceeded, Resource.UserSavedSuccefully);
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse ManageUser(List<LU_Role> listRole)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            UserSearchModel searchModel = new UserSearchModel();
            try
            {
                searchModel.ListOfRole = listRole;
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

        public ServiceResponse GetUserList(UserSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>() {
                    new SearchValueData { Name = "FromIndex", Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1) },
                    new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) },
                    new SearchValueData{ Name = "SortExpression",Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)},
                    new SearchValueData{ Name = "SortType",Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)},
                    new SearchValueData{ Name = "Email",Value =  Convert.ToString(searchParams.Email)},
                    new SearchValueData{ Name = "Name",Value =  Convert.ToString(searchParams.Name)},
                    new SearchValueData{ Name = "RoleID",Value =  Convert.ToString(searchParams.RoleID)}
                };

                List<UserViewModel> totalData = GetEntityList<UserViewModel>("GetUserList", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<UserViewModel> users = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = users;
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        public ServiceResponse EnabledDisabledUser(long userId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                User data = GetEntity<User>(userId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);

                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.UserEnableSucceeded : Resource.UserDisableSucceeded,
                        data.IsActive ? Resource.UserEnabledSuccessfully : Resource.UserDisabledSuccessfully);

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

        #endregion User Region

        #region Customer Region

        public ServiceResponse AddCustomer(string encryptedId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };

            try
            {
                long customerId = Convert.ToInt32(Crypto.Decrypt(encryptedId));
                Customer customer = GetEntity<Customer>(new List<SearchValueData>
                {
                    new SearchValueData {Name = "CustomerID", Value = customerId.ToString()}
                }) ?? new Customer();

                response.IsSuccess = true;
                response.Data = customer;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse ManageCustomer()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            CustomerSearchModel searchModel = new CustomerSearchModel();
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

        public ServiceResponse SaveCustomer(Customer model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            bool isEditMode = model.CustomerID > 0;

            try
            {
                string customWhere = isEditMode ? string.Format("CustomerID!={0}", model.CustomerID) : "";

                var searchListValueData = new List<SearchValueData> { new SearchValueData { Name = "Email", Value = model.Email, IsEqual = true } };
                Customer existingCustomer = GetEntity<Customer>(searchListValueData, customWhere);
                if (existingCustomer != null)
                {
                    response.Message = Common.MessageWithTitle(isEditMode ? Resource.UpdateCustomerFailed : Resource.AddCustomerFailed, Resource.UserExistWithThisEmail);
                }
                else
                {
                    Customer customer =
                        GetEntity<Customer>(new List<SearchValueData>()
                            {
                                new SearchValueData()
                                    { Name = "CustomerID", Value = model.CustomerID.ToString(), IsEqual = true }
                            }) ?? new Customer();

                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.Email = model.Email;
                    customer.Phone = model.Phone;
                    customer.Address = model.Address;
                    customer.StandardNumber = model.StandardNumber;
                    customer.IsActive = !isEditMode || customer.IsActive;

                    SaveObject(customer, loggedInUserId);

                    response.IsSuccess = true;
                    response.Message = Common.MessageWithTitle(Resource.CustomerSaveSucceeded, Resource.CustomerSavedSuccefully);
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse GetCustomerList(CustomerSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>() {
                    new SearchValueData { Name = "FromIndex", Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1) },
                    new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) },
                    new SearchValueData{ Name = "SortExpression",Value = string.IsNullOrEmpty(sortIndex) ? "CreatedDate" : Convert.ToString(sortIndex)},
                    new SearchValueData{ Name = "SortType",Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)},
                    new SearchValueData{ Name = "Email",Value =  Convert.ToString(searchParams.Email)},
                    new SearchValueData{ Name = "Name",Value =  Convert.ToString(searchParams.Name)}
                };

                List<CustomerViewModel> totalData = GetEntityList<CustomerViewModel>("GetCustomerList", searchList);
                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<CustomerViewModel> customers = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = customers;
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

        public ServiceResponse EnabledDisabledCustomer(long customerId, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Customer data = GetEntity<Customer>(customerId);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    SaveObject(data, loggedInUserId);
                    response.Message = Common.MessageWithTitle(
                        data.IsActive ? Resource.CustomerEnableSucceeded : Resource.CustomerDisableSucceeded,
                        data.IsActive ? Resource.CustomerEnabledSuccessfully : Resource.CustomerDisabledSuccessfully);
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

        #endregion Customer Region

        #region USD Master

        public ServiceResponse SaveUSDMaster(CurrentUSDRateHistory model, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            try
            {
                if (model.CurrentUSDRateID > 0)
                {
                    CurrentUSDRateHistory currentUSDRate =
                        GetEntity<CurrentUSDRateHistory>(new List<SearchValueData>()
                            {
                                new SearchValueData()
                                    {
                                        Name = "IsActive",
                                        Value = "1"
                                    }
                            });

                    if (Convert.ToDecimal(currentUSDRate.CurrentUSDRate) == Convert.ToDecimal(model.CurrentUSDRate))
                    {
                        //response.IsSuccess = false;
                        //response.Message = Common.MessageWithTitle(Resource.USDRateSavedFailed,
                        //                                       Resource.EnterDifferentUSDRate);

                        response.IsSuccess = true;
                        response.Message = Common.MessageWithTitle(Resource.USDRateSaveSucceeded,
                                                                   Resource.USDRateSavedSuccefully);
                        return response;
                    }
                }

                GetScalar("SaveUSDRates", new List<SearchValueData>
                {
                    new SearchValueData {Name = "CurrentUSDRate", Value = model.CurrentUSDRate},
                    new SearchValueData {Name = "ChangedBY", Value = loggedInUserId.ToString()}
                });

                response.IsSuccess = true;
                response.Message = Common.MessageWithTitle(Resource.USDRateSaveSucceeded,
                                                           Resource.USDRateSavedSuccefully);

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResponse ManageUSDMaster()
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            CurrentUSDRateHistory usdMasterModel = GetEntity<CurrentUSDRateHistory>(new List<SearchValueData>
                {
                    new SearchValueData {Name = "IsActive", Value = "1"}
                }) ?? new CurrentUSDRateHistory();
            try
            {
                response.IsSuccess = true;
                response.Data = usdMasterModel;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.ErrorCode = Constants.ErrorCode_InternalError;
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse GetUSDMasterList(CurrentUSDRateSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>() {
                    new SearchValueData { Name = "FromIndex", Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1) },
                    new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) },
                    new SearchValueData{ Name = "SortExpression",Value = string.IsNullOrEmpty(sortIndex) ? "ChangedDate" : Convert.ToString(sortIndex)},
                    new SearchValueData{ Name = "SortType",Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)},
                    new SearchValueData{ Name = "CurrentUSDRate",Value =  Convert.ToString(searchParams.CurrentUSDRate)},
                    new SearchValueData{ Name = "LastUpdatedName",Value =  Convert.ToString(searchParams.LastUpdatedName)}
                };

                List<CurrentUsdRateViewModel> totalData = GetEntityList<CurrentUsdRateViewModel>("GetUSDMasterList", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<CurrentUsdRateViewModel> usdRateList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = usdRateList;
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

        public ServiceResponse GetGraphUSDMasterList(int pageSize, int pageIndex, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>() {
                    new SearchValueData { Name = "FromIndex", Value = Convert.ToString(((pageIndex - 1) * pageSize) + 1) },
                    new SearchValueData { Name = "ToIndex", Value = Convert.ToString(pageIndex * pageSize) },
                    new SearchValueData{ Name = "SortExpression",Value = string.IsNullOrEmpty(sortIndex) ? "ChangedDate" : Convert.ToString(sortIndex)},
                    new SearchValueData{ Name = "SortType",Value = string.IsNullOrEmpty(sortDirection) ? "DESC" : Convert.ToString(sortDirection)},
                    new SearchValueData{ Name = "CurrentUSDRate",Value = ""},
                    new SearchValueData{ Name = "LastUpdatedName",Value = ""}
                };

                List<CurrentUsdRateViewModel> totalData = GetEntityList<CurrentUsdRateViewModel>("GetUSDMasterList", searchList);

                int count = 0;
                if (totalData != null && totalData.Count > 0)
                    count = totalData.First().Count;

                Page<CurrentUsdRateViewModel> usdRateList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
                response.Data = usdRateList;
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

        #endregion USD Master
    }
}
