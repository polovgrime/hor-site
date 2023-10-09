using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioWebServer
{
    internal class Server
    {
        private static readonly Dictionary<string, byte[]> pages = new Dictionary<string, byte[]>()
        {
            // {"/", File.ReadAllBytes("./src/index.html") },
            // { "/terminal", File.ReadAllBytes("./src/terminal.html")},
            { "/", File.ReadAllBytes("./src/terminal.html")},
            { "/favicon.ico", File.ReadAllBytes("./src/favicon.png") },
            { "get_out_of_here", File.ReadAllBytes("./src/get_out.png") }
        };

        private static readonly string[] banRoutes = new string[]
        {
            "auth",
            "login",
            "wp.php",
            "admin",
            "wp-login.php",
            "bitrix"
        };

        private static readonly string[] prefixes = new string[]
        {
            "http://*:80/",
            //"http://*:443/"
        };

        private HttpListener server = new HttpListener();

        private static Logger logger = Logger.CreateClassInstaince(nameof(Server));

        public void Start(CancellationToken ct)
        {
            foreach (var prefix in prefixes)
            {
                server.Prefixes.Add(prefix);
            }

            server.Start();
            ExecuteAsync(ct);
        }

        private void ExecuteAsync(CancellationToken ct)
        {
            Task.Run(async () =>
            {
                logger.Info("Server's started");
                while (ct.IsCancellationRequested == false)
                {
                    logger.Info("Listening for next request");
                    var context = await server.GetContextAsync();
                    var request = context.Request;

                    logger.Info($"Request for {request.Url}");
                    logger.Info($"Request from {request.RemoteEndPoint}");

                    var buffer = ResolvePageContent(request.RawUrl);

                    context.Response.ContentLength64 = buffer.Length;
                    using (Stream output = context.Response.OutputStream)
                    {
                        await output.WriteAsync(buffer);
                        await output.FlushAsync();
                    }

                    Thread.Sleep(100);
                }
                server.Stop();
            });
        }

        private static byte[] ResolvePageContent(string url)
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
    }
}
