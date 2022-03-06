using Microsoft.Office.Interop.Outlook;

namespace ReportingVerifier;

public record OutlookCalendarEvent(string ProjectCode, int MiddleCode, DateTime Start)
{
    public static OutlookCalendarEvent? TryFrom(AppointmentItem item)
    {
        string[] subjectParts = item.Subject.Split('-');
        if (subjectParts.Length < 2)
            return null;

        string projectCode = subjectParts[0].Trim();

        bool middleCodeParseResult = int.TryParse(subjectParts[1].Trim(), out int middleCode);
        if (middleCodeParseResult is false || middleCode <= 0)
            return null;

        return new OutlookCalendarEvent(projectCode, middleCode, item.Start);
    }

    public bool HasCorrectProjectCode(string areaPath)
        => areaPath.ToProjectCode().Equals(ProjectCode, StringComparison.InvariantCultureIgnoreCase);
}
