namespace CityInfoAPI.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            Console.WriteLine(_mailFrom + _mailTo);
            Console.WriteLine(subject);
            Console.WriteLine(message);
        }
    }
}
