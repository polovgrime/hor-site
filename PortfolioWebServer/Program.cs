﻿using System.Buffers;
using System.Net;
using System.Text;

var isExecuting = true;
Dictionary<string, byte[]> pages = new Dictionary<string, byte[]>()
{
   // {"/", File.ReadAllBytes("./src/index.html") },
   // { "/terminal", File.ReadAllBytes("./src/terminal.html")},
    { "/", File.ReadAllBytes("./src/terminal.html")}
    { "/favicon.ico", File.ReadAllBytes("./src/favicon.png") },
    { "get_out_of_here", File.ReadAllBytes("./src/get_out.png") }
};

string[] banRoutes = new string[]
{
    "auth",
    "login",
    "wp.php",
    "admin",
    "wp-login.php",
    "bitrix"
};

HttpListener server = new HttpListener();
server.Prefixes.Add("http://*:80/");
//server.Prefixes.Add("http://*:443/");
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

        var buffer = ResolvePageContent(request.RawUrl);

        context.Response.ContentLength64 = buffer.Length;
        using (Stream output = context.Response.OutputStream)
        {
             await output.WriteAsync(buffer);
             await output.FlushAsync();
        }       

        Thread.Sleep(100);
    }
}

byte[] ResolvePageContent(string url)
{
    if (pages.ContainsKey(url) == true)
    {
        return pages[url];
    }

    if (banRoutes.Any(bannedRoute => url.Contains(bannedRoute)))
    {
        return pages["get_out_of_here"];
    }

    return Encoding.UTF8.GetBytes("sorry");
}