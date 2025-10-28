namespace Accounting
{
    class Entrypoint
    {
        public static void Main(string[] args)
        {
            string username = "ficfiric";
            string password = "test123";
            string fullName = "Filip Grujic";

            string sessionId = User.Register(username, password, fullName);
            sessionId = User.Login(username, password);
        }
    }
}