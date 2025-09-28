using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoMCP;

public class N8nPublishService : IPublishService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _webhookUrl;
    public N8nPublishService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _webhookUrl = "" ?? throw new InvalidOperationException("N8N webhook URL not configured.");
    }

    public async Task<PublishResponse> PublishInsightAsync(string title, string content, string? metaJson = null)
    {
        var req = new PublishRequest
        {
            Title = title,
            Content = content,
            Meta = string.IsNullOrWhiteSpace(metaJson)
                ? null
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(metaJson)
        };

        using var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);

        using var httpRes = await httpClient.PostAsJsonAsync(_webhookUrl, req);
        var body = await httpRes.Content.ReadAsStringAsync();

        var resp = new PublishResponse
        {
            Ok = httpRes.IsSuccessStatusCode,
            Message = httpRes.ReasonPhrase
        };

        try
        {
            var json = System.Text.Json.JsonDocument.Parse(body).RootElement;
            if (json.TryGetProperty("docUrl", out var urlProp))
                resp.DocUrl = urlProp.GetString();
            if (json.TryGetProperty("message", out var msgProp))
                resp.Message = msgProp.GetString();
        }
        catch { }

        return resp;
    }
}