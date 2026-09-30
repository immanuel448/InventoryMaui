
using CommunityToolkit.Mvvm.ComponentModel; // Toolkit para MVVM, facilita propiedades observables
using CommunityToolkit.Mvvm.Input;          // Toolkit para comandos y bindings
using InventoryMaui.Models;                 // Modelos de datos
using InventoryMaui.Services;               // Servicios para consumir API

namespace InventoryMaui.ViewModels;

// ViewModel para editar productos, sigue patrón MVVM
public partial class ProductEditViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService; // Servicio para acceder a la API

    [ObservableProperty] private int id;              // Id del producto
    [ObservableProperty] private string name = "";    // Nombre
    [ObservableProperty] private string description = ""; // Descripción
    [ObservableProperty] private decimal price;       // Precio
    [ObservableProperty] private int stock;           // Stock
    [ObservableProperty] private bool isBusy;         // Estado de carga

    public ProductEditViewModel(ProductApiService productApiService)
    {
        _productApiService = productApiService; // Se inyecta el servicio
    }

    // Método para cargar un producto desde la API, se recibe el id del producto
    public async Task LoadProductAsync(int productId)
    {
        if (IsBusy) return; // Evita llamadas simultáneas

        try
        {
            IsBusy = true;

            var product = await _productApiService.GetProductByIdAsync(productId);

            if (product == null)
            {
                // Mensaje si no se encuentra el producto
                await Shell.Current.DisplayAlert("Producto", "No se encontró el producto.", "Aceptar");
                await Shell.Current.GoToAsync(".."); // Regresa a la página anterior
                return;
            }

            // Asigna valores del producto al ViewModel
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Stock = product.Stock;
        }
        catch (HttpRequestException)
        {
            // Error de conexión con el servidor
            await Shell.Current.DisplayAlert("Error", "No fue posible conectarse con el servidor.", "Aceptar");
        }
        catch (Exception)
        {
            // Error genérico
            await Shell.Current.DisplayAlert("Error", "Ocurrió un error al cargar el producto.", "Aceptar");
        }
        finally
        {
            IsBusy = false; // Libera el estado ocupado
        }
    }

    [RelayCommand]
    private async Task UpdateProduct()
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
                Id = Id,
                Name = Name.Trim(),
                Description = Description.Trim(),
                Price = Price,
                Stock = Stock,
                IsActive = true
            };

            var result = await _productApiService.UpdateProductAsync(Id, product);

            if (result == null)
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "No fue posible actualizar el producto.",
                    "Aceptar");

                return;
            }

            await Shell.Current.DisplayAlert(
                "Éxito",
                "Producto actualizado correctamente.",
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
                "Ocurrió un error al actualizar el producto.",
                "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
