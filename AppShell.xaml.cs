using InventoryMaui.Views;

namespace InventoryMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // registra las rutas para navegar entre páginas
        Routing.RegisterRoute(nameof(ProductCreatePage), typeof(ProductCreatePage));
        Routing.RegisterRoute(nameof(ProductEditPage), typeof(ProductEditPage));
    }
}