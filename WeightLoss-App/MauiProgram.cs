using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using WeightLoss_App.Database;
using WeightLoss_App.ViewModels;
using WeightLoss_App.Views;

namespace WeightLoss_App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureSyncfusionCore();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<Weight>();
        builder.Services.AddSingleton<Meals>();
        builder.Services.AddSingleton<Workout>();
        builder.Services.AddSingleton<Settings>();
        builder.Services.AddSingleton<WeightViewModel>();
        
        builder.Services.AddSingleton<WeightDatabase>();

        return builder.Build();
    }
}