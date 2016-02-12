using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SetUserNameAttribute : Attribute
    {
        public SetUserNameAttribute(string targetPropertyName)
        {
            TargetPropertyName = targetPropertyName;
        }
        //public string SQLQuery { get; set; }
        //public string DropDownListProperty { get; set; }
        public string TargetPropertyName { get; set; }
    }
}
