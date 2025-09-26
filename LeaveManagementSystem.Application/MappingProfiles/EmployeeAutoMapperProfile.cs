using LeaveManagementSystem.Application.Models.Employees;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class EmployeeAutoMapperProfile : Profile
    {
        public EmployeeAutoMapperProfile()
        {
            CreateMap<ApplicationUser, EmployeeListVM>();
        }

    }
}
