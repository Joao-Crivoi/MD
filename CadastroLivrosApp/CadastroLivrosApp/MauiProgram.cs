using CadastroLivrosApp.Services;
using Microsoft.Extensions.Logging;
using System.IO; // Necessário para Path.Combine

namespace CadastroLivrosApp
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

#if DEBUG
            // Adiciona suporte a logs de debug.
            builder.Logging.AddDebug();
#endif

            
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "Book.db3");
           
            builder.Services.AddSingleton<string>(databasePath);

            
            builder.Services.AddSingleton<BookDatabase>();

            
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<BookDetailPage>();

            return builder.Build();
        }
    }
}