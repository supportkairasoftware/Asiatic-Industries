using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AsiaticIndustries.Core.Models;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Models.ViewModel;
using AsiaticIndustries.Core.Resources;

namespace AsiaticIndustries.Core.Infrastructure.DataProvider
{
    class SecurityDataProvider : BaseDataProvider, ISecurityDataProvider
    {
        // Code for check user login,session
        public ServiceResponse CheckLogin(LoginModel login, bool isRegenerateSession)
        {
            ServiceResponse response = new ServiceResponse { IsSuccess = false };
            String hostName = (Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName);
            try
            {
               
                if (login != null)
                {

                   // String hostName = Environment.MachineName;// Dns.GetHostName();
                   
                    

                    List<SearchValueData> searchListValueData = new List<SearchValueData>();
                    searchListValueData.Add(new SearchValueData { Name = "Email", Value = login.Email, IsEqual = true });
                    //searchListValueData.Add(new SearchValueData { Name = "HostName", Value = login.Email, IsEqual = true });

                    User user = GetEntity<User>(searchListValueData);

                    if (user != null && (Common.PasswordsMatch(login.Password, user.PasswordSalt, user.Password) ||
                                         isRegenerateSession) && hostName.ToLower() == user.HostName.ToLower())
                    {


                        DateTime startDate = new DateTime(2050, 12, 1);
                        if (startDate.AddDays(Constants.BlockedUserAfterDay).Date <= DateTime.Now.Date)
                        {
                            response.Message = Common.MessageWithTitle(Resource.LicenceExpiredTitle, Resource.LicenceExpiredMessage);
                            return response;
                        }


                        if (!user.IsActive)
                            response.Message = Common.MessageWithTitle(Resource.LoginFailed, Resource.InactiveAccount);
                        else
                        {
                            SessionValueData sessionValueData = new SessionValueData();
                            sessionValueData.UserID = user.UserID;
                            sessionValueData.Name = string.Format("{0} {1}", user.FirstName, user.LastName);
                            sessionValueData.FirstName = user.FirstName;
                            sessionValueData.Roles = GetEntityList<LU_Role>();
                            sessionValueData.SelectedRole = sessionValueData.Roles.Single(c => c.RoleID == user.RoleID);
                            response.Data = sessionValueData;
                            response.IsSuccess = true;
                            response.Message = Common.MessageWithTitle(Resource.LoginSuccess, Resource.LoginSuccessMessage);
                        }
                    }
                    else
                    {
                        response.Message = Common.MessageWithTitle(string.Format("{0} for {1}",Resource.LoginFailed,hostName), Resource.UsernamePasswordIncorrect);
                    }
                }
                else
                    response.Message = Common.MessageWithTitle(string.Format("{0} for {1}", Resource.LoginFailed, hostName), Resource.ExceptionMessage);
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(string.Format("{0} for {1}", Resource.LoginFailed, hostName), Resource.ExceptionMessage);
            }

            return response;
        }

        // Get user as per his email.
        public User GetUserByUserName(string userName)
        {
            string customWhere = "Email = '" + userName + "'";
            return GetEntity<User>(null, customWhere);
        }

    }
}
