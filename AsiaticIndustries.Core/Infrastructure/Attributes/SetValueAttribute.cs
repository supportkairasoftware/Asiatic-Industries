using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SetValueAttribute : Attribute
    {
        public SetValueAttribute(string sqlQuery, string[] propertyNames)
        {
            SQLQuery = sqlQuery;
            PropertyNames = propertyNames;
        }
        public string SQLQuery { get; set; }
        public string[] PropertyNames { get; set; }
    }
}
