using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using AutoMapper;


namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(LeaveManagementSystemWebContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // Logic to allocate leave to the employee

            // Get all leave types
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            // Get the current period based on the year
            // Calculate leave based on number of months left in the year
            var currentYear = DateTime.Now.Year;
            var currentPeriod = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentYear);
            var monthsLeftInYear = currentPeriod.EndDate.Month - DateTime.Now.Month;

            // for each leave type, allocate leave for the employee
            foreach (var leaveType in leaveTypes)
            {
                var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.LeaveTypeId,
                    PeriodId = currentPeriod.Id,
                    NumberOfDays = (int) Math.Ceiling(accrualRate * monthsLeftInYear)
                };

                // save the allocation to the database
                // tracking the allocation
                _context.LeaveAllocations.Add(leaveAllocation);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User) 
                : await _userManager.FindByIdAsync(userId);
            var allocations = await GetAllocations(user.Id);
            var allocationVMList = _mapper.Map<List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVM = new EmployeeAllocationVM
            {
                Id = user.Id,
                FirstName = user.FirstName, 
                LastName = user.LastName, 
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                LeaveAllocations = allocationVMList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count
            };

            return employeeVM;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            var employees = _mapper.Map<List<EmployeeListVM>>(users.ToList());

            return employees;
        }
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var currentDate = DateTime.Now;
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // Include related entities if necessary, e.g., LeaveType, Period - acts as a join 
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync();

            return leaveAllocations;
        }

        private async Task<bool> AllocationExists(string userId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations
                .AnyAsync(q => 
                q.EmployeeId == userId 
                && q.PeriodId == periodId 
                && q.LeaveTypeId == leaveTypeId
            );

            return exists;
        }
    }
}
