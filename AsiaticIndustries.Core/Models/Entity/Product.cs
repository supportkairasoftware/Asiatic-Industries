using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("Products")]
    [PrimaryKey("ProductID")]
    [Sort("ProductID", "ASC")]
    public class Product
    {

        public long ProductID { get; set; }

        [Unique("ProductName")]
        [Display(Name = @"Product Name")]
        [Required(ErrorMessageResourceName = "ProductNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "ProductNameMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string ProductName { get; set; }



        private string _crudePrice;
        [Display(Name = @"Standard Quantity")]
        [Required(ErrorMessageResourceName = "CrudePriceRequired", ErrorMessageResourceType = typeof (Resource))]
        [StringLength(13, ErrorMessageResourceName = "CrudePriceMaxLength", ErrorMessageResourceType = typeof (Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "CrudePriceInvalid",ErrorMessageResourceType = typeof (Resource))]
        public string CrudePrice
        {
            get { return _crudePrice; }
            set { _crudePrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }


        private string _profitPerKg;
        [Display(Name = @"Profit/Kg")]
        [Required(ErrorMessageResourceName = "ProfitPerKgRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "ProfitPerKgMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "ProfitPerKgInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string ProfitPerKg
        {
            get { return _profitPerKg; }
            set { _profitPerKg = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        [Display(Name = @"Do you want to add third party product?")]
        public bool IsThirdPartyProduct { get; set; }


       

        [Ignore]
        [Display(Name = @"Crude Price/Kg")]
        [Required(ErrorMessageResourceName = "CrudePricePerKgRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "CrudePricePerKgMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "CrudePricePerKgInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string CrudePricePerKg { get; set; }


        [Ignore]
        [Display(Name = @"Profit/Kg")]
        [Required(ErrorMessageResourceName = "TempProfitPerKgRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "TempProfitPerKgMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "TempProfitPerKgInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TempProfitPerKg { get; set; }


        private string _mfgOverHeadPrice;
        [Display(Name = @"MFG Over Head Price (100%)")]
        [Required(ErrorMessageResourceName = "MfgOverHeadPriceRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "MfgOverHeadPriceMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "MfgOverHeadPriceInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string MfgOverHeadPrice
        {
            get { return _mfgOverHeadPrice; }
            set { _mfgOverHeadPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        private string _adminOverHeadPrice;
        [Display(Name = @"Admin Over Head Price (100%)")]
        [Required(ErrorMessageResourceName = "AdminOverHeadPriceRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "AdminOverHeadPriceMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "AdminOverHeadPriceInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string AdminOverHeadPrice
        {
            get { return _adminOverHeadPrice; }
            set { _adminOverHeadPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        public bool IsActive { get; set; }




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
        public string EncryptedProductID
        {
            get { return Crypto.Encrypt(Convert.ToString(ProductID)); }
        }

        [Ignore]
        public int Count { get; set; }

    }
}
