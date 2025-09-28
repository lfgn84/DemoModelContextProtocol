using System.Net.Http.Json;

namespace DemoMCP;

public class ProductService
{
    private readonly HttpClient httpClient;
    private List<ProductDto> productList = new();

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
    }

    public async Task<List<ProductDto>> GetProducts()
    {
        if (productList?.Count > 0)
            return productList;

        // Online
        var response = await httpClient.GetAsync("https://dummyjson.com/products");
        if (response.IsSuccessStatusCode)
        {
            var productResponse = await response.Content.ReadFromJsonAsync<ProductListResponseDto>();
            productList = productResponse?.Products ?? [];
        }

        productList ??= [];

        return productList;
    }

    public async Task<ProductDto?> GetProductById(int id)
    {
        var products = await GetProducts();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<List<ProductDto>?> GetProductsByName(string title)
    {
        var products = await GetProducts();
        
        return products?.Where(p => p.Title?.Equals(title, StringComparison.OrdinalIgnoreCase) == true || p.Title?.Contains(title, StringComparison.OrdinalIgnoreCase) == true ).ToList();
    }
}