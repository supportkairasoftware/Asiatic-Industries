using System;
using System.Collections.Generic;
using System.Web;
using AsiaticIndustries.Core.Models.Entity;

namespace AsiaticIndustries.Core.Infrastructure
{
    public class SessionHelper
    {
        public static long UserID
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["UserID"]); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }

        public static string Name
        {
            get { return Convert.ToString(HttpContext.Current.Session["Name"]); }
            set { HttpContext.Current.Session["Name"] = value; }
        }

        public static string FirstName
        {
            get { return Convert.ToString(HttpContext.Current.Session["FirstName"]); }
            set { HttpContext.Current.Session["FirstName"] = value; }
        }

        public static bool IsTimeOutHappened
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["IsTimeOutHappened"]); }
            set { HttpContext.Current.Session["IsTimeOutHappened"] = value; }
        }

        public static List<LU_Role> Roles
        {
            get { return (List<LU_Role>)HttpContext.Current.Session["Roles"]; }
            set { HttpContext.Current.Session["Roles"] = value; }
        }
        public static LU_Role SelectedRole
        {
            get { return (LU_Role)HttpContext.Current.Session["SelectedRole"]; }
            set { HttpContext.Current.Session["SelectedRole"] = value; }
        }
    }
}
