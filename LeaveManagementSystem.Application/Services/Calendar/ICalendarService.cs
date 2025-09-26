using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.Application.Models.LeaveRequests;

namespace LeaveManagementSystem.Application.Services.Calendar
{
    public interface ICalendarService
    {
        Task<String> CreateCalendarEvent(LeaveRequestReviewVM approvedLeave);
        Task<MemoryStream> WriteEventToStream(string serializedCalendar);
    }
}
