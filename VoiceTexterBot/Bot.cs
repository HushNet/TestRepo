﻿using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBot.Controllers;

namespace VoiceTexterBot;

public class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;

    private TextMessageController _textMessageController;
    private VoiceMessageController _voiceMessageController;
    private InlineKeyboardController _inlineKeyboardController;
    private DefaultMessageController _defaultMessageController;

    public Bot(ITelegramBotClient telegramClient, 
        TextMessageController textMessageController, 
        VoiceMessageController voiceMessageController, 
        InlineKeyboardController inlineKeyboardController, 
        DefaultMessageController defaultMessageController)
    {
        _telegramClient = telegramClient;
        _inlineKeyboardController = inlineKeyboardController;
        _textMessageController = textMessageController;
        _voiceMessageController = voiceMessageController;
        _defaultMessageController = defaultMessageController;
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }
        
        if (update.Type == UpdateType.Message)
        {
            Console.WriteLine(update.Message.Type);
            switch (update.Message?.Type)
            {
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                case MessageType.Voice:
                    await _voiceMessageController.Handle(update.Message, cancellationToken);
                    return;
                case MessageType.Audio:
                    await _voiceMessageController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }

    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением");
        Thread.Sleep(10000);

        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
            cancellationToken: stoppingToken);
 
        Console.WriteLine("Бот запущен");
        return Task.CompletedTask;
    }
}