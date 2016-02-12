using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AsiaticIndustries.Core.Infrastructure
{
    public class Constants
    {
        public const string NotFoundUrl = "/security/notfound";
        public const string InternalServerUrl = "/security/internalerror";
        public const string AccessDeniedUrl = "/security/accessdenied";
        public const string LogoutUrl = "/security/logout";
        public const string LoginUrl = "/security";
        public const string AccountVerificationUrl = "/security/accountverification";

        public const string AnonymousLoginPermission = "AnonymousLoginPermission";
        public const string AnonymousPermission = "Anonymous";
        public const string AuthorizedPermission = "Authorized";

        public const string AllInternalUsers = "Admin,Export Manager,Lab Assistant,Raw Material Manager,Quote/Marketing Manager";





        public const string AllInternalUsersExceptLA = "Admin,Export Manager";
        public const string AllInternalUsersExceptMR = "Admin,Lab Assistant";



        public const string OnlyAdmin = "Admin";
        public const string OnlyMarketingRep = "Export Manager";
        public const string OnlyLabAssistant = "Lab Assistant";
        public const string OnlyRawManager = "Raw Material Manager";
        public const string OnlyQuoteManager = "Quote/Marketing Manager";
        public const string Only_Admin_RawManager = "Admin,Raw Material Manager";
        public const string Only_Admin_MarketingRep = "Admin,Export Manager";
        public const string Only_Admin_MarketingRep_LabAssistant = "Admin,Export Manager,Lab Assistant";
        public const string Only_Admin_QuoteManager = "Admin,Quote/Marketing Manager";
        public const string Only_Admin_MarketingRep_QuoteManager = "Admin,Export Manager,Quote/Marketing Manager";

        public const string Only_Admin_MarketingRep_LabAssistant_QuoteManager = "Admin,Export Manager,Lab Assistant,Quote/Marketing Manager";


        public const long Role_Admin = 1;
        public const long Role_MarketingRep = 2;
        public const long Role_LA = 3;
        public const long Role_RawManager = 4;
        public const long Role_QuoteManager = 5;
        


        public const string NotAuthorized = "NotAuthorized";
        public const string EncryptedQueryString = "EncryptedQueryString";

        public const string RegxEmail = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
        public const string RegexName = @"[a-zA-Z]+[ a-zA-Z-_]*";
        public const string RegexPhone = @"^[0-9]{10,15}$";
        public const string RegexCommaSeparatedNumber = @"[0-9.,%+-]+[ 0-9.,%+-]*";
        public const string RegxPrice = @"\d+(\.\d{1,2})?";
        public const string RegxPercent = @"(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$";
        public const string RegxTwoDecimalPoint =  @"\d+(\.\d{1,2})?";
        public const string RegxThreeDecimalPoint = @"\d+(\.\d{1,2,3})?";
        public const string RegxPassword = @"^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+}{"":;\'?\/>.<,]).{8,20}$";
        public const string RegxAlphaNumericExcludingZero = @"^[a-zA-Z1-9_]*$";
        public const string RegxNumericExcludingZero = @"^[1-9_]*$";
        public const string RegxPositiveNumber = @"^0*[1-9]\d*$";

        public const string Regx3_2DecimalPoint = @"^[0-9]{1,3}(?:\.[0-9]{1,2})?$";//   @"\d+(\.\d{1,2})?";


        
        public const int AllRecordsConstant = -1;
        public const string QueryID = "ID";
        public const string IsDeleted = "0";

        public const string RecordSavedSuccessfully = "Record saved successfully";
        public const string RecordAlreadyExists = "Sorry, record with same {0} already exists";

        public const string DbDateFormat = "yyyy-MM-dd";
        public const string DisplayDateFormat = "dd/MM/yyyy";
        public const string MinimumDateForDb = "1900-01-01";
        public const string MaximumDateForDb = "9999-12-31";
        public const string RecordCombinedAlreadyExists = "Sorry, record with same {0} combined already exists";

        public static string ErrorCode_AccessDenied = "403";
        public static string ErrorCode_InternalError = "500";
        public static string ErrorCode_NotFound = "404";


        public const string NgReturnUrl_Home = "#/home";
        public const string NgReturnUrl_Dashboard = "#/dashboard";
        public const string NgReturnUrl_MyAccount = "#/myaccount";
        public const string NgReturnUrl_AddUser = "#/user";
        public const string NgReturnUrl_ListUser = "#/listuser";
        public const string NgReturnUrl_AddCustomer = "#/addcustomer";
        public const string NgReturnUrl_ListCustomer = "#/listcustomer";
        public const string NgReturnUrl_AddRawMaterial = "#/addrawmaterial";
        public const string NgReturnUrl_ListRawMaterial = "#/listrawmaterial";
        public const string NgReturnUrl_AddProduct = "#/addproduct";
        public const string NgReturnUrl_ListProduct = "#/listproduct";
        public const string NgReturnUrl_AddQuote = "#/addquote";
        public const string NgReturnUrl_ListQuote = "#/listquote";
        public const string NgReturnUrl_AddSample = "#/addsample";
        public const string NgReturnUrl_ListSample = "#/listsample";
        public const string NgReturnUrl_UsdMaster = "#/usdmaster";



        
        public const int NotificationStartAfterDay = 15;
        public const int BlockedUserAfterDay = 22;


        public static string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }
    
    }
}
