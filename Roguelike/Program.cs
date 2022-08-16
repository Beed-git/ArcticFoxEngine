using ArcticFoxEngine;
using ArcticFoxEngine.Services.Logging;
using Roguelike;

var builder = new CoreBuilder()
    .RegisterSingleton<ILogger, LoggerService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>()
    .RegisterSingleton<TestService>();

using var core = builder.Build();
core.Run();

if (Console.ReadKey().Key == ConsoleKey.Spacebar)
{
    core.Running = false;
}