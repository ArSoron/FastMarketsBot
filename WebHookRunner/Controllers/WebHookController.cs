using System.Linq;
using System.Threading.Tasks;
using FastMarketsBot.Services.Telegram.Commands;
using FastMarketsBot.Services.Telegram.Helpers;
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

            if ((update.Type == UpdateType.Message || update.Type == UpdateType.EditedMessage) && update.Message.Type == MessageType.Text)
            {
                var command = _commandFactory.GetCommand(update.Message.Text);
                var arguments = update.Message.Text.GetArguments(command.CommandType);
                await command.ProcessAsync(update.Message, arguments);
                return Ok();
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var command = _commandFactory.GetCommand(update.CallbackQuery.Data);
                var arguments = update.CallbackQuery.Data.GetArguments(command.CommandType);
                await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                await command
                    .ProcessAsync(update.CallbackQuery.Message, arguments);
                return Ok();
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
