using FastMarkets.MindTricksService;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class FavouritesCommand : MindTricksCommandBase
    {

        public FavouritesCommand(ITelegramBotClient botClient, IMindTricksService mindTricksService) : base(botClient, mindTricksService)
        {
        }

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            string.Join("\n", _mindTricksService.GetFavourites().Select(symbol => $"/{symbol.Id}\n{symbol.Description}")),
            ParseMode.Html);
        }
    }
}
