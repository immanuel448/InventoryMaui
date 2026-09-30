using InventoryMaui.ViewModels;

namespace InventoryMaui.Views;

// Permite recibir parámetros en la navegación (ej: id del producto)
[QueryProperty(nameof(ProductId), "id")]
public partial class ProductEditPage : ContentPage
{
    private readonly ProductEditViewModel _viewModel; // ViewModel que maneja la lógica

    // Propiedad que recibe el parámetro "id" desde la navegación
    public string ProductId
    {
        set
        {
            // Si el parámetro es un número válido, carga el producto
            if (int.TryParse(value, out int productId))
            {
                _ = _viewModel.LoadProductAsync(productId);
            }
        }
    }

    // Constructor: inicializa la página y conecta el ViewModel con la vista
    public ProductEditPage(ProductEditViewModel viewModel)
    {
        InitializeComponent(); // Carga el XAML asociado

        _viewModel = viewModel;
        BindingContext = _viewModel; // Enlaza la UI con el ViewModel
    }
}
