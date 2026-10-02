using CommunityToolkit.Mvvm.ComponentModel; // Toolkit MVVM: facilita propiedades observables
using CommunityToolkit.Mvvm.Input;          // Toolkit MVVM: genera comandos automáticamente
using InventoryMaui.Models;                 // Modelos de datos
using InventoryMaui.Services;               // Servicios API
using InventoryMaui.Views;                  // Vistas de navegación
using System.Collections.ObjectModel;

namespace InventoryMaui.ViewModels;

// ViewModel principal de productos
public partial class ProductsViewModel : ObservableObject
{
    private readonly ProductApiService _productApiService; // Servicio para consumir API

    [ObservableProperty]
    private ObservableCollection<ProductDto> products = new(); // Lista observable de productos

    [ObservableProperty]
    private bool isBusy; // Estado de carga para mostrar spinner

    public ProductsViewModel(ProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    // Método que carga productos desde la API
    public async Task LoadProductsAsync()
    {
        if (IsBusy) return; // Evita llamadas simultáneas

        try
        {
            IsBusy = true;

            var products = await _productApiService.GetProductsAsync();
            Products = new ObservableCollection<ProductDto>(products); // Actualiza la lista
        }
        catch (HttpRequestException)
        {
            // Error de conexión
            await Shell.Current.DisplayAlert("Error", "No fue posible conectarse con el servidor.", "Aceptar");
        }
        catch (Exception)
        {
            // Error genérico
            await Shell.Current.DisplayAlert("Error", "Ocurrió un error al cargar los productos.", "Aceptar");
        }
        finally
        {
            IsBusy = false; // Libera estado ocupado
        }
    }

    // Comandos que se conectan con la UI (XAML)
    [RelayCommand] private async Task LoadProducts() => await LoadProductsAsync(); // Actualizar lista
    [RelayCommand] private async Task GoToCreate() => await Shell.Current.GoToAsync(nameof(ProductCreatePage)); // Ir a crear producto
    [RelayCommand] private async Task EditProduct(int productId) => await Shell.Current.GoToAsync($"{nameof(ProductEditPage)}?id={productId}"); // Ir a editar producto

    // Comando para eliminar producto
    [RelayCommand]
    private async Task DeleteProduct(int productId)
    {
        if (IsBusy) return;

        bool confirm = await Shell.Current.DisplayAlert("Eliminar producto", "¿Estás seguro?", "Sí", "No");
        if (!confirm) return;

        try
        {
            IsBusy = true;

            var success = await _productApiService.DeleteProductAsync(productId);
            if (!success)
            {
                await Shell.Current.DisplayAlert("Error", "No fue posible eliminar el producto.", "Aceptar");
                return;
            }

            await Shell.Current.DisplayAlert("Éxito", "Producto eliminado correctamente.", "Aceptar");
            var product = Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                Products.Remove(product);
            } // Recarga lista después de eliminar
        }
        catch (HttpRequestException)
        {
            await Shell.Current.DisplayAlert("Error", "No fue posible conectarse con el servidor.", "Aceptar");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Error", "Ocurrió un error al eliminar el producto.", "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
