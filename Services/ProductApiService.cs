using InventoryMaui.Models;
using System.Net.Http.Json;

namespace InventoryMaui.Services;

public class ProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //para obtener los productos
    public async Task<List<ProductDto>> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/products")
               ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/products/{id}");

        if (!response.IsSuccessStatusCode)//si no encuentra nada
            return null;

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, ProductDto product)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/products/{id}",
            product);

        if (!response.IsSuccessStatusCode)
            return null;

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return product;

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }

    public async Task<ProductDto?> CreateProductAsync(ProductDto product)
    {
        //PostAsJsonAsync() envía el producto a POST /api/products.
        var response = await _httpClient.PostAsJsonAsync("api/products", product);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }
}