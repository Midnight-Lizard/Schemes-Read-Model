using Microsoft.AspNetCore.Mvc;
using MidnightLizard.KafkaConnect.ElasticSearch.Services;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Controllers
{
    [Route("[controller]/[action]")]
    public class StatusController : Controller
    {
        private readonly IMessagingQueue queue;

        public StatusController(IMessagingQueue eventsQueue)
        {
            this.queue = eventsQueue;
        }

        public IActionResult IsReady()
        {
            return this.Ok("app is ready");
        }

        public IActionResult IsAlive()
        {
            if (this.queue.CheckStatus())
            {
                return this.Ok("app is alive");
            }
            return this.BadRequest("app has too many errors and should be restarted");
        }
    }
}
