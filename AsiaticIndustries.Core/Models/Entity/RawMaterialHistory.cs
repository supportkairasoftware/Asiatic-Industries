using System;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Infrastructure.Attributes;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("RawMaterialHistories")]
    [PrimaryKey("RawMaterialHistoryID")]
    [Sort("RawMaterialHistoryID", "ASC")]
    public class RawMaterialHistory
    {
        public long RawMaterialHistoryID { get; set; }
        public long RawMaterialID { get; set; }
        public string RawMaterialName { get; set; }
        public string RawMaterialPrice { get; set; }

        [SetValueOnAdd((int)Common.SetValue.LoggedInUserId)]
        public long LastUpdatedBy { get; set; }
        [SetValueOnAdd((int)Common.SetValue.CurrentTime)]
        public DateTime LastUpdatedDate { get; set; }
    }
}
