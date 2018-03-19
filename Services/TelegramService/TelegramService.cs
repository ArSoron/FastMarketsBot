using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using FastMarketsBot.Services.Telegram.Commands;
using FastMarketsBot.Services.Telegram.Helpers;

namespace FastMarketsBot.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly ILogger<TelegramService> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly ICommandFactory _commandFactory;

        public TelegramService(ITelegramBotClient botClient, ILoggerFactory loggerFactory,ICommandFactory commandFactory)
        {
            _logger = loggerFactory.CreateLogger<TelegramService>();
            _botClient = botClient;
            _commandFactory = commandFactory;

            _logger.LogInformation("Initialized");
        }

        public void Register()
        {
            using (_logger.BeginScope("Service registration"))
            {
                _logger.LogInformation("Starting");
                _botClient.OnMessage += BotOnMessageReceived;
                _botClient.OnMessageEdited += BotOnMessageReceived;
                _botClient.OnCallbackQuery += BotOnCallbackQueryReceived;

                _botClient.OnReceiveError += BotOnReceiveError;

                _botClient.OnReceiveGeneralError += BotOnReceiveGeneralError;

                _botClient.StartReceiving();
                _logger.LogInformation("Now Receiving");
            }
        }

        public void StayingAlive()
        {
            _logger.LogInformation($"Ah, ha, ha, ha, stayin' alive @ {DateTime.UtcNow}");
        }

        #region Handlers
        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            var command = _commandFactory.GetCommand(message.Text);
            var arguments = message.Text.GetArguments(command.CommandType);
            await command.ProcessAsync(message, arguments);
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var command = _commandFactory
                .GetCommand(callbackQueryEventArgs.CallbackQuery.Data);
            var arguments = callbackQueryEventArgs.CallbackQuery.Data.GetArguments(command.CommandType);
            await command
                .ProcessAsync(callbackQueryEventArgs.CallbackQuery.Message, arguments);
            await _botClient.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id);
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            _logger.LogError($"Received error: {receiveErrorEventArgs.ApiRequestException.ErrorCode} — {receiveErrorEventArgs.ApiRequestException.Message}");
        }

        private void BotOnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs receiveGeneralErrorEventArgs)
        {
            _logger.LogError(receiveGeneralErrorEventArgs.Exception, "Received general error");
        }
        #endregion

        public void Dispose()
        {
            using (_logger.BeginScope("Service Unregistration"))
            {
                _botClient.StopReceiving();
            }
        }
    }
}
