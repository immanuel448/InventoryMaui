using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryMaui.Models;
using InventoryMaui.Services;

namespace InventoryMaui.ViewModels;

public partial class ProductCreateViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService;

    //[ObservableProperty] crea automáticamente propiedades
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private decimal price;

    [ObservableProperty]
    private int stock;

    [ObservableProperty]
    private bool isBusy;

    public ProductCreateViewModel(ProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    [RelayCommand]
    private async Task CreateProduct()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert(
                "Validación",
                "El nombre del producto es obligatorio.",
                "Aceptar");

            return;
        }

        if (Price < 0)
        {
            await Shell.Current.DisplayAlert(
                "Validación",
                "El precio no puede ser negativo.",
                "Aceptar");

            return;
        }

        if (Stock < 0)
        {
            await Shell.Current.DisplayAlert(
                "Validación",
                "El stock no puede ser negativo.",
                "Aceptar");

            return;
        }

        try
        {
            IsBusy = true;

            var product = new ProductDto
            {
                Name = Name.Trim(),
                Description = Description.Trim(),
                Price = Price,
                Stock = Stock,
                IsActive = true
            };

            var result = await _productApiService.CreateProductAsync(product);

            if (result == null)
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "No fue posible crear el producto.",
                    "Aceptar");

                return;
            }

            await Shell.Current.DisplayAlert(
                "Éxito",
                "Producto creado correctamente.",
                "Aceptar");

            await Shell.Current.GoToAsync("..");
        }
        catch (HttpRequestException)
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
                "Ocurrió un error al crear el producto.",
                "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}