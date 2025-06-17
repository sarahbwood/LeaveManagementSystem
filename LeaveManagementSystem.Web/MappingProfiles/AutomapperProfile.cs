using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();

            // only for the properties that are different/different names
            // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LeaveTypeName)

            CreateMap<LeaveTypeCreateVM, LeaveType>();

            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap(); // both ways mapping
        }

    }
}
