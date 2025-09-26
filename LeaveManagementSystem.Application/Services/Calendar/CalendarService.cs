using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace LeaveManagementSystem.Application.Services.Calendar
{
    public class CalendarService() : ICalendarService
    {
        public async Task<String> CreateCalendarEvent(LeaveRequestReviewVM approvedLeave)
        {
            var calendarEvent = new CalendarEvent
            {
                Summary = $"Leave - {approvedLeave.Employee.FullName}", 
                Description = $"{approvedLeave.LeaveType}", 
                Start = new CalDateTime(approvedLeave.StartDate, new TimeOnly(0, 0, 0), "Africa/Johannesburg"),
                End = new CalDateTime(approvedLeave.EndDate, new TimeOnly(23, 59, 59), "Africa/Johannesburg"),
            };

            var calendar = new Ical.Net.Calendar();
            calendar.Events.Add(calendarEvent);
            calendar.AddTimeZone(new VTimeZone("Africa/Johannesburg")); 
            
            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);
            
            return serializedCalendar;
        }

        public async Task<MemoryStream> WriteEventToStream(string serializedCalendar)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(serializedCalendar);
            streamWriter.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
