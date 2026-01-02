using System.Net;

namespace KruceBlake.Web.Models
{
    public class ErrorViewModel
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
        public string OriginalPath { get; set; } = "unknown";
        public bool ShowReferenceId => !string.IsNullOrEmpty(ReferenceId);
    }
}