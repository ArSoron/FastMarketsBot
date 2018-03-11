using FastMarkets.MindTricksService;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class SearchCommand : MindTricksCommandBase
    {

        public SearchCommand(ITelegramBotClient botClient, IMindTricksService mindTricksService) : base(botClient, mindTricksService)
        {
        }

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            var searchResults = _mindTricksService.Search(message.Text);
            await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            searchResults.Any()?
            $"We found following symbols for {message.Text}\n" +
            string.Join("\n", searchResults.Select(symbol => $"/{symbol.Id}\n{symbol.Description}")):
            "Nothing found",
            ParseMode.Html);
        }
    }
}
