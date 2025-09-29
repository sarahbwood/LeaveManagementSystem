using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Application.Services.Users
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetEmployees();
        Task<ApplicationUser> GetLoggedInUser();
        Task<ApplicationUser> GetUserById(string id);
        Task<bool> IsAdmin(string userId);
        Task<List<ApplicationUser>> GetManagers();
    }
}