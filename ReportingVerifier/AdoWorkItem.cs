using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace ReportingVerifier;

public record AdoWorkItem(int Id, string Title, string AreaPath)
{
    public const string TitleField = "System.Title";
    public const string AreaPathField = "System.AreaPath";

    public static AdoWorkItem? TryFrom(WorkItem workItem)
    {
        if (workItem.Id.HasValue is false)
            return null;

        if (workItem.Fields.ContainsKey(TitleField) is false)
            return null;

        if (workItem.Fields.ContainsKey(AreaPathField) is false)
            return null;

        int id = workItem.Id.Value;
        var title = workItem.Fields[TitleField].ToString()!;
        var areaPath = workItem.Fields[AreaPathField].ToString()!;
        return new AdoWorkItem(id, title, areaPath);
    }
}
