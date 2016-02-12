using System.Collections.Generic;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    public interface IDashboardDataProvider
    {
        #region Dashboard

        ServiceResponse DashboardDetails(string roleName);

        #endregion Dashboard
    }
}
