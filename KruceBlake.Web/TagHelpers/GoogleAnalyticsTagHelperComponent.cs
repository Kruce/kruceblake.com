using KruceBlake.Web.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace KruceBlake.Web.TagHelpers
{
    public class GoogleAnalyticsTagHelperComponent(IOptions<GoogleAnalyticsOptions> googleAnalyticsOptions) : TagHelperComponent
    {
        private readonly GoogleAnalyticsOptions _googleAnalyticsOptions = googleAnalyticsOptions.Value;
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Inject the code only in the head element
            if (string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                // Get the tracking code from the configuration
                var trackingCode = _googleAnalyticsOptions.TrackingCode;
                if (!string.IsNullOrEmpty(trackingCode))
                {
                    // PostContent correspond to the text just before closing tag
                    output.PostContent.AppendHtml(
                        $"<script async src='https://www.googletagmanager.com/gtag/js?id={trackingCode}'></script>" +
                        $"<script>window.dataLayer=window.dataLayer || [];" +
                        $"function gtag(){{dataLayer.push(arguments);}}" +
                        $"gtag('js', new Date());" +
                        $"gtag('config','{trackingCode}');</script>");
                }
            }
            return Task.CompletedTask;
        }
    }
}
