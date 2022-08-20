using ArcticFoxEngine;
using ArcticFoxEngine.Services.TextureManager;
using ArcticFoxEngine.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Roguelike;

var serviceCollection = new ServiceCollection()
    .AddLogging(options =>
    {
        options.SetMinimumLevel(LogLevel.Trace);
        options.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.IncludeScopes = true;
            options.TimestampFormat = "hh:mm:ss";
            options.UseUtcTimestamp = false;
        });
    })
    .AddWindow<WindowEventHandler>()
    .AddTextureManager()
    .AddSingleton<Renderer>()
;

using var core = new Core(serviceCollection);
core.Run();