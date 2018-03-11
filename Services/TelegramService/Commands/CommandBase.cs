using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public abstract class CommandBase: ICommand
    {
        protected readonly ITelegramBotClient _botClient;

        protected CommandBase(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public abstract Task ProcessAsync(Message message, params string[] arguments);
    }
}
