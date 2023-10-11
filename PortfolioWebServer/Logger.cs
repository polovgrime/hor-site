namespace PortfolioWebServer
{
    internal class Logger
    {
        private readonly string fileName;
        
        public Logger(string className)
        {
            fileName = $"./logs/{DateTime.Today.ToString("yyyy-MM-dd")}_{className}.log";
            if (Directory.Exists("./logs/") == false)
            {
                Directory.CreateDirectory("./logs/");
            }

            if (File.Exists(fileName) == false)
            {
                File.Create(fileName).Close();
            }
        }

        public void Info(string message)
        {
            Append("Info: " + message);
        }

        public void Error(string message) 
        {
            Append($"Error: {message}");
        }

        private void Append(string message)
        {
            var finalMessage = $"{message} | {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ff")}";
            Console.WriteLine(finalMessage);
            File.AppendAllText(fileName, finalMessage + "\n");
        }

        public static Logger CreateClassInstaince(string className) 
        { 
            return new Logger(className);
        }
    }
}
