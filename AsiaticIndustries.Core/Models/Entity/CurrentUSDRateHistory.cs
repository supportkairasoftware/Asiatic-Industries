using System;
using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using AsiaticIndustries.Core.Resources;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("CurrentUSDRateHistories")]
    [PrimaryKey("CurrentUSDRateID")]
    [Sort("CurrentUSDRateID", "ASC")]
    public class CurrentUSDRateHistory
    {
        public long CurrentUSDRateID { get; set; }

        private string _currentUSDRate;
        [Display(Name = @"Current USD Rate")]
        [Required(ErrorMessageResourceName = "CurrentUSDRateRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(13, ErrorMessageResourceName = "CurrentUSDRateMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPrice, ErrorMessageResourceName = "CurrentUSDRateInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string CurrentUSDRate {
            get { return _currentUSDRate; }
            set { _currentUSDRate = Math.Round(Convert.ToDecimal(value), 2).ToString(); }
        }

        [SetValueOnAdd((int)Common.SetValue.CurrentTime)]
        public string ChangedDate { get; set; }

        [SetValueOnAdd((int)Common.SetValue.LoggedInUserId)]
        public long ChangedBY { get; set; }
        public bool IsActive { get; set; }

        [Ignore]
        public bool IsEditMode { get; set; }
    }
}
