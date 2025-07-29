
using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(LeaveManagementSystemWebContext _context, IMapper _mapper, UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled; // EF Core will track the change

            // reallocate the leave days back to the employee's leave allocation
            var daysToReallocate = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            var leaveAllocation = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == leaveRequest.EmployeeId && q.LeaveTypeId == leaveRequest.LeaveTypeId);

            leaveAllocation.NumberOfDays += daysToReallocate;

            // save the changes to the database
            await _context.SaveChangesAsync();

        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            // map data from view model to data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            // get the id of the currently logged in user
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;

            // set leave request status to pending - default
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending; // used enum

            // save the leave request to the database
            _context.LeaveRequests.Add(leaveRequest);

            // deduct allocated leave days from the employee's leave allocation
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);

            allocationToDeduct.NumberOfDays -= numberOfDays;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DaysExceedAllocation(LeaveRequestCreateVM model)
        {
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var numberOfDaysAllocated = await _context.LeaveAllocations.FirstAsync(
                q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);

            return numberOfDaysAllocated.NumberOfDays < numberOfDays;
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
                        q => new LeaveRequestReadOnlyVM {
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
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType) // include LeaveType to get the leave type name
                .Where(q => q.EmployeeId == user.Id) // like a select * from LeaveRequests where EmployeeId = user.Id
                .ToListAsync();

            var model = leaveRequests.Select(q => 
                new LeaveRequestReadOnlyVM { 
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

        public Task ReviewLeaveRequest(LeaveRequestReviewVM model)
        {
            throw new NotImplementedException();
        }
    }
}

   