namespace KruceBlake.Web.Models
{
    public class ErrorViewModel
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public string RequestId { get; set; } = "unknown";
        public string OriginalPath { get; set; } = "unknown";
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
