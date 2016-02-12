using System;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("RawMaterials")]
    [PrimaryKey("RawMaterialID")]
    [Sort("RawMaterialID", "ASC")]
    public class RawMaterial
    {
        public long RawMaterialID { get; set; }

        [Display(Name = @"Name")]
        [Required(ErrorMessageResourceName = "RawMaterialNameRequired", ErrorMessageResourceType = typeof (Resource))]
        [StringLength(100, ErrorMessageResourceName = "RawMaterialNameMaxLength",
            ErrorMessageResourceType = typeof (Resource))]
        public string RawMaterialName { get; set; }

        private string _rawMaterialPrice;
        [Display(Name = @"Price")]
        [Required(ErrorMessageResourceName = "RawMaterialPriceRequired", ErrorMessageResourceType = typeof (Resource))]
        [StringLength(13, ErrorMessageResourceName = "RawMaterialPriceMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "RawMaterialPriceInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string RawMaterialPrice
        {
            get { return _rawMaterialPrice; }
            set { _rawMaterialPrice = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
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

    }
}
