using MySql.Data.MySqlClient;

namespace Accounting;

class DatabaseHandler
{
    private static string? DB_SERVER;
    private static string? DB_NAME;
    private static string? DB_USERNAME;
    private static string? DB_PASSWORD;

    private static string? connectionString;

    public static void LoadConnectionParams()
    {
        DB_SERVER = Environment.GetEnvironmentVariable("DB_SERVER");
        DB_NAME = Environment.GetEnvironmentVariable("DB_NAME");
        DB_USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME");
        DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD");
    }

    public static bool VerifyConnection()
    {
        connectionString = $"Server={DB_SERVER};Database={DB_NAME};User ID={DB_USERNAME};Password={DB_PASSWORD};SslMode=Disabled;";

        try
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection failed:");
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static List<User> LoadUsers()
    {
        throw new NotImplementedException();
    }
}