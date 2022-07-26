using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBot.Models;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers;

public class InlineKeyboardController
{
    private ITelegramBotClient _telegramBotClient;
    private IStorage _memoryStorage;

    public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramBotClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }
    
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery.Data == null)
            return;

        _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

        string languageText = callbackQuery.Data switch
        {
            "ru" => "Русский",
            "en" => "Английский",
            _ => string.Empty
        };

        await _telegramBotClient.SendTextMessageAsync(callbackQuery.From.Id,
            $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" +
            $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

    }
}