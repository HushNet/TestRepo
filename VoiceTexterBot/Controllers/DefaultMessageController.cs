using Telegram.Bot;
using Telegram.Bot.Types;

namespace VoiceTexterBot.Controllers;

public class DefaultMessageController
{
    private ITelegramBotClient _telegramBotClient;

    public DefaultMessageController(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Данный тип сообщения не поддерживается",
            cancellationToken: cancellationToken);
    }
}