using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DropDownAttribute : Attribute
    {
        public DropDownAttribute(string sqlQuery, string dropDownListProperty)
        {
            SQLQuery = sqlQuery;
            DropDownListProperty = dropDownListProperty;
        }
        public string SQLQuery { get; set; }
        public string DropDownListProperty { get; set; }
    }
}
