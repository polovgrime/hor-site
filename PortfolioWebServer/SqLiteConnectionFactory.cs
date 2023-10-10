using Microsoft.Data.Sqlite;

public static class SqLiteConnectionFactory
{
    private static readonly string connectionString;

    static SqLiteConnectionFactory()
    {
        var horSiteDbPath = Environment.GetEnvironmentVariable("HorWebsiteDb");

        connectionString = $"Data Source={horSiteDbPath}";
    }

    public static SqliteConnection Create()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        return connection;
    }
}
