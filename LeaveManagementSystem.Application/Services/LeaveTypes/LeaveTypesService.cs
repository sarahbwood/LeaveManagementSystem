using LeaveManagementSystem.Application.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeaveManagementSystem.Application.Services.LeaveTypes
{

    public class LeaveTypesService(LeaveManagementSystemWebContext _context, IMapper _mapper, ILogger<LeaveTypesService> _logger) : ILeaveTypesService // primary constructor
    {
        // Database access methods for LeaveTypes

        // Method to get all leave types
        public async Task<List<LeaveTypeReadOnlyVM>> GetAll()
        {
            var data = await _context.LeaveTypes.ToListAsync(); // SELECT * FROM LeaveTypes
            return _mapper.Map<List<LeaveTypeReadOnlyVM>>(data); // Convert to view model using AutoMapper


            // convert the data model to a view model - Manually
            //var viewData = data.Select(q => new IndexVM
            //{
            //    LeaveTypeId = q.LeaveTypeId,
            //    LeaveTypeName = q.LeaveTypeName,
            //    NumberOfDays = q.NumberOfDays,
            //});
        }

        // Method to get specific leave type by ID
        // Generic T is used to allow flexibility in the return type - e.g., LeaveTypeEditVM or LeaveTypeDetailsVM
        public async Task<T?> Get<T>(int id) where T : class
        {
            // parameterized query to prevent SQL injection attacks
            // like "SELECT * FROM LeaveTypes WHERE LeaveTypeId = id"
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.LeaveTypeId == id);

            if (leaveType == null)
            {
                return null;
            }

            return _mapper.Map<T>(leaveType); // Convert to view model using AutoMapper
        }

        // Method to delete a leave type by ID
        public async Task Remove(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
                await _context.SaveChangesAsync();
            }
        }

        // Method to update a leave type
        public async Task Edit(LeaveTypeEditVM model)
        {
            var leaveType = _mapper.Map<LeaveType>(model);
            _context.LeaveTypes.Update(leaveType);
            await _context.SaveChangesAsync();

        }

        public async Task Create(LeaveTypeCreateVM model)
        {
            _logger.LogInformation("Creating new Leave Type: {leaveTypeName} - {numberOfDays} day(s)", model.LeaveTypeName, model.NumberOfDays);
            var leaveType = _mapper.Map<LeaveType>(model);
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
        }

        public bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.LeaveTypeId == id);
        }

        public async Task<bool> CheckIfLeaveTypeNameExists(string leaveTypeName)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.LeaveTypeName.ToLower().Equals(leaveTypeName.ToLower()));

            // could do this, but it is not recommended to use string comparison in LINQ queries
            // LINQ queries have to be translated to SQL, and SQL does not support StringComparison.InvariantCultureIgnoreCase
            // .Equals(leaveTypeName, StringComparison.InvariantCultureIgnoreCase
        }

        public async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.LeaveTypeName.ToLower()
                .Equals(leaveTypeEdit.LeaveTypeName.ToLower())
                && q.LeaveTypeId != leaveTypeEdit.LeaveTypeId
            );
        }

        public async Task<bool> DaysExceedMaximum(int leaveTypeId, int numDays)
        {
            var leaveType = await _context.LeaveTypes
                .FindAsync(leaveTypeId);
            return leaveType.NumberOfDays < numDays;
        }
    }
}
