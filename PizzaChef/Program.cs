using Azure.Messaging.ServiceBus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PizzaChef
{
    internal class Program
    {
        private static string ConnectionString = "Endpoint=sb://c4it-testbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=TY+1cmsgcAyiCJOfWRggfNArokAxmyIY1ozZ/RNgR54=";//hidden

        private static string TopicName = "PizzaOrdersTopic";
        private static string SubscriptionName = "PizzaChefSubscription";

        private static async Task Main(string[] args)
        {
            ServiceBusProcessor _editorialMessagesProcessor = null;
            try
            {
                ServiceBusClient serviceBusClient = new ServiceBusClient(ConnectionString);

                _editorialMessagesProcessor = serviceBusClient.CreateProcessor(TopicName, SubscriptionName); //SPIEGA
                _editorialMessagesProcessor.ProcessMessageAsync += PizzaItemMessageHandler;
                _editorialMessagesProcessor.ProcessErrorAsync += PizzaItemErrorHandler;
                await _editorialMessagesProcessor.StartProcessingAsync();

                Console.WriteLine("Waiting for pizza orders");
                Console.ReadKey();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_editorialMessagesProcessor != null)
                    await _editorialMessagesProcessor.StopProcessingAsync();
            }
        }

        private static Task PizzaItemErrorHandler(ProcessErrorEventArgs arg)
        {
            throw new NotImplementedException();
        }

        private static async Task PizzaItemMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();
                //Console.WriteLine("Received " + body);

                var processedPizza = JsonSerializer.Deserialize<ProcessedPizzaOrder>(body);

                Console.WriteLine($"Processing {processedPizza}");

                // complete the message. messages is deleted from the queue.
                await args.CompleteMessageAsync(args.Message);
            }
            catch (System.Exception ex)
            {
                // handle exception
            }
        }
    }
}