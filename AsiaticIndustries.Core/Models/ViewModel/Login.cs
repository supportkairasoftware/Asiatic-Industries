using System.ComponentModel.DataAnnotations;
using AsiaticIndustries.Core.Infrastructure;
using AsiaticIndustries.Core.Resources;

namespace AsiaticIndustries.Core.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}
