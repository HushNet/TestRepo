using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers;

public class TextMessageController
{
    private ITelegramBotClient _telegramBotClient;

    public TextMessageController(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":

                var languageButtons = new List<InlineKeyboardButton[]>();
                languageButtons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Русский", "ru"), 
                        InlineKeyboardButton.WithCallbackData("English", "en") 
                    });

                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                    $"<b> Этот бот превращает голосовые сообщения в текст.</b>{Environment.NewLine}"
                    + $"{Environment.NewLine}Пожалуйста, выберите язык голосового сообщения{Environment.NewLine}"
                +$"Please choose the voice message language.{Environment.NewLine}"
                    , cancellationToken: ct, parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(languageButtons));
                break;
            default:
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                    $"Отправьте аудио сообщение, которое хотите превратить в текст.",
                    cancellationToken: ct);
                break;
        }
    }
}