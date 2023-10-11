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
            var finalMessage = "Info: " + message;
            Append(finalMessage);
        }

        public void Error(string message) 
        {
            var finalMessage = $"Error: {message} | {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ff")}";      
            Append(finalMessage);
        }

        private void Append(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(fileName, message + "\n");
        }

        public static Logger CreateClassInstaince(string className) 
        { 
            return new Logger(className);
        }
    }
}
