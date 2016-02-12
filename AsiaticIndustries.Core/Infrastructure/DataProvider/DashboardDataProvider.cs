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
    public class DashboardDataProvider : BaseDataProvider, IDashboardDataProvider
    {
        #region User Region

        public ServiceResponse DashboardDetails(string roleName)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>() {
                    new SearchValueData { Name = "RoleName", Value =roleName }
                };

                DashboardModel totalData = GetMultipleEntity<DashboardModel>("GetDashBoardModel", searchList);

                response.Data = totalData;
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
    }
}
