using System;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("Customers")]
    [PrimaryKey("CustomerID")]
    [Sort("CustomerID", "ASC")]
    public class Customer
    {
        public long CustomerID { get; set; }

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

        [StringLength(200, ErrorMessageResourceName = "PhoneMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [StringLength(200, ErrorMessageResourceName = "AddressMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        public bool IsActive { get; set; }


        [Display(Name = @"Standard Number")]
        [StringLength(100, ErrorMessageResourceName = "StandardNumberMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string StandardNumber { get; set; }


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

        [Ignore]
        public string EncryptedCustomerID
        {
            get { return Crypto.Encrypt(Convert.ToString(CustomerID)); }
        }

    }
}
