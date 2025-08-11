
using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(LeaveManagementSystemWebContext _context, IMapper _mapper, IUserService _userService, ILeaveAllocationsService _leaveAllocationsService) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled; // EF Core will track the change

            // reallocate the leave days back to the employee's leave allocation
            await UpdateAllocationDays(leaveRequest, false); // pass false to add the days back to the allocation

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

            // deduct allocated leave days from the employee's leave allocation
            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DaysExceedAllocation(LeaveRequestCreateVM model)
        {
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var user = await _userService.GetLoggedInUser();

            var numberOfDaysAllocated = await _leaveAllocationsService.GetCurrentAllocation(model.LeaveTypeId, user.Id);

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
            var user = await _userService.GetLoggedInUser(); // get the currently logged in user
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

            // if declined , reallocate the leave days back to the employee's leave allocation
            if (!isApproved)
            {
                await UpdateAllocationDays(leaveRequest, false); // pass false to add the days back to the allocation
            }

            // save the changes to the database
            await _context.SaveChangesAsync();
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationsService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.NumberOfDays -= numberOfDays;
            }
            else
            {
                allocation.NumberOfDays += numberOfDays;
            }

            _context.Entry(allocation).State = EntityState.Modified; // mark the allocation as modified
        }

        private int CalculateDays(DateOnly startDate, DateOnly endDate)
        {
            return endDate.DayNumber - startDate.DayNumber;
        }
    }
}

   