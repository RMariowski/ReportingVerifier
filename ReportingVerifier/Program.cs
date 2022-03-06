namespace ReportingVerifier;

public static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var outlook = new Outlook();
            var azureDevOps = new AzureDevOps();

            var now = DateTime.Now;
            var startOfTheMonth = new DateTime(now.Year, now.Month, 1);
            var endOfTheMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            Console.WriteLine("Getting calendar events from Outlook...");
            var calendarEvents = outlook.GetCalendarItems(startOfTheMonth, endOfTheMonth).ToArray();
            Console.WriteLine("Finished getting calendar events from Outlook");

            var middleCodes = calendarEvents.Select(calendarEvent => calendarEvent.MiddleCode).ToHashSet();

            Console.WriteLine("Getting work items from Azure DevOps...");
            var workItems = await azureDevOps.GetWorkItems(middleCodes);
            Console.WriteLine("Finished getting work items from Azure DevOps");

            Console.WriteLine($"Checking {calendarEvents.Length} calendar events...");
            foreach (var calendarEvent in calendarEvents)
            {
                if (workItems.ContainsKey(calendarEvent.MiddleCode) is false)
                {
                    Console.WriteLine($"Work item not found for {calendarEvent}");
                    continue;
                }

                var workItem = workItems[calendarEvent.MiddleCode];
                if (!calendarEvent.HasCorrectProjectCode(workItem.AreaPath))
                {
                    Console.WriteLine(
                        $"Mismatch found {calendarEvent}. Project code should be {workItem.AreaPath.ToProjectCode()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Execution finished");
    }
}
