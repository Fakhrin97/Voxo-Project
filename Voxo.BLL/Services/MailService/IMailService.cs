using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxo.BLL.Data;

namespace Voxo.BLL.Services.MailService
{
    public interface IMailService
    {
        Task SendEmailAsync(RequestEmail requestEmail);
    }
}
