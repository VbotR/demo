using demo.Data;
using Microsoft.Extensions.Logging;

namespace demo;

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
        builder.Logging.AddDebug();
#endif

        // Проверяем и применяем миграции, если необходимо
        using (var dbContext = new AppDbContext())
        {
            dbContext.InitializeDatabase();
        }

        return builder.Build();
    }
}
