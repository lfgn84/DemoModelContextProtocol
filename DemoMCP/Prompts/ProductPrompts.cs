using System.ComponentModel;
using ModelContextProtocol.Server;

namespace DemoMCP;

[McpServerPromptType]
public static class ProductPrompts
{

    [McpServerPrompt, Description("High-level overview of the entire catalog.")]
    public static string CatalogOverview(
        [Description("Max items to analyze (fetch all, then cap locally)")] int cap = 200)
    {
        return $"""
        Call the MCP tool **"List All Products"**.
        The tool returns a JSON string. Parse it into a list of products with fields like: Id, Title, Brand, Category, Price, Rating.

        Then:
        - Consider at most {cap} items for analysis (if more, use the first {cap}).
        - Report total count, distinct brand count, distinct category count.
        - Compute average price, median price, min/max price, and average rating.
        - Show a Top 10 table by Rating with columns: Id | Title | Brand | Category | Price | Rating.
        - Finish with 3 brief insights (e.g., popular categories, notable bargains, price clusters).

        Output markdown.
        """;
    }

    [McpServerPrompt, Description("Fetch and summarize a specific product by its ID.")]
    public static string ProductDetailsById(
        [Description("The product id to fetch")] int id)
    {
        return $"""
        Call **"Get Product by ID"** with id={id}.
        Parse the returned JSON string into a product object.

        Then present:
        - A concise summary paragraph (Title, Brand, Category, Price, Rating).
        - A small markdown table with the key fields (Id, Title, Description, Price, DiscountPercentage if present, Rating, Stock, Thumbnail).
        - A short bullet list of possible use cases or buyer personas based on the description.
        """;
    }

    [McpServerPrompt, Description("Search products by a term and rank them.")]
    public static string SearchAndRank(
        [Description("Term to search for")] string q,
        [Description("How many top results to show")] int top = 10)
    {
        return $"""
        Call **"Search Products by Name"** with searchTerm="{q}".
        Parse the returned JSON string into a list of products.

        Rank results by Rating (desc), then by Price (asc) as a tiebreaker.
        Show the top {top} in a table: Id | Title | Brand | Category | Price | Rating.
        Add a short recommendation: which 2 items are best value (consider rating/price).
        """;
    }

    [McpServerPrompt, Description("Compare two brands under an optional price ceiling.")]
    public static string CompareBrands(
        [Description("First brand")] string brandA,
        [Description("Second brand")] string brandB,
        [Description("Only consider products priced at or below this amount (0 = no ceiling)")] decimal maxPrice = 0)
    {
        return $"""
        Call **"Get Products by Brand"** twice: once with brand="{brandA}", once with brand="{brandB}".
        Parse both JSON strings into product lists.

        If maxPrice > 0, filter each list to Price <= {maxPrice}.

        For each brand compute:
        - count
        - avg price
        - avg rating
        - best value score = rating / price (higher is better)
        - top 3 items by value score

        Output:
        - A side-by-side summary table per brand with the metrics above.
        - A combined Top 6 by value score (Id | Title | Brand | Price | Rating | ValueScore).
        - A brief recommendation of which brand offers better value and why.
        """;
    }

    [McpServerPrompt, Description("Find best-value products within a category.")]
    public static string BestValueInCategory(
        [Description("Target category")] string category,
        [Description("How many items to return")] int top = 5)
    {
        return $"""
        Call **"Get Products by Category"** with category="{category}".
        Parse the returned JSON string into a list of products.

        Compute ValueScore = Rating / Price (handle divide-by-zero if needed).
        Return the top {top} items by ValueScore in a table:
        Id | Title | Brand | Price | Rating | ValueScore.

        Then add a short buying guide: what type of user each item suits best.
        """;
    }

    [McpServerPrompt, Description("Surface good deals inside a price range.")]
    public static string DealsInPriceRange(
        [Description("Min price (inclusive)")] decimal minPrice,
        [Description("Max price (inclusive)")] decimal maxPrice,
        [Description("Top N results to display")] int top = 10)
    {
        return $"""
        Call **"Get Products by Price Range"** with minPrice={minPrice} and maxPrice={maxPrice}.
        Parse the returned JSON string into a list of products.

        Rank primarily by Rating (desc), then Price (asc).
        Show the top {top} as a table: Id | Title | Brand | Category | Price | Rating.
        Conclude with 3 quick suggestions (budget pick, balanced pick, premium pick).
        """;
    }

    [McpServerPrompt, Description("Return raw JSON for a brand (debug).")]
    public static string RawBrandJson([Description("Brand to fetch")] string brand)
    {
        return $"""
        Call **"Get Products by Brand"** with brand="{brand}" and return the raw JSON string verbatim in a fenced code block.
        """;
    }
}
