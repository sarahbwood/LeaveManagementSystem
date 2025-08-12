using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Models.Periods;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveAllocationAutoMapperProfile : Profile
    {
        public LeaveAllocationAutoMapperProfile()
        {
            CreateMap<LeaveAllocation, LeaveAllocationVM>();
            CreateMap<LeaveAllocation, LeaveAllocationEditVM>();
            CreateMap<ApplicationUser, EmployeeListVM>();
            CreateMap<Period, PeriodReadOnlyVM>();
            CreateMap<Period, PeriodEditVM>().ReverseMap();
            CreateMap<Period, PeriodCreateVM>().ReverseMap();

        }

    }
}
