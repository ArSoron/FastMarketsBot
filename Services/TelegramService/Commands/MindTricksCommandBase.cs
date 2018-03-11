using FastMarkets.MindTricksService;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public abstract class MindTricksCommandBase : CommandBase
    {
        protected readonly IMindTricksService _mindTricksService;

        protected MindTricksCommandBase(ITelegramBotClient botClient, IMindTricksService mindTricksService) : base(botClient)
        {
            _mindTricksService = mindTricksService;
        }
    }
}
