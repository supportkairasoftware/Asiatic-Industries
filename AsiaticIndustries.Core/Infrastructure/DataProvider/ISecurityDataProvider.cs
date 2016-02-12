using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.ViewModel;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    public interface ISecurityDataProvider
    {
        ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession = false);
    }
}
