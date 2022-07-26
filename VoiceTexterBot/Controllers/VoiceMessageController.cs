using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBot.Configuration;
using VoiceTexterBot.Extensions;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers;

public class VoiceMessageController
{
    private ITelegramBotClient _telegramBotClient;
    private IFileHandler _audioFileHandler;
    private AppSettings _appSettings;
    private IStorage _memoryStorage;

    public VoiceMessageController(ITelegramBotClient telegramBotClient, AppSettings appSettings, IFileHandler audioFileHandler, IStorage memoryStorage)
    {
        _telegramBotClient = telegramBotClient;
        _audioFileHandler = audioFileHandler;
        _appSettings = appSettings;
        _memoryStorage = memoryStorage;
    }
    
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        var fileId = message.Voice?.FileId;
        if (message.Type == MessageType.Audio)
        {
            fileId = message.Audio.FileId;
        }

        

        if (fileId == null)
            return;

        await _audioFileHandler.Download(fileId, cancellationToken);
        
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Сообщение загружено, начинаю расшифровку...",
            cancellationToken: cancellationToken);

        var voiceLanguage = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;
        
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, _audioFileHandler.Process(voiceLanguage),
            cancellationToken: cancellationToken);
    }
}