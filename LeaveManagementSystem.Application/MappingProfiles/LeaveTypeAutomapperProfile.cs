using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveTypeAutomapperProfile : Profile
    {
        public LeaveTypeAutomapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();

            // only for the properties that are different/different names
            // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LeaveTypeName)

            CreateMap<LeaveTypeCreateVM, LeaveType>();

            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap(); // both ways mapping
        }

    }
}
