using System.Net;

namespace ADVA_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string>? ErrorMessages { get; set; } = null;
        public bool IsSuccess { get; set; }
        public Object Result { get; set; }
    }
}
