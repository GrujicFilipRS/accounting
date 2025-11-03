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

        connectionString = $"Server={DB_SERVER};Database={DB_NAME};User ID={DB_USERNAME};Password={DB_PASSWORD};SslMode=Disabled;";
    }

    public static bool VerifyConnection()
    {
        try
        {
            using MySqlConnection connection = new(connectionString);
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

    public static void SaveUser(int id, string username, string password, string fullName)
    {
        using MySqlConnection connection = new(connectionString);
        connection.Open();

        // Prepared statement usage
        string query = "INSERT INTO users (id, username, hashedPassword, fullName) VALUES (@id, @username, @password, @fullname)";
        using MySqlCommand cmd = new(query, connection);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@fullname", fullName);

        cmd.ExecuteNonQuery();
    }

    public static List<User> LoadUsers()
    {
        using MySqlConnection connection = new(connectionString);
        connection.Open();

        string query = "SELECT * FROM users;";
        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader reader = cmd.ExecuteReader();

        List<User> users = new();
        while (reader.Read())
        {
            int id = Convert.ToInt32(reader["id"]); // Stupid way of parsing, cuz Int.Parse doesn't work on type object
            string username = reader["username"].ToString()!;
            string pwd = reader["hashedPassword"].ToString()!;
            string fullName = reader["fullName"].ToString()!;

            User user = new(id, username, pwd, fullName);
            users.Add(user);
        }

        return users;
    }

    public static List<Account> LoadAccounts()
    {
        using MySqlConnection connection = new(connectionString);
        connection.Open();

        string query = "SELECT * FROM accounts;";
        using MySqlCommand cmd = new(query, connection);

        using MySqlDataReader reader = cmd.ExecuteReader();

        List<Account> accounts = new();
        while (reader.Read())
        {
            int id = Convert.ToInt32(reader["id"]);
            string address = reader["address"].ToString()!;
            double balance = Convert.ToDouble(reader["balance"]);
            int ownerId = Convert.ToInt32(reader["ownerId"]);
            string accountType = reader["accountType"].ToString()!;
            DateTime lastInterestPayment = Convert.ToDateTime(reader["lastInterestPayment"]);

            if (accountType == "CHECKING")
            {
                CheckingAccount acc = new(id, address, balance, ownerId);
                accounts.Add(acc);
            }
            else if (accountType == "SAVINGS")
            {
                SavingsAccount acc = new(id, address, balance, ownerId, lastInterestPayment);
                accounts.Add(acc);
            }
            else continue;
        }

        return accounts;
    }
}