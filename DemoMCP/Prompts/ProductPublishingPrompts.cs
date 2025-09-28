using System.ComponentModel;
using ModelContextProtocol.Server;

namespace DemoMCP;

[McpServerPromptType]
public static class ProductPublishingPrompts
{
    [McpServerPrompt, Description("Generate a product insight (markdown) for a given product name or id, then publish it to n8n.")]
    public static string GenerateAndPublishInsight()
    {
        return """
        Task:
        1) Find the product (prefer ProductTools.GetProductById if you have the id; otherwise ProductTools.SearchProducts by name and pick the best match).
        2) Write a concise INSIGHT including:
           - # <Product Name>
           - **Price**, **Rating**
           - Short description
           - 3–5 Key Takeaways (bullets)
           - Who is this for? (1-2 lines)
        3) Call the tool "Publish Insight to n8n" with:
           - title: "Product Insights - <Product Name>"
           - content: the markdown you just wrote
           - metaJson: include a small JSON with { "source": "copilot", "productId": <id or null> }

        Notes:
        - Keep it under ~200 words.
        - Do not include code blocks in the content you send to the publishing tool.
        """;
    }

    [McpServerPrompt, Description("Publish arbitrary content to n8n as-is, e.g., when the user already provided the final text.")]
    public static string PublishProvidedContent()
    {
        return """
        You already have final content from the user or previous steps.
        Simply call the tool "Publish Insight to n8n" with:
          - title: a short, descriptive title (e.g., "Product Insights - <topic>")
          - content: the provided text (markdown or html)
          - metaJson: optional small JSON with any tags/labels

        Return the publishing tool's response (e.g., link).
        """;
    }
}
