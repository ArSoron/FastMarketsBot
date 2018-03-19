using FastMarkets.MindTricksService;
using FastMarkets.MindTricksService.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FastMarketsBot.Services.Telegram.Commands
{
    public class SymbolDetailsCommand : MindTricksCommandBase
    {

        public SymbolDetailsCommand(ITelegramBotClient botClient, IMindTricksService mindTricksService) : base(botClient, mindTricksService)
        {
        }

        public override CommandType CommandType => CommandType.SymbolDetails;

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            string symbolId = arguments.First().TrimStart('/');
            Market market = _mindTricksService.GetMarket(symbolId);
            Enum.TryParse(arguments.Skip(1).FirstOrDefault(), out DisplayType displayType);

            var currentInlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("Same Time Last Month", $"/{symbolId} STLM"),
                            InlineKeyboardButton.WithCallbackData("Same Time Last Year", $"/{symbolId} STLY")
                        },
                        new [] //second row
                        {
                            InlineKeyboardButton.WithCallbackData("Details", $"/{symbolId} Details"),
                        }
                    });
            var detailsInlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            InlineKeyboardButton.WithCallbackData("Add to favourites", $"/favourites {symbolId}")
                        }
                    });
            IReplyMarkup replyMarkup;
            if (market != null && displayType == DisplayType.Current)
                replyMarkup = currentInlineKeyboard;
            else if (market != null && displayType == DisplayType.Details)
                replyMarkup = detailsInlineKeyboard;
            else
                replyMarkup = InlineKeyboardMarkup.Empty();

            string replyText = "";
            if (market == null)
            {
                replyText = "Symbol not found";
            }
            else if (displayType == DisplayType.Details)
            {
                replyText = $"{market.ToLongDisplayValue()}";
            }
            else
            {
                replyText = $"/{market.NormalizedSymbol} "
                    + (displayType == DisplayType.STLY
                        ? "<strong>Same time last year</strong>"
                        : displayType == DisplayType.STLM
                            ? "<strong>Same time last month</strong>"
                            : "")
                    + "\n"
                    + FormatPrice(displayType == DisplayType.STLY
                        ? market.STLY
                        : displayType == DisplayType.STLM
                            ? market.STLM
                            : market.LastPrice);
            }

            await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            replyText,
            ParseMode.Html, replyMarkup: replyMarkup);
        }

        private string FormatPrice(Price price)
        {
            return $"Low: {price.Low}; Mid: {price.Mid}; High: {price.High}; " +
            $"{((price.MidChangeSincePrevious > 0 ? "▲" : "▼") + Math.Abs(price.MidChangeSincePrevious))}\n" +
            $"Assessed on {price.AssessmentDate.ToString("dd MMM yyyy")}";
        }

        enum DisplayType
        {
            Current = 0,
            STLM,
            STLY,
            Details
        }
    }
}
