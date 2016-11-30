namespace Services
{
    public interface IEmailSender
    {
        void SendEmail(string recipient, string message);
    }
}
