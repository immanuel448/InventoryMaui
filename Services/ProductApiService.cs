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
}