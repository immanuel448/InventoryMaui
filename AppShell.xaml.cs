using InventoryMaui.Views;

namespace InventoryMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ProductCreatePage), typeof(ProductCreatePage));
    }
}