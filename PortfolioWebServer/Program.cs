using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PortfolioWebServer;
using PortfolioWebServer.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        await StateService.CreateState();

        var cancellationTokenSource = new CancellationTokenSource();
        var server = new Server();

        server.Start(cancellationTokenSource.Token);

        Console.ReadLine();
        cancellationTokenSource.Cancel();
    }
}
