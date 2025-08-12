using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.Services.Periods
{
    public interface IPeriodsService
    {
        Task Create(PeriodCreateVM model);
        Task Edit(PeriodEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<PeriodReadOnlyVM>> GetAll();
        Task Remove(int id);
        Task<Period> GetCurrentPeriod();
    }
}
