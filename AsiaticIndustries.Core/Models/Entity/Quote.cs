using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("Quotes")]
    [PrimaryKey("QuoteID")]
    [Sort("QuoteID", "ASC")]
    public class Quote
    {

        public long QuoteID { get; set; }


        [Display(Name = @"Customer")]
        [Required(ErrorMessageResourceName = "CustomerRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxAlphaNumericExcludingZero, ErrorMessageResourceName = "CustomerRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CustomerID { get; set; }


        private string _packingCost;
        [Display(Name = @"Packing Cost")]
        [Required(ErrorMessageResourceName = "PackingCostRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "PackingCostMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "PackingCostInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string PackingCost
        {
            get { return _packingCost; }
            set { _packingCost = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }



        private string _totalFreightCost;
        //[Display(Name = @"Total Freight Cost")]
        [Display(Name = @"Freight Cost")]
        [Required(ErrorMessageResourceName = "TotalFreightCostRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "TotalFreightCostMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "TotalFreightCostInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TotalFreightCost
        {
            get { return _totalFreightCost; }
            set { _totalFreightCost = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        private string _standardFreightCost;
        //[Display(Name = @"Standard Freight Cost")]
        [Display(Name = @"Freight Quantity")]
        [Required(ErrorMessageResourceName = "StandardFreightCostRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "StandardFreightCostMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "StandardFreightCostInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string StandardFreightCost
        {
            get { return _standardFreightCost; }
            set { _standardFreightCost = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        private string _totalForwardingCost;
        //[Display(Name = @"Total Forwarding Cost")]
        [Display(Name = @"Forwarding Cost")]
        [Required(ErrorMessageResourceName = "TotalForwardingCostRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "TotalForwardingCostMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "TotalForwardingCostInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TotalForwardingCost
        {
            get { return _totalForwardingCost; }
            set { _totalForwardingCost = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        private string _standardForwardingCost;
        //[Display(Name = @"Standard Forwarding Cost")]
        [Display(Name = @"Forwarding Quantity")]
        [Required(ErrorMessageResourceName = "StandardForwardingCostRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "StandardForwardingCostMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "StandardForwardingCostInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string StandardForwardingCost
        {
            get { return _standardForwardingCost; }
            set { _standardForwardingCost = Math.Round(Convert.ToDecimal(value), 2).ToString();   }
        }


        [Display(Name = @"Total Interest Month")]
        [Required(ErrorMessageResourceName = "TotalInterestMonthRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(4, ErrorMessageResourceName = "TotalInterestMonthMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPositiveNumber, ErrorMessageResourceName = "TotalInterestMonthInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TotalInterestMonth { get; set; }

        [Display(Name = @"Interest Rate(%)/Month")]
        [Required(ErrorMessageResourceName = "InterestRateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "InterestRateMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPercent, ErrorMessageResourceName = "InterestRateInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string InterestRate { get; set; }


        [Display(Name = @"Commission(%)")]
        //[Required(ErrorMessageResourceName = "CommissionRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "CommissionMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPercent, ErrorMessageResourceName = "CommissionInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Commission { get; set; }




        [Display(Name = @"Discount(%)")]
        //[Required(ErrorMessageResourceName = "InterestRateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "DiscountPercentMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPercent, ErrorMessageResourceName = "DiscountPercentInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string DiscountPercent { get; set; }

        [Display(Name = @"Discount Reason")]
        public string DiscountReason { get; set; }

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
        public string EncryptedQuoteID
        {
            get { return Crypto.Encrypt(Convert.ToString(QuoteID)); }
        }

        [Ignore]
        public bool IsPriceWillRevised { get; set; }

        public string USDRate { get; set; }
    }
}
