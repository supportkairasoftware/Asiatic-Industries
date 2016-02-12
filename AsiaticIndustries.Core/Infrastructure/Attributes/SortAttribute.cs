﻿using System;

namespace AsiaticIndustries.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SortAttribute : Attribute
    {
        public SortAttribute(string sortKey, string sortDirection)
        {
            KeyValue = sortKey;
            DirectionValue = sortDirection;
        }

        public string KeyValue
        {
            get;
            private set;
        }

        public string DirectionValue
        {
            get;
            private set;
        }
    }
}
