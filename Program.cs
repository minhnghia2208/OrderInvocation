using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using System.Net.Http;
using OrderInvocation.Model;
using System.Text.Json;
using System.Text;
using Azure;
using OrderInvocation;
using Azure.Core.Amqp;
using System.Configuration;

ServiceBusClient client;
ServiceBusProcessor processor;

string ABSconnectionString = ConfigurationManager.ConnectionStrings["ABS"].ConnectionString;
string QueueConnectionString = ConfigurationManager.ConnectionStrings["Queue"].ConnectionString;


var clientOptions = new ServiceBusClientOptions()
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};
client = new ServiceBusClient(ABSconnectionString, clientOptions);
processor = client.CreateProcessor(QueueConnectionString, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;
    await processor.StartProcessingAsync();

    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

async Task MessageHandler(ProcessMessageEventArgs args)
{
    Order body = args.Message.Body.ToObjectFromJson<Order>();
    body.toString();
    await Utils.InvokeEvent(new HttpClient(), body);
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}