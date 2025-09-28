using System.Threading.Tasks;

namespace DemoMCP;

public interface IPublishService
{
    Task<PublishResponse> PublishInsightAsync(string title, string content, string? metaJson = null);
}