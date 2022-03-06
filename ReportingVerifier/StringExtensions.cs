namespace ReportingVerifier;

public static class StringExtensions
{
    public static string ToProjectCode(this string areaPath)
        => Settings.CodeMappings.ContainsKey(areaPath) ? Settings.CodeMappings[areaPath] : areaPath;
}
