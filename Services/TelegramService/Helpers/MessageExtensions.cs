using FastMarketsBot.Services.Telegram.Commands;
using System;
using System.Linq;

namespace FastMarketsBot.Services.Telegram.Helpers
{
    public static class MessageExtensions
    {
        public static string GetCommand(this string message) {
            return message.Split('@').First().TrimStart('/').Split(' ').First();
        }
        public static string[] GetArguments(this string message, CommandType commandType)
        {
            string commandName = Enum.GetName(typeof(CommandType), commandType);
            var arguments = message.Split('@').First().TrimStart('/').Split(' ');
            if (arguments.First().Equals(commandName, StringComparison.InvariantCultureIgnoreCase))
                return arguments.Skip(1).ToArray();
            return arguments;
        }
    }
}
