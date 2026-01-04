namespace KruceBlake.Web.Options
{
    public record GoogleAnalyticsOptions
    {
        public const string GoogleAnalytics = "GoogleAnalytics";
        public string TrackingCode { get; set; } = string.Empty;
    }
}