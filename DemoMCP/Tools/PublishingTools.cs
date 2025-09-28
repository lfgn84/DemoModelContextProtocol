using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace DemoMCP;

[McpServerToolType]
public sealed class PublishingTools
{
    private readonly IPublishService _publishService;

    public PublishingTools(IPublishService publishService)
    {
        _publishService = publishService;
    }

    [McpServerTool]
    [DisplayName("Publish Insight to n8n")]
    [Description("Sends a product insight to the n8n webhook to be published (e.g., to Google Docs). Returns any link produced by the workflow.")]
    public Task<PublishResponse> PublishInsightToN8n(
        [Description("Document title to publish")] string title,
        [Description("Insight content (markdown or HTML)")] string content,
        [Description("Optional metadata (JSON serialized as string)")] string? metaJson = null)
    {
        return _publishService.PublishInsightAsync(title, content, metaJson);
    }
}
