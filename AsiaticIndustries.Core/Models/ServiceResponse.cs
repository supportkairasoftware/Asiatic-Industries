namespace AsiaticIndustries.Core.Models
{
    public class ServiceResponse//<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ErrorCode { get; set; }
    }
}
