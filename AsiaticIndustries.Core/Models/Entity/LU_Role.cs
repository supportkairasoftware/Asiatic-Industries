using AsiaticIndustries.Core.Infrastructure.Attributes;
using PetaPoco;

namespace AsiaticIndustries.Core.Models.Entity
{
    [TableName("LU_Roles")]
    [PrimaryKey("RoleID")]
    [Sort("RoleID", "DESC")]
    public class LU_Role
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public int AccessLevel { get; set; }
    }
}
