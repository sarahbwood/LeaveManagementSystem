using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Services.Email
{
    public interface IEmailService
    {
        Task EmailManagers(string email, string htmlMessage, MemoryStream calendarEvent);
    }
}
