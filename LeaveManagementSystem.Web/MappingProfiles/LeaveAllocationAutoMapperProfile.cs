using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.MappingProfiles
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
