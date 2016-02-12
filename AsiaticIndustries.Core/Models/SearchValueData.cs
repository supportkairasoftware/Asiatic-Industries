namespace AsiaticIndustries.Core.Models
{
    public class SearchValueData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsEqual { get; set; }
        public bool IsNotEqual { get; set; }
    }

    public class SearchModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int OperatorId { get; set; }
    }
}
