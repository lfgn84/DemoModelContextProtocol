namespace DemoMCP;

public sealed class N8nOptions
{
    public string? WebhookUrl { get; set; }
}

public sealed class PublishRequest
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = ""; // markdown o html
    public Dictionary<string, object>? Meta { get; set; }
}

public sealed class PublishResponse
{
    public bool Ok { get; set; }
    public string? DocUrl { get; set; }      // si tu workflow responde con un link
    public string? Message { get; set; }
}
