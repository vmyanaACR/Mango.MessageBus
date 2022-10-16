using Mango.MessageBus;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

public class AzureServiceBus : IMessageBus
{
    private string connectionString = "Endpoint=sb://mangorestaurantcore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=E7TpO0KlgyWAQK5qYt54+RtgB7o46CHh7T4JiSCiQ08=";
    public async Task PublishMessage(BaseMessage baseMessage, string topicName)
    {
        await using var client = new ServiceBusClient(connectionString);
        ServiceBusSender sender = client.CreateSender(topicName);
        var jsonMessage = JsonConvert.SerializeObject(baseMessage);
        ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };
        await sender.SendMessageAsync(finalMessage);
        await client.DisposeAsync();
    }
}