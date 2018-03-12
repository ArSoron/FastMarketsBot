using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FastMarketsBot.WebHookRunner.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IWebApiConfiguration _webApiConfiguration;

        public HomeController(IWebApiConfiguration webApiConfiguration)
        {
            _webApiConfiguration = webApiConfiguration;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Redirect(_webApiConfiguration.BotUrl);
        }
    }
}
