using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.Services.Periods
{
    public interface IPeriodsService
    {
        Task<T?> Get<T>(int id) where T : class;
        Task<List<PeriodReadOnlyVM>> GetAll();
        Task Remove(int id);
        Task<Period> GetCurrentPeriod();
        Task<bool> PeriodExists(int id);
    }
}
