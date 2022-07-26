using System.ComponentModel.Design;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBot;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;
using VoiceTexterBot.Utilities;

Console.OutputEncoding = Encoding.Unicode;

var host = new HostBuilder()
    .ConfigureServices(((hostContext, services) => ConfigureServices(services)))
    .UseConsoleLifetime()
    .Build();

Console.WriteLine("Сервис запущен");

await host.RunAsync();

Console.WriteLine("Сервис остановлен");


static AppSettings BuildAppSettings()
{
    return new AppSettings()
    {
        BotToken = "5542652336:AAEK2_PAG3XzWyLodC1WYI5TrywezoOrdaY",
        DownloadsFolder = @"C:\Users\Alexey\Desktop\RiderProjects\VoiceTexterBot\DownloadedFiles",
        AudioFileName = "audio",
        InputAudioFormat = "ogg",
        OutputAudioFormat = "wav",
        InputAudioBitrate = 48000F
    };
}

static void ConfigureServices(IServiceCollection services)
{
    AppSettings appSettings = BuildAppSettings();
    services.AddSingleton(appSettings);

    services.AddSingleton<IStorage, MemoryStorage>();
    services.AddSingleton<IFileHandler,AudioFileHandler>();
    
    services.AddTransient<DefaultMessageController>();
    services.AddTransient<VoiceMessageController>();
    services.AddTransient<TextMessageController>();
    services.AddTransient<InlineKeyboardController>();

    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(appSettings.BotToken));
    services.AddHostedService<Bot>();
}