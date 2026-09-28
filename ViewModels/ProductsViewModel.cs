using CommunityToolkit.Mvvm.ComponentModel;
using InventoryMaui.Models;
using InventoryMaui.Services;
using System.Collections.ObjectModel;

namespace InventoryMaui.ViewModels;

//ObservableObject, permite que la interfaz detecte cambios en las propiedades.
public partial class ProductsViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService;

    //genera automáticamente las propiedades públicas.
    [ObservableProperty]
    private ObservableCollection<ProductDto> products = new();

    [ObservableProperty]
    private bool isBusy;//para saber si estamos cargando información

    public ProductsViewModel(ProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    //llama al servicio y obtiene los productos.    
    public async Task LoadProductsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var products = await _productApiService.GetProductsAsync();

            Products = new ObservableCollection<ProductDto>(products);
        }
        catch (HttpRequestException)//para manejar errores de conexión con el servidor
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "No fue posible conectarse con el servidor.",
                "Aceptar");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "Ocurrió un error al cargar los productos.",
                "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}