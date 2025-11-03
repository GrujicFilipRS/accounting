namespace Accounting;

class Entrypoint
{
    public static void Main(string[] args)
    {
        Application.Setup();
        Application.LoadData();
        Application.Run();
    }
}