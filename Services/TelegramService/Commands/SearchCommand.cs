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

        public override CommandType CommandType => CommandType.Search;

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            var searchResults = _mindTricksService.Search(arguments);
            await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            searchResults.Any() ?
            $"We found following symbols for {string.Join(" ", arguments)}\n" +
            string.Join("\n", searchResults.Select(market => market.ToLongDisplayValue())) :
            "Nothing found",
            ParseMode.Html);
        }
    }
}
