using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppConfigDemo.Hubs;
using AppConfigDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AppConfigDemo.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class NotificationHandlerController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _config;

        public NotificationHandlerController(IHubContext<NotificationHub> hubContext, IConfiguration config)
        {
            _hubContext = hubContext;
            _config = config;
        }

        [HttpPost]
        [Route("api/NotificationHandler")]
        public async Task<IActionResult> Post([FromBody]object request)
        {
            //Deserializing the request 
            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();

            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(request.ToString());


            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                // Validate whether EventType is of "Microsoft.EventGrid.SubscriptionValidationEvent"
                if (eventGridEvent.EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                    // Do any additional validation (as required) such as validating that the Azure resource ID of the topic matches
                    // the expected topic and then return back the below response
                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = eventData.ValidationCode
                    };


                    if (responseData.ValidationResponse != null)
                    {

                        return Ok(responseData);
                    }
                }
                else
                {
                    dynamic configMessage = eventGridEvent.Data;

                    string key = configMessage.Key.ToString();

                    if (configMessage.Key == "TestApp:Settings:Message")
                    {
                        var result = await AppConfigConnector.GetValue(key, _config);

                        var message = JsonConvert.DeserializeObject<AppConfigMessage>(result);

                        await _hubContext.Clients.All.SendAsync("SendMessage", message.Value);
                    }
                }
            }

            return Ok();
        }
    }
}