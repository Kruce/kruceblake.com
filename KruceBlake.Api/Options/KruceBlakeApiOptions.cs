namespace KruceBlake.Api.Options
{
    public record KruceBlakeApiOptions
    {
        public const string KruceBlakeApi = "KruceBlakeApi";
        public string HeaderName { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}