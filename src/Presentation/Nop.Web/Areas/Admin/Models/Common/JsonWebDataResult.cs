namespace Nop.Web.Areas.Admin.Models.Common
{
    public class JsonWebDataResult
    {
        public JsonWebDataResult()
        {
            Success = true;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
