using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PizzaOrderSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzaOrderController : ControllerBase
    {
        private static string ConnectionString = "";//hidden

        private string QueueName = "PizzaOrders";

        [HttpPost]
        public async Task<IActionResult> CreateOrder(IEnumerable<PizzaOrder> orders)
        {
            await ProcessOrder(orders);
            return Ok();
        }

        private async Task ProcessOrder(IEnumerable<PizzaOrder> validOrders)
        {
            await using (ServiceBusClient client = new ServiceBusClient(ConnectionString))
            {
                ServiceBusSender sender = client.CreateSender(QueueName);

                foreach (var order in validOrders)
                {
                    // Serialize as JSON string

                    var jsonEntity = JsonSerializer.Serialize(order);

                    /// Create Bus Message
                    var serializedContents = new ServiceBusMessage(jsonEntity);

                    // Send the message on the Bus
                    await sender.SendMessageAsync(serializedContents);
                }
            }
        }
    }
}