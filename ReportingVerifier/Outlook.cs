using Microsoft.Office.Interop.Outlook;

namespace ReportingVerifier;

public class Outlook
{
    private readonly Application _app = new();

    public IEnumerable<OutlookCalendarEvent> GetCalendarItems(DateTime from, DateTime to)
    {
        var mapiNamespace = _app.GetNamespace("MAPI");
        var calendarFolder = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);
        var items = calendarFolder.Items;
        items.IncludeRecurrences = Settings.IncludeRecurrences;
        var appointmentItems = items.Cast<AppointmentItem>();

        var filteredItems = FilterItems(appointmentItems, from, to);

        var outlookCalendarEvents = filteredItems
            .Select(OutlookCalendarEvent.TryFrom)
            .Where(calendarEvent => calendarEvent is not null)
            .Cast<OutlookCalendarEvent>();

        return outlookCalendarEvents;
    }

    private static IEnumerable<AppointmentItem> FilterItems(IEnumerable<AppointmentItem> items,
        DateTime from, DateTime to)
    {
        var filteredItems = items
            .Where(item => !item.Subject.StartsWith(Settings.IgnoreCalendarEventsWithSubjectThatStartsWith))
            .Where(item => item.Start >= from && item.Start <= to);

        return filteredItems;
    }
}
