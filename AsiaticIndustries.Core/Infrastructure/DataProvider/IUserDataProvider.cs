using System.Collections.Generic;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    public interface IUserDataProvider
    {
        #region User Region

        ServiceResponse AddUser(string encryptedId, List<LU_Role> listRole, LU_Role loggedInRole);

        ServiceResponse ManageUser(List<LU_Role> listRole);

        ServiceResponse SaveUser(User model, long loggedInUserId);

        ServiceResponse GetUserList(UserSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);

        ServiceResponse EnabledDisabledUser(long userId, long loggedInUserId);

        #endregion User Region

        #region Customer Region

        ServiceResponse AddCustomer(string encryptedId);

        ServiceResponse ManageCustomer();

        ServiceResponse SaveCustomer(Customer model, long loggedInUserId);

        ServiceResponse GetCustomerList(CustomerSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);

        ServiceResponse EnabledDisabledCustomer(long customerId, long loggedInUserId);

        #endregion Customer Region

        #region USD Master

        ServiceResponse SaveUSDMaster(CurrentUSDRateHistory model, long loggedInUserId);

        ServiceResponse ManageUSDMaster();

        ServiceResponse GetUSDMasterList(CurrentUSDRateSearchModel searchParams, int pageSize, int pageIndex, string sortIndex, string sortDirection);

        ServiceResponse GetGraphUSDMasterList(int pageSize, int pageIndex, string sortIndex, string sortDirection);

        #endregion USD Master
    }
}
