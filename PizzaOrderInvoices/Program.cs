using Azure.Messaging.ServiceBus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PizzaOrderInvoices
{
    internal class Program
    {
        private static string ConnectionString = "Endpoint=sb://c4it-testbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=TY+1cmsgcAyiCJOfWRggfNArokAxmyIY1ozZ/RNgR54=";//hidden

        private static string TopicName = "PizzaOrdersTopic";
        private static string SubscriptionName = "PizzaInvoicesSubscription";

        private static async Task Main(string[] args)
        {
            ServiceBusProcessor _ordersProcessor = null;
            try
            {
                ServiceBusClient serviceBusClient = new ServiceBusClient(ConnectionString);

                _ordersProcessor = serviceBusClient.CreateProcessor(TopicName, SubscriptionName); //SPIEGA
                _ordersProcessor.ProcessMessageAsync += PizzaInvoiceMessageHandler;
                _ordersProcessor.ProcessErrorAsync += PizzaItemErrorHandler;
                await _ordersProcessor.StartProcessingAsync();

                Console.WriteLine("Waiting for pizza orders");
                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_ordersProcessor != null)
                    await _ordersProcessor.StopProcessingAsync();
            }
        }

        private static Task PizzaItemErrorHandler(ProcessErrorEventArgs arg)
        {
            throw new NotImplementedException();
        }

        private static async Task PizzaInvoiceMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();

                var processedPizza = JsonSerializer.Deserialize<Pizza>(body);

                Console.WriteLine($"Creating invoice for pizza {processedPizza.Name}");
                // complete the message. messages is deleted from the queue.
                await args.CompleteMessageAsync(args.Message);
            }
            catch (System.Exception ex)
            {
                // handle exception
            }
        }
    }

    public class Pizza
    {
        public string Name { get; set; }
    }
}