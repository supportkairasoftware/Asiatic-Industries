using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("ProductItems")]
    [PrimaryKey("ProductItemID")]
    [Sort("ProductItemID", "ASC")]
    public class ProductItem
    {
        public long ProductItemID { get; set; }

        [Required(ErrorMessageResourceName = "RawMaterialRequired", ErrorMessageResourceType = typeof(Resource))]
        public long RawMaterialID { get; set; }

        [Required(ErrorMessageResourceName = "NumberOfKgsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NumberOfKgs { get; set; }

        [Ignore]
        public string TotalPrice { get; set; }
        
        public long ProductID { get; set; }
        
        [Ignore]
        public string StrProductItemID { get; set; }
    }
}
