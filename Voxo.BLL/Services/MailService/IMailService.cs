
namespace Voxo.BLL.Services.MailService
{
    public interface IMailService
    {
        Task SendEmailAsync(RequestEmail requestEmail);
    }
}
