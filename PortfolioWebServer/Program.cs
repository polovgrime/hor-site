using PortfolioWebServer;
using System.Buffers;
using System.Net;
using System.Text;

public class Program
{
    

    public static void Main(string[] args)
    {
        
        var cancellationTokenSource = new CancellationTokenSource();
        var server = new Server();

        server.Start(cancellationTokenSource.Token);

        Console.ReadLine();
        cancellationTokenSource.Cancel();
    }

   

    
}
