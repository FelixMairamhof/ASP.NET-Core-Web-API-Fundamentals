namespace CityInfoAPI.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = String.Empty;
        private string _mailFrom = String.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine(_mailFrom + _mailTo);
            Console.WriteLine(subject);
            Console.WriteLine(message);
        }
    }
}
