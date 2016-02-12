using System.Web.Mvc;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;

namespace AsiaticIndustries.Core.Controllers
{
    //[CustomAuthorize(Permissions = Constants.AllLoggedInUserPermission)]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
