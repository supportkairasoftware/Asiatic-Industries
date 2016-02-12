using System;
using System.Collections.Generic;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Models.Entity;

namespace AsiaticIndustries.Core.Models.ViewModel
{

    #region User

    public class UserViewModel
    {
        public long UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedUserID
        {
            get { return Crypto.Encrypt(Convert.ToString(UserID)); }
        }
    }

    public class UserSearchModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public List<LU_Role> ListOfRole { get; set; }
    }

    #endregion User

    #region Customer

    public class CustomerViewModel
    {
        public long CustomerID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public string StandardNumber { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedCustomerID
        {
            get { return Crypto.Encrypt(Convert.ToString(CustomerID)); }
        }

    }

    public class CustomerSearchModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    #endregion Customer

    #region USD Master

    public class CurrentUsdRateViewModel
    {
        public long CurrentUSDRateID { get; set; }

        private string _currentUSDRate;
        public string CurrentUSDRate
        {
            get { return _currentUSDRate; }
            set { _currentUSDRate = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        public string ChangedDate { get; set; }
        public string ChangedBY { get; set; }
        public string LastUpdatedName { get; set; }

        public bool IsActive { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

    public class CurrentUSDRateSearchModel
    {
        public string CurrentUSDRate { get; set; }
        public string LastUpdatedName { get; set; }
    }

    #endregion USD Master

}
