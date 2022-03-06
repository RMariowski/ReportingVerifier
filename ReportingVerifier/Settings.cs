namespace ReportingVerifier;

public static class Settings
{
    public const char IgnoreCalendarEventsWithSubjectThatStartsWith = '@';
    public const bool IncludeRecurrences = false;

    public static readonly Uri AzureDevOpsUri = new("https://");
    public const string AzureDevOpsAccessToken = "";

    public static readonly Dictionary<string, string> CodeMappings = new()
    {
        // { AreaPath, MiddleCode }
    };
}
