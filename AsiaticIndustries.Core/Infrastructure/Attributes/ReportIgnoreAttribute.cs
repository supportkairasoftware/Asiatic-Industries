using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ReportIgnoreAttribute : Attribute
    {
        public ReportIgnoreAttribute()
        {
        }
    }
}
