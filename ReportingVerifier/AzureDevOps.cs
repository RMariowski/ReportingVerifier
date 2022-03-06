using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace ReportingVerifier;

public class AzureDevOps
{
    private static readonly string[] FieldsToFetch = { AdoWorkItem.TitleField, AdoWorkItem.AreaPathField };

    private readonly WorkItemTrackingHttpClient _workItemTrackingClient;

    public AzureDevOps()
    {
        var credentials = new VssBasicCredential(string.Empty, Settings.AzureDevOpsAccessToken);
        var connection = new VssConnection(Settings.AzureDevOpsUri, credentials);
        _workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();
    }

    public async Task<IDictionary<int, AdoWorkItem>> GetWorkItems(HashSet<int> ids)
    {
        if (ids.Count == 0)
            return new Dictionary<int, AdoWorkItem>();

        var workItems = await _workItemTrackingClient.GetWorkItemsAsync(ids, FieldsToFetch);
        if (workItems is null)
            return new Dictionary<int, AdoWorkItem>();

        var adoWorkItems = workItems
            .Select(AdoWorkItem.TryFrom)
            .Where(workItem => workItem is not null)
            .Cast<AdoWorkItem>()
            .ToDictionary(workItem => workItem.Id, workItem => workItem);

        return adoWorkItems;
    }
}
