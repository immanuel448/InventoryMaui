using InventoryMaui.Services;
using InventoryMaui.ViewModels;
using Microsoft.Extensions.Logging;

namespace InventoryMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient<ProductApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7242/");
            });

            builder.Services.AddTransient<ProductsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
