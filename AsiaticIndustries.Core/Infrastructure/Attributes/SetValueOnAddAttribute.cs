using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SetValueOnAddAttribute : Attribute
    {
        public SetValueOnAddAttribute(int setValue)
        {
            SetValue = setValue;
        }
        public int SetValue { get; set; }
    }
}
