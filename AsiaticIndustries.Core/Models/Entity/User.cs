using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("Users")]
    [PrimaryKey("UserID")]
    [Sort("UserID", "ASC")]
    public class User
    {
        public long UserID { get; set; }

        [Display(Name = @"First Name")]
        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "FirstNameMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Display(Name = @"Last Name")]
        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "LastNameMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceName = "EmailMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        [StringLength(200, ErrorMessageResourceName = "PhoneMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [StringLength(200, ErrorMessageResourceName = "AddressMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = @"Role")]
        [Required(ErrorMessageResourceName = "RoleRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RoleID { get; set; }

        [SetValueOnAdd((int)Common.SetValue.LoggedInUserId)]
        public long CreatedBy { get; set; }
        [SetValueOnAdd((int)Common.SetValue.CurrentTime)]
        public DateTime CreatedDate { get; set; }
        [SetValueOnAdd((int)Common.SetValue.LoggedInUserId)]
        [SetValueOnUpdate((int)Common.SetValue.LoggedInUserId)]
        public long LastUpdatedBy { get; set; }
        [SetValueOnAdd((int)Common.SetValue.CurrentTime)]
        [SetValueOnUpdate((int)Common.SetValue.CurrentTime)]
        public DateTime LastUpdatedDate { get; set; }

        public bool IsActive { get; set; }


        [Display(Name = @"Host Name")]
        [Required(ErrorMessageResourceName = "HostNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "HostNameMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string HostName { get; set; }

        [Ignore]
        [Display(Name = @"Password")]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        //[StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TempAddPassword { get; set; }

        [Ignore]
        [Display(Name = @"Confirm Password")]
        //[StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "ConfirmPasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [Compare("TempAddPassword", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmAddPassword { get; set; }

        [Ignore]
        [Display(Name = @"Password")]
        //[StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
       // [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TempEditPassword { get; set; }

        [Ignore]
        [Display(Name = @"Confirm Password")]
       // [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "ConfirmPasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [Compare("TempEditPassword", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmEditPassword { get; set; }

        [Ignore]
        public List<LU_Role> ListOfRole { get; set; }

        [Ignore]
        public string RoleName { get; set; }

        [Ignore]
        public string EncryptedUserID
        {
            get { return Crypto.Encrypt(Convert.ToString(UserID)); }
        }

    }
}
