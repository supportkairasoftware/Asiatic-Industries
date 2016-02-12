using System.Collections.Generic;
using AsiaticIndustries.Core.Models.Entity;

namespace AsiaticIndustries.Core.Models
{
    class SessionValueData
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public List<LU_Role> Roles { get; set; }
        public LU_Role SelectedRole { get; set; }
    }
}
