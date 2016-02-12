using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.ViewModel
{
    public class QuoteAddModel
    {
        public QuoteAddModel()
        {
            Quote = new Quote();
            PrepopulateCustomers = new List<CustomerDdlModel>();
            PrepopulateProductSamples = new List<ProductSampleDdlModel>();
            QuoteDetail = new QuoteDetail();
        }

        public Quote Quote { get; set; }

        public List<CustomerDdlModel> PrepopulateCustomers { get; set; }
        [Ignore]
        public string StrPrepopulateCustomers { get; set; }

        public List<ProductSampleDdlModel> PrepopulateProductSamples { get; set; }
        [Ignore]
        public string StrPrepopulateProductSamples { get; set; }

        [Ignore]
        public List<string> OldProductSampleIds { get; set; }
        [Ignore]
        public List<string> NewProductSampleIds { get; set; }

        
        [Ignore]
        [Display(Name=@"Sample")]
        [Required(ErrorMessageResourceName = "ProductSampleRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(Constants.RegxAlphaNumericExcludingZero, ErrorMessageResourceName = "RawMaterialRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempProductSampleId { get; set; }

        [Ignore]
        public string CurrentUSDRate { get; set; }

        [Ignore]
        public string NewUSDRate { get; set; }


          [Ignore]
          public QuoteDetail QuoteDetail { get; set; }
    }


    public class QuoteSearchModel
    {
        public QuoteSearchModel()
        {
            ProductIDs=new List<string>();
            ProductSampleIDs = new List<string>();
        }

        public string CustomerName   { get; set; }
        public List<string> ProductSampleIDs { get; set; }
        public List<string> ProductIDs { get; set; }


        public bool IsAdmin { get; set; }
        public long CreatedBy { get; set; }

    }



    public class QuoteViewModel
    {

        public long QuoteDetailID { get; set; }
        //public string TotalCrudePerkg { get; set; }
        //public string TotalMFGOH { get; set; }
        //public string TotalADMOH { get; set; }
        public string TotalPackingCost { get; set; }
        public string TotalFreightCost { get; set; }
        public string TotalForwardingCost { get; set; }
        public string TotalInterest { get; set; }
        //public string TotalProfit { get; set; }
        public string FinalPrice { get; set; }
        public string USDRate { get; set; }
        public string TotalNettInUSD { get; set; }
        public long QuoteID { get; set; }
        public string TotalPrice { get; set; }

        [Ignore]
        public string EncryptedQuoteDetailID
        {
            get { return Crypto.Encrypt(Convert.ToString(QuoteDetailID)); }
        }

      
        public bool IsActive { get; set; }

        public string CustomerName { get; set; }
        public string ProductSamples { get; set; }
        public string Products { get; set; }

        [Ignore]
        public string EncryptedQuoteID
        {
            get { return Crypto.Encrypt(Convert.ToString(QuoteID)); }
        }
        public int Count { get; set; }
        public int Row { get; set; }

    }




    public class ProductSampleDdlModel
    {
        public long ProductSampleID { get; set; }
        public string ReferenceNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductSampleSearchText { get; set; }
        public String StrProductSampleID { get; set; }
        public long QuoteID { get; set; }
    }


    public class CustomerDdlModel
    {
        public long CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CustomerSearchText { get; set; }
        public string Address { get; set; }
        public String StrCustomerID { get; set; }
    }



    public class QuoteProductModel
    {
        public QuoteProductModel()
        {
            ProductSample = new ProductSample();
            Product = new Product();
            ListOfProductRawMaterial = new List<ProductRawMaterial>();
        }

        public ProductSample ProductSample { get; set; }
        public Product Product { get; set; }
        public List<ProductRawMaterial> ListOfProductRawMaterial { get; set; }

        public string CurrentUSDPrice { get; set; }
    }


    public class ProductRawMaterial
    {
        public long ProductItemID { get; set; }
        public long RawMaterialID { get; set; }
        public string NumberOfKgs { get; set; }
        public string RawMaterialName { get; set; }
        public string RawMaterialPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }




    public class QuoteProductPriceSearchModel
    {
        public long ProductSampleID { get; set; }
        public long QuoteID { get; set; }
        public bool IsPriceWillRevised { get; set; }
    }

    public class QuoteProductPriceModel
    {

        public long ProductID { get; set; }
        public string ProductName { get; set; }


        public long ProductSampleID { get; set; }
        public string ReferenceNumber { get; set; }
        public string ProductConstraints { get; set; }
        public string Description { get; set; }
        public string TotalRawMaterialsCost { get; set; }

        public string CrudePrice { get; set; }
        public string ProfitPerKg { get; set; }
        public string MfgOverHeadPrice { get; set; }
        public string AdminOverHeadPrice { get; set; }
       

        public string CrudePerKg { get; set; }
        public string MFGOH { get; set; }
        public string ADMOH { get; set; }
        public string Profit { get; set; }

        public string TotalProductCost { get; set; }

        public string USDRate { get; set; }
    }


}
