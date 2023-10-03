using System.Buffers;
using System.Net;
using System.Text;

var isExecuting = true;
Dictionary<string, string> pages = new Dictionary<string, string>()
{
    {"/", File.ReadAllText("./src/index.html") },
    { "/terminal", File.ReadAllText("./src/terminal.html")}
};

HttpListener server = new HttpListener();
server.Prefixes.Add("http://*:80/");
server.Prefixes.Add("http://*:443/");
server.Start();

Task.Run(ExecuteAsync);

Console.WriteLine("Server's started");
Console.ReadLine();
isExecuting = false;

server.Stop();

async Task ExecuteAsync()
{
    while (isExecuting)
    {
        Console.WriteLine("Listening for next request");
        var context = await server.GetContextAsync();
        var request = context.Request;
        Console.WriteLine($"Request from {request.Url}");

        var buffer = Encoding.UTF8.GetBytes(ResolvePageContent(request.RawUrl));

        context.Response.ContentLength64 = buffer.Length;
        using (Stream output = context.Response.OutputStream)
        {
             await output.WriteAsync(buffer);
             await output.FlushAsync();
        }       

        Thread.Sleep(100);
    }
}

string ResolvePageContent(string url)
{
    if (pages.ContainsKey(url) == false)
    {
        return "sorry";
    }

    return pages[url];
}