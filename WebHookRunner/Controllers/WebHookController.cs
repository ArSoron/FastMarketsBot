using System.Linq;
using System.Threading.Tasks;
using FastMarketsBot.Services.Telegram.Commands;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FastMarketsBot.WebHookRunner.Controllers
{
    [Route("api/[controller]")]
    public class WebHookController : Controller
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ICommandFactory _commandFactory;
        private readonly IWebApiConfiguration _webApiConfiguration;

        public WebHookController(ITelegramBotClient botClient, ICommandFactory commandFactory, IWebApiConfiguration webApiConfiguration)
        {
            _botClient = botClient;
            _commandFactory = commandFactory;
            _webApiConfiguration = webApiConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            var message = update.Message;

            //Console.WriteLine("Received Message from {0}", message.Chat.Id);

            if (update.Type == UpdateType.Message && message.Type == MessageType.Text)
            {
                var arguments = message.Text.Split(' ').ToArray();
                await _commandFactory.GetCommand(message.Text).ProcessAsync(message, arguments);
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var arguments = update.CallbackQuery.Data.Split(' ').ToArray();
                await _commandFactory
                    .GetCommand(update.CallbackQuery.Data)
                    .ProcessAsync(update.CallbackQuery.Message, arguments);
                await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
            }

            return Ok();
        }

        [HttpGet("subscribe/{id}")]
        public async Task<IActionResult> Subscribe(string id)
        {
            if (id == _webApiConfiguration.SubscribeKey)
            {
                await _botClient.SetWebhookAsync(_webApiConfiguration.HostName);
                return Ok("Subscribed");
            }
            return Forbid("Unauthorized");
        }

        [HttpGet("unsubscribe/{id}")]
        public async Task<IActionResult> UnSubscribe(string id)
        {
            if (id == _webApiConfiguration.SubscribeKey)
            {
                await _botClient.DeleteWebhookAsync();
                return Ok("Unsubscribed");
            }
            return Forbid("Unauthorized");
        }
    }
}
