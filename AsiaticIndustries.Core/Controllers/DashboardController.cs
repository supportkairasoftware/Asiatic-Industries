using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Infrastructure.DataProvider;
using AsiaticIndustries.Core.Models;
using System.Web.Mvc;

namespace AsiaticIndustries.Core.Controllers
{
    [CustomAuthorize(Permissions = Constants.AllInternalUsers, NgReturnUrl = Constants.NgReturnUrl_Dashboard)]
    public class DashboardController : BaseController
    {
        private IDashboardDataProvider _dashboardDataProvider;

        public ActionResult Index()
        {
            _dashboardDataProvider = new DashboardDataProvider();
            ServiceResponse response = _dashboardDataProvider.DashboardDetails(SessionHelper.SelectedRole.RoleName);
            return View(response.Data);

        }
    }
}
