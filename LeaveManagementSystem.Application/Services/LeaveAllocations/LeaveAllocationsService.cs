using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;


namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    public class LeaveAllocationsService(LeaveManagementSystemWebContext _context, IUserService _userService, IMapper _mapper, IPeriodsService _periodsService) : ILeaveAllocationsService
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
            var currentPeriod = await _periodsService.GetCurrentPeriod();
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
                    NumberOfDays = (int)Math.Ceiling(accrualRate * monthsLeftInYear)
                };

                // save the allocation to the database
                // tracking the allocation
                _context.LeaveAllocations.Add(leaveAllocation);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);
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

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);

            var allocationVM = _mapper.Map<LeaveAllocationEditVM>(allocation);
            return allocationVM;
        }

        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var currentPeriod = await _periodsService.GetCurrentPeriod();
            var currentAllocation = await _context.LeaveAllocations
                .FirstAsync(q =>
                    q.EmployeeId == employeeId
                    && q.PeriodId == currentPeriod.Id
                    && q.LeaveTypeId == leaveTypeId
                );

            return currentAllocation;
        }

        public async Task EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM)
        {
            await _context.LeaveAllocations
                .Where(q => q.Id == leaveAllocationEditVM.Id) // filter responsibily children!
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.NumberOfDays, leaveAllocationEditVM.NumberOfDays));
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userService.GetEmployees();
            var employees = _mapper.Map<List<EmployeeListVM>>(users);

            return employees;
        }
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var period = await _periodsService.GetCurrentPeriod();
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // Include related entities if necessary, e.g., LeaveType, Period - acts as a join 
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.Id == period.Id)
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
