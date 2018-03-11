using Telegram.Bot;
using FastMarketsBot.Services.Telegram.Helpers;
using System;
using System.Linq;
using FastMarkets.MindTricksService;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly SelfUpdatingMessage _selfUpdatingMessage;
        private readonly IMindTricksService _mindTricksService;

        public CommandFactory(ITelegramBotClient botClient, SelfUpdatingMessage selfUpdatingMessage, IMindTricksService mindTricksService)
        {
            _botClient = botClient;
            _selfUpdatingMessage = selfUpdatingMessage;
            _mindTricksService = mindTricksService;
        }

        public ICommand GetCommand(CommandType command)
        {
            switch (command)
            {
                case CommandType.Start:
                    return new StartCommand(_botClient);
                case CommandType.Favourites:
                    return new FavouritesCommand(_botClient, _mindTricksService);
                case CommandType.Help:
                default:
                    return new StartCommand(_botClient);
            }
        }

        public ICommand GetCommand(string messageText)
        {
            if (Enum.TryParse(messageText.Split(' ').First().TrimStart('/'), true, out CommandType commandType))
            {
                return GetCommand(commandType);
            }
            if (!messageText.StartsWith("/"))
            {
                return new SearchCommand(_botClient, _mindTricksService);
            }
            return new SymbolDetailsCommand(_botClient, _mindTricksService);
        }
    }
}
