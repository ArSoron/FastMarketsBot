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

        public override async Task ProcessAsync(Message message, params string[] arguments)
        {
            string symbolId = arguments.First().TrimStart('/');
            Symbol symbol = _mindTricksService.GetSymbol(symbolId);
            Enum.TryParse(arguments.Skip(1).FirstOrDefault(), out PriceType priceType);

            InlineKeyboardButton current = InlineKeyboardButton.WithCallbackData("Current", $"/{symbolId}");

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // first row
                        {
                            priceType != PriceType.STLM ? InlineKeyboardButton.WithCallbackData("STLM", $"/{symbolId} STLM") : current,
                            priceType != PriceType.STLY ? InlineKeyboardButton.WithCallbackData("STLY", $"/{symbolId} STLY") : current
                        }
                    });

            await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            symbol != null ?
            $"/{symbol.Id} " + (priceType == PriceType.STLY ? "Same time last year" : priceType == PriceType.STLM ? "Same time last month" : "") + "\n" +
            FormatPrice(priceType == PriceType.STLY ? symbol.STLY : priceType == PriceType.STLM ? symbol.STLM : symbol.LastPrice) :
            "Symbol not found",
            ParseMode.Html, replyMarkup: symbol != null? inlineKeyboard: InlineKeyboardMarkup.Empty());
        }

        private string FormatPrice(Price price)
        {
            return $"Low: {price.Low}; Mid: {price.Mid}; High: {price.High}; " +
            $"{((price.MidChangeSincePrevious > 0 ? "▲" : "▼") + Math.Abs(price.MidChangeSincePrevious))}\n" +
            $"Assessed at {price.AssessmentDate}";
        }

        enum PriceType
        {
            Current = 0,
            STLM,
            STLY
        }
    }
}
