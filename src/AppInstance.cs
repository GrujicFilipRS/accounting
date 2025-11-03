namespace Accounting;

class Application
{
    public static void Run()
    {
        string acc1Sess = User.Register("fgrujic1", "CofiMafija123", "Filip Grujic");
        string acc2Sess = User.Register("fgrujic2", "CofiMafija123", "Filip Grujic");

        string addr1 = CheckingAccount.CreateAccount(acc1Sess);
        Account.Deposit(acc1Sess, addr1, 1000.0);

        string addr2 = CheckingAccount.CreateAccount(acc2Sess);

        Console.WriteLine(Account.GetBalanceFormatted(acc1Sess, addr1));
        Console.WriteLine(Account.GetBalanceFormatted(acc2Sess, addr2));
        Console.WriteLine();

        CheckingAccount.Transfer(acc1Sess, addr1, addr2, 250.0);

        Console.WriteLine(Account.GetBalanceFormatted(acc1Sess, addr1));
        Console.WriteLine(Account.GetBalanceFormatted(acc2Sess, addr2));
    }

    public static void Setup()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }
}