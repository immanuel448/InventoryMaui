using InventoryMaui.ViewModels;

namespace InventoryMaui.Views;

public partial class ProductCreatePage : ContentPage
{
    private readonly ProductCreateViewModel _viewModel;

    public ProductCreatePage(ProductCreateViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}