using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.ViewModel
{
    public class ProductAddModel
    {
        public ProductAddModel()
        {
            Product = new Product();
            ListOfProductItem = new List<ProductItem>();
            ListOfRawMaterial = new List<RawMaterial>();
            OldProductItemIds = new List<string>();
            NewProductItemIds = new List<string>();

        }

        public Product Product { get; set; }
        
        public List<ProductItem> ListOfProductItem { get; set; }
        [Ignore]
        public List<string> OldProductItemIds { get; set; }
        [Ignore]
        public List<string> NewProductItemIds { get; set; }

        public List<RawMaterial> ListOfRawMaterial { get; set; }




        [Ignore]
        [Required(ErrorMessageResourceName = "RawMaterialRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPositiveNumber, ErrorMessageResourceName = "RawMaterialRequired", ErrorMessageResourceType = typeof(Resource))]
        public long TempRawMaterialId { get; set; }

        
        [Ignore]
        [Required(ErrorMessageResourceName = "NumberOfKgsRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxTwoDecimalPoint, ErrorMessageResourceName = "NumberOfKgsInvalid", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(8, ErrorMessageResourceName = "NumberOfKgsMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string TempNumberOfKgs { get; set; }

    }
    
    public class ProductSearchModel
    {
        public string ProductName { get; set; }
        public long RawMaterialID { get; set; }
        public List<RawMaterial> ListOfRawMaterial { get; set; }
    }
    
    public class ProductViewModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string CrudePrice { get; set; }
        public string ProfitPerKg{ get; set; }
        public bool IsThirdPartyProduct { get; set; }
        public string MfgOverHeadPrice { get; set; }
        public string AdminOverHeadPrice { get; set; }
        public bool IsActive { get; set; }
        [Ignore]
        public string EncryptedProductID
        {
            get { return Crypto.Encrypt(Convert.ToString(ProductID)); }
        }
        public int Count { get; set; }
        public int Row { get; set; }

    }


    public class ProductDdlModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public String StrProductID { get; set; }
    }


    
    #region Raw Material

    public class RawMaterialViewModel
    {
        public long RawMaterialID { get; set; }
        public string RawMaterialName { get; set; }

        private string _rawMaterialPrice;
        public string RawMaterialPrice
        {
            get { return _rawMaterialPrice; }
            set { _rawMaterialPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }
        public bool IsActive { get; set; }
        public bool IsEditMode { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedRawMaterialID
        {
            get { return Crypto.Encrypt(Convert.ToString(RawMaterialID)); }
        }
    }

    public class RawMaterialSearchModel
    {
        public string RawMaterialName { get; set; }
        public string RawMaterialPrice { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "RawMaterialNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "RawMaterialNameMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string TempRawMaterialName { get; set; }

        private string _rawMaterialPrice;
        [Ignore]
        [Required(ErrorMessageResourceName = "RawMaterialPriceRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "RawMaterialPriceMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "RawMaterialPriceInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string TempRawMaterialPrice
        {
            get { return _rawMaterialPrice; }
            set { _rawMaterialPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }
    }

    public class RawMaterialGraphViewModel
    {
        public long RawMaterialID { get; set; }
        public string RawMaterialName { get; set; }

        private string _rawMaterialPrice;
        public string RawMaterialPrice
        {
            get { return _rawMaterialPrice; }
            set { _rawMaterialPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public string LastUpdatedName { get; set; }

        public bool IsActive { get; set; }
        public bool IsEditMode { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedRawMaterialID
        {
            get { return Crypto.Encrypt(Convert.ToString(RawMaterialID)); }
        }
    }

    #endregion Raw Material
}
