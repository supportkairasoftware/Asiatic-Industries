using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Models.Entity;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.ViewModel
{
    public class ProductSampleAddModel
    {
        public ProductSampleAddModel()
        {
            ProductSample = new ProductSample();
            PrepopulateProducts=new List<ProductDdlModel>();
        }

        public ProductSample ProductSample { get; set; }

        public List<ProductDdlModel> PrepopulateProducts { get; set; }
    }


    public class ProductSampleSearchModel
    {
        public ProductSampleSearchModel()
        {
            ProductIDs=new List<string>();
        }

        public string CustomerName   { get; set; }
        public string ReferenceNumber { get; set; }
        public List<string> ProductIDs { get; set; }

    }



    public class ProductSampleViewModel
    {
        public long ProductSampleID { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public string AINumber { get; set; }
        public string CheckedBy { get; set; }
        public string ProductName { get; set; }
        public string ProductConstraints { get; set; }
        public bool IsActive { get; set; }

        [Ignore]
        public string EncryptedProductSampleID
        {
            get { return Crypto.Encrypt(Convert.ToString(ProductSampleID)); }
        }
        public int Count { get; set; }
        public int Row { get; set; }

    }

}
