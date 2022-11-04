using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitMqMassTransit.DTOs;

namespace RabbitMqMassTransit.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public IndexModel(ILogger<IndexModel> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this._publishEndpoint = publishEndpoint;
        }

        public void OnGet()
        {
            _publishEndpoint.Publish<Person>(new Person() { Name = "milad", Lastname = "karami" });
        }
    }
}