using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;

namespace UtilityBot.Controllers;

public class TextMessageController
{
    private ITelegramBotClient _telegramBotClient;
    private ISummator _summator;
    private IStorage _storage;

    public TextMessageController(ITelegramBotClient telegramBotClient, ISummator summator, IStorage storage)
    {
        _telegramBotClient = telegramBotClient;
        _summator = summator;
        _storage = storage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":
                var operationButtons = new List<InlineKeyboardButton[]>();

                operationButtons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData("Количество символов", "countChars"),
                    InlineKeyboardButton.WithCallbackData("Сумма чисел БЛА БЛА", "countNums")
                });

                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                    $"Я могу посчитать сколько символов в твоем сообщении, либо посчитать сумму чисел.{Environment.NewLine}"
                    + $"Выбери одну из двух операций ниже.{Environment.NewLine}",
                    cancellationToken: ct, parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(operationButtons));
                return;
            default:
                if (_storage.GetSession(message.Chat.Id).OperationType == "undefined")
                {
                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                        $"<b>Не выбран тип операции. Для выбора команда /start </b>",
                        cancellationToken: ct, parseMode: ParseMode.Html);
                }
                else
                {
                    try
                    {
                        var sumResult = _summator.SumMessageByData(_storage.GetSession(message.Chat.Id).OperationType,
                            message.Text);
                        switch (_storage.GetSession(message.Chat.Id).OperationType)
                        {
                            case "countChars":
                                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Количество символов: {sumResult}");
                                break;
                            case "countNums":
                                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sumResult}");
                                break;
                        }
                        
                    }
                    catch (FormatException)
                    {
                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                            $"Неправильный формат сообщения!");
                    }
                    catch (Exception)
                    {
                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                            $"Непредвиденная ошибка!");
                    }
                }

                break;
        }
    }
}