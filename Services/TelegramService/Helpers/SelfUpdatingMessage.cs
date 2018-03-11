using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FastMarketsBot.Services.Telegram.Helpers
{
    public class SelfUpdatingMessage
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<SelfUpdatingMessage> _logger;
        private static IDictionary<long, CancellationTokenSource> userTokens 
            = new Dictionary<long, CancellationTokenSource>();

        public SelfUpdatingMessage(ITelegramBotClient botClient, ILoggerFactory loggerFactory) {
            _botClient = botClient;
            _logger = loggerFactory.CreateLogger<SelfUpdatingMessage>();
        }

        public async Task Send(long chatId, Func<Task<string>> messageFunc,
            ParseMode parseMode = ParseMode.Html)
        {
            await Send(chatId, messageFunc, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60), parseMode);
        }

        public async Task Send(long chatId, Func<Task<string>> messageFunc,
            TimeSpan interval, TimeSpan duration, ParseMode parseMode = ParseMode.Html)
        {
            if (userTokens.ContainsKey(chatId))
            {
                lock (userTokens)
                {
                    if (userTokens.ContainsKey(chatId))
                    {
                        userTokens[chatId].Cancel();
                        userTokens.Remove(chatId);
                    }
                }
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(duration);
            userTokens.Add(chatId, cancellationTokenSource);

            string messageText = "";
            Message sentMessage = null;
            await Repeat.Interval(interval, async () =>
            {
                var oldMessageText = messageText;
                messageText = await messageFunc();
                if (messageText != oldMessageText)
                {
                    try
                    {
                        sentMessage = sentMessage?.MessageId == null
                        ? await _botClient.SendTextMessageAsync(chatId, "🔄 " + messageText, parseMode)
                        : await _botClient.EditMessageTextAsync(chatId, sentMessage.MessageId, "🔄 " + messageText, parseMode);
                    } catch (Exception ex)
                    {
                        _logger.LogError(ex,"Error while sending self-updating message");
                    }
                }
            }, cancellationTokenSource.Token);
            try
            {
                if (sentMessage?.MessageId > 0)
                {
                    await _botClient.EditMessageTextAsync(chatId, sentMessage.MessageId, messageText, parseMode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to finalize self-updating message");
            }
        }
    }
}
