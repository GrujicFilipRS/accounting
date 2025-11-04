using dotenv.net;

namespace Accounting;

class Application
{
    public static void Run()
    {
        User.Register("fgrujic123", "TestPassword123", "Filip Grujic");
    }

    public static void Setup()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DotEnv.Load();
        DatabaseHandler.LoadConnectionParams();
        if (!DatabaseHandler.VerifyConnection())
        {
            Console.WriteLine("Exiting, due to database connection not being made");
            Environment.Exit(1);
        }
    }

    public static void LoadData()
    {
        User.LoadUsers();
        Account.LoadAccounts();
    }
}