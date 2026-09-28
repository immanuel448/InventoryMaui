using InventoryMaui.ViewModels;

namespace InventoryMaui.Views;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsViewModel _viewModel;

    public ProductsPage(ProductsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        //Los datos que uses en los Binding vienen de este ViewModel
        BindingContext = _viewModel;
    }

    //se ejecuta cuando la página aparece
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadProductsAsync();
    }
}