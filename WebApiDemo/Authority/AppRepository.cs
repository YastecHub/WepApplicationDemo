namespace WebApiDemo.Authority
{
    public class AppRepository
    {
        private static List<Application> _applications = new List<Application>()
        {
            new Application()
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6",
                Secret = "5d2c7f3a-b5b2-4e5d-9391-0d8aefb8c87f",
                Scopes = "read,write,delete"
            }
        };



        public static Application? GetApplicationByClientId(string clientId)
        {
            return _applications.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
