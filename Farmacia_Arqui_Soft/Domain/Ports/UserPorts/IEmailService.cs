namespace Farmacia_Arqui_Soft.Domain.Ports.UserPorts
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
