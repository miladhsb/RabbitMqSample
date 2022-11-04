using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitMQ.Client;
using RabbitMqSample.DTOs;

namespace RabbitMqSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRabbitManager _rabbitManager;

        public IndexModel(ILogger<IndexModel> logger, IRabbitManager rabbitManager)
        {
            _logger = logger;
            this._rabbitManager = rabbitManager;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetPublishMessage()
        {


            _rabbitManager.Publish<PersonDTo>(
                message: new PersonDTo { lastname = "last", name = "name" },
                QueueName:"milad",
                exchangeName: "milad.person",
                exchangeType: ExchangeType.Topic,
                routeKey: "*.queue.durable.dotnetcore.#");



            return RedirectToPage("");
        }

       

    }
}