using dotenv.net;

namespace Accounting;

class Application
{
    public static void Run()
    {
        
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