using LeaveManagementSystem.Application.Models.Employees;
using LeaveManagementSystem.Application.Services.Calendar;
using LeaveManagementSystem.Application.Services.Departments;
using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.Employees;
using LeaveManagementSystem.Application.Services.Users;
using LeaveManagementSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public partial class LeaveRequestsService(LeaveManagementSystemWebContext _context, IMapper _mapper, IUserService _userService, IEmployeesService _employeesService, IEmailSender _emailSender, IDepartmentsService _departmentsService, IWebHostEnvironment _webHostEnvironment, ICalendarService _calendarService, IEmailService _emailService) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled; // EF Core will track the change

            // save the changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model); // map data from view model to data model

            // get the id of the currently logged in user
            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;
    
            // set leave request status to pending - default
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending; // used enum

            // save the leave request to the database
            _context.LeaveRequests.Add(leaveRequest);

            // save the changes to the database
            await _context.SaveChangesAsync();

            // send email to the manager regarding the new leave request
            await EmailLeaveRequestToManager(user.DepartmentId, leaveRequest.Id);
        }

        public async Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                 .Include(q => q.LeaveType)
                 .ToListAsync();

            var model = new EmployeeLeaveRequestListVM
            {
                TotalRequests = leaveRequests.Count, // this 'count' is a property, not a method
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
                LeaveRequests = leaveRequests
                    .Select(
                        q => new LeaveRequestReadOnlyVM
                        {
                            Id = q.Id,
                            StartDate = q.StartDate,
                            EndDate = q.EndDate,
                            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
                            LeaveType = q.LeaveType.LeaveTypeName, // requires a join with LeaveType - must use Include in the query
                            LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId // can't be done as part of the Linq query, must be done after the query as there is no support for enums in EF Core
                        }
                    ).ToList()
            };

            return model;
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser(); // get the currently logged in user
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType) // include LeaveType to get the leave type name
                .Where(q => q.EmployeeId == user.Id) // like a select * from LeaveRequests where EmployeeId = user.Id
                .ToListAsync();

            var model = leaveRequests.Select(q =>
                new LeaveRequestReadOnlyVM
                {
                    Id = q.Id,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
                    LeaveType = q.LeaveType.LeaveTypeName, // requires a join with LeaveType - must use Include in the query
                    LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId // can't be done as part of the Linq query, must be done after the query as there is no support for enums in EF Core
                }
            ).ToList(); // select into new LeaveRequestReadOnlyVM objects

            return model;
        }

        public async Task<LeaveRequestReviewVM> GetLeaveRequestForReview(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Include(q => q.Employee) // need to use First instead of Find because we need to include the Leave Type and Employee navigation properties
                .FirstAsync(q => q.Id == leaveRequestId); // first will throw an exception if no record is found - instead of returning null

            var model = new LeaveRequestReviewVM
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                LeaveType = leaveRequest.LeaveType.LeaveTypeName,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.Employee.Id,
                    FirstName = leaveRequest.Employee.FirstName,
                    LastName = leaveRequest.Employee.LastName,
                    Email = leaveRequest.Employee.Email,
                }
            };

            return model;
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool isApproved)
        {
            var user = await _userService.GetLoggedInUser(); // get the currently logged in user
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            // add reviewer details
            leaveRequest.ReviewerId = user.Id;

            // change request status
            leaveRequest.LeaveRequestStatusId = isApproved
                ? (int)LeaveRequestStatusEnum.Approved
                : (int)LeaveRequestStatusEnum.Declined;

            // save the changes to the database
            await _context.SaveChangesAsync();

            // send email to the employee regarding the status of their leave request
            await EmailLeaveRequestStatusToEmployee(leaveRequestId);

            if (isApproved)
            {
                await NotifyManagementAboutLeave(leaveRequestId);

                // TODO
                // add to calendar view
            }
        }

        private async Task EmailLeaveRequestToManager(int? departmentId, int leaveRequestId)
        {
            //get manager - if employee is not a manager themselves
            var manager = await _departmentsService.GetDepartmentManager(departmentId);

            // get leave request
            var leaveRequest = await GetLeaveRequestForReview(leaveRequestId); // LeaveRequestReviewVM

            // get email template
            var emailTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "leaverequest_email_layout.html");
            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);
            var messageBody = emailTemplate
               .Replace("{FullName}", $"{manager.FirstName} {manager.LastName}")
               .Replace("{EmployeeName}", leaveRequest.Employee.FullName)
               .Replace("{EmployeeEmail}", leaveRequest.Employee.Email)
               .Replace("{LeaveType}", leaveRequest.LeaveType)
               .Replace("{StartDate}", leaveRequest.StartDate.ToString())
               .Replace("{EndDate}", leaveRequest.EndDate.ToString())
               .Replace("{NumberOfDays}", leaveRequest.NumberOfDays.ToString())
               .Replace("{AdditionalComments}", leaveRequest.RequestComments);
               

            await _emailSender.SendEmailAsync(manager.Email, "Review Leave Request", messageBody);
        }

        private async Task EmailLeaveRequestStatusToEmployee(int leaveRequestId)
        {
            // get leave request
            var leaveRequest = await GetLeaveRequestForReview(leaveRequestId); // LeaveRequestReviewVM

            // get email template
            var emailTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "leaverequest_status_email_layout.html");
            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);
            var messageBody = emailTemplate
               .Replace("{Status}", leaveRequest.LeaveRequestStatus.ToString().ToLower())
               .Replace("{FullName}", leaveRequest.Employee.FullName)
               .Replace("{LeaveType}", leaveRequest.LeaveType)
               .Replace("{StartDate}", leaveRequest.StartDate.ToString())
               .Replace("{EndDate}", leaveRequest.EndDate.ToString())
               .Replace("{NumberOfDays}", leaveRequest.NumberOfDays.ToString());

            await _emailSender.SendEmailAsync(leaveRequest.Employee.Email, "Leave Request Status", messageBody);
        }

        private async Task NotifyManagementAboutLeave(int leaveId)
        {
            var approvedLeave = await GetLeaveRequestForReview(leaveId);
            var leaveEvent = await _calendarService.CreateCalendarEvent(approvedLeave);
            var leaveEventStream = await _calendarService.WriteEventToStream(leaveEvent);

            // get email template
            var emailTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "leave_notification_template.html");
            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);
            var messageBody = emailTemplate
               .Replace("{EmployeeName}", approvedLeave.Employee.FullName)
               .Replace("{StartDate}", approvedLeave.StartDate.ToString())
               .Replace("{EndDate}", approvedLeave.EndDate.ToString());

            // TODO - change to manager mailing list
            await _emailService.EmailManagers(approvedLeave.Employee.Email, messageBody, leaveEventStream);

        }
    }
}

