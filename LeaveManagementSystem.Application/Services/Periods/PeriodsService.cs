using LeaveManagementSystem.Application.Models.Periods;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.Periods
{
    public class PeriodsService(LeaveManagementSystemWebContext _context, IMapper _mapper) : IPeriodsService
    {
        public async Task<List<PeriodReadOnlyVM>> GetAll()
        {
            // Fetch all periods from the database
            var periods = await _context.Periods.ToListAsync();
            return _mapper.Map<List<PeriodReadOnlyVM>>(periods);
        }

        public async Task<T?> Get<T>(int id) where T : class
        {
            // Fetch a specific period by ID
            var period = await _context.Periods.FirstOrDefaultAsync(p => p.Id == id);
            if (period == null)
            {
                return null;
            }
            return _mapper.Map<T>(period);
        }

        public async Task Remove(int id)
        {
            // Find the period by ID and remove it
            var period = await _context.Periods.FindAsync(id);
            if (period != null)
            {
                _context.Periods.Remove(period);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(PeriodEditVM model)
        {
            // Map the view model to the data model
            var period = _mapper.Map<Period>(model);
            _context.Periods.Update(period);
            await _context.SaveChangesAsync();
        }

        public async Task Create(PeriodCreateVM model)
        {
            // Map the view model to the data model
            var period = _mapper.Map<Period>(model);
            _context.Periods.Add(period);
            await _context.SaveChangesAsync();
        }

        public async Task<Period> GetCurrentPeriod()
        {
            var currentYear = DateTime.Now.Year;
            var currentPeriod = await _context.Periods
                .SingleAsync(q => q.EndDate.Year == currentYear);

            return currentPeriod;
        }
    }
}
