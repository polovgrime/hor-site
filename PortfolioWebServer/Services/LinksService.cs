using Microsoft.Data.Sqlite;

namespace PortfolioWebServer.Services
{
    public class Link
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public Link(SqliteDataReader reader)
        {
            Url = reader["Url"].ToString()!;
            Title = reader["Title"].ToString()!;
        }
    }

    public class LinksService
    {
        private const string LinkQuery = "SELECT Url, Title FROM Links";

        private readonly SqliteConnection connection;

        public LinksService()
        {
            connection = SqLiteConnectionFactory.Create();
        }

        public IEnumerable<Link> RetrieveLinks()
        {
            var links = new List<Link>();

            using (var cmd = new SqliteCommand(LinkQuery, connection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    links.Add(new Link(reader));
                }

            return links;
        }
    }
}
