using InventoryMaui.ViewModels;

namespace InventoryMaui.Views;

public partial class ProductEditPage : ContentPage
{
    private readonly ProductEditViewModel _viewModel;

    public ProductEditPage(ProductEditViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public async Task LoadProductAsync(int productId)
    {
        await _viewModel.LoadProductAsync(productId);
    }
}