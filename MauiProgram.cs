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

            //En esta aplicación de desarrollo, acepta el certificado HTTPS de nuestra API aunque sea un certificado de desarrollo
            builder.Services.AddHttpClient<ProductApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7242/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<ProductCreateViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
