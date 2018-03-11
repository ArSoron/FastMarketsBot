﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class StartCommand : CommandBase
    {
        const string welcomeMessage = "Hello! Welcome to FastMarkets Bot!\n"+
"Try /favourites to list default instruments";

        public StartCommand(ITelegramBotClient botClient) : base(botClient)
        {
        }

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            await _botClient.SendTextMessageAsync(message.Chat.Id, welcomeMessage);
        }
    }
}
