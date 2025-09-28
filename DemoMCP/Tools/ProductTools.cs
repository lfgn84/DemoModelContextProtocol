using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace DemoMCP;

[McpServerToolType]
public sealed class ProductTools
{
    private readonly ProductService _productService;

    public ProductTools(ProductService productService)
    {
        _productService = productService;
    }

    [McpServerTool]
    [Description("Retrieves a comprehensive list of all available products from the catalog, including details like price, description, and ratings")]
    [DisplayName("List All Products")]
    public async Task<string> GetProducts()
    {
        var products = await _productService.GetProducts();
        return System.Text.Json.JsonSerializer.Serialize(products);
    }

    [McpServerTool]
    [Description("Locates and returns a specific product using its unique identifier, providing full product details")]
    [DisplayName("Get Product by ID")]
    public async Task<string> GetProductById(
        [Description("The unique numerical identifier of the product to retrieve")] 
        int id)
    {
        var product = await _productService.GetProductById(id);
        return System.Text.Json.JsonSerializer.Serialize(product);
    }

    [McpServerTool]
    [Description("Performs a flexible search across product names, returning all products that either match or contain the search term")]
    [DisplayName("Search Products by Name")]
    public async Task<string> SearchProducts(
        [Description("The product name or partial text to search for (case-insensitive)")] 
        string searchTerm)
    {
        var products = await _productService.GetProductsByName(searchTerm);
        return System.Text.Json.JsonSerializer.Serialize(products);
    }

    [McpServerTool]
    [Description("Filters and returns all products from a specific brand")]
    [DisplayName("Get Products by Brand")]
    public async Task<string> GetProductsByBrand(
        [Description("The brand name to filter products by (case-insensitive)")] 
        string brand)
    {
        var products = await _productService.GetProducts();
        var filtered = products.Where(p => p.Brand?.Equals(brand, StringComparison.OrdinalIgnoreCase) == true).ToList();
        return System.Text.Json.JsonSerializer.Serialize(filtered);
    }

    [McpServerTool]
    [Description("Retrieves all products within a specified price range")]
    [DisplayName("Get Products by Price Range")]
    public async Task<string> GetProductsByPriceRange(
        [Description("The minimum price (inclusive)")] decimal minPrice,
        [Description("The maximum price (inclusive)")] decimal maxPrice)
    {
        var products = await _productService.GetProducts();
        var filtered = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
        return System.Text.Json.JsonSerializer.Serialize(filtered);
    }

    [McpServerTool]
    [Description("Returns all products from a specific category")]
    [DisplayName("Get Products by Category")]
    public async Task<string> GetProductsByCategory(
        [Description("The category name to filter products by (case-insensitive)")] 
        string category)
    {
        var products = await _productService.GetProducts();
        var filtered = products.Where(p => p.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
        return System.Text.Json.JsonSerializer.Serialize(filtered);
    }
}