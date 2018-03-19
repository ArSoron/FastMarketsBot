using System;
using System.Linq;
using System.Collections.Generic;
using FastMarketsBot.Services.Telegram.Helpers;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IEnumerable<ICommand> _commands;
        private readonly ICommand _defaultCommand;

        public CommandFactory(IEnumerable<ICommand> commands, ICommand defaultCommand)
        {
            _commands = commands;
            _defaultCommand = defaultCommand;
        }

        public ICommand GetCommand(CommandType commandType)
        {
            return _commands.FirstOrDefault(command => command.CommandType == commandType) ??_defaultCommand ;
        }

        public ICommand GetCommand(string messageText)
        {
            if (Enum.TryParse(messageText.GetCommand(), true, out CommandType commandType))
            {
                return GetCommand(commandType);
            }
            if (!messageText.StartsWith("/"))
            {
                return GetCommand(CommandType.Search);
            }
            return GetCommand(CommandType.SymbolDetails);
        }
    }
}
