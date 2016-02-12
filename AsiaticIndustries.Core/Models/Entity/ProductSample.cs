using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("ProductSamples")]
    [PrimaryKey("ProductSampleID")]
    [Sort("ProductSampleID", "ASC")]
    public class ProductSample
    {
        public long ProductSampleID { get; set; }

        //NOTE: We have chaanged AI to RK and vice-versa. For display only.

        [Display(Name = @"AI Number")]
        [Required(ErrorMessageResourceName = "ReferenceNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "ReferenceNumberMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string ReferenceNumber { get; set; }

        [Display(Name = @"RK Number")]
        //[Required(ErrorMessageResourceName = "AINumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "AINumberMaxLength", ErrorMessageResourceType = typeof(Resource))]
        public string AINumber { get; set; }


        [Display(Name = @"Concentration")]
        [Required(ErrorMessageResourceName = "ProductConstraintsRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceName = "ProductConstraintsMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.Regx3_2DecimalPoint, ErrorMessageResourceName = "ProductConstraintsInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string ProductConstraints { get; set; }

        [Display(Name = @"Product")]
        [Required(ErrorMessageResourceName = "ProductRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPositiveNumber, ErrorMessageResourceName = "ProductRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ProductID { get; set; }

        [Display(Name = @"Description")]
        //[Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Description { get; set; }

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
        public string EncryptedProductSampleID
        {
            get { return Crypto.Encrypt(Convert.ToString(ProductSampleID)); }
        }
        

    }
}
