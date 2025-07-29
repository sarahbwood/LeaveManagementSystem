using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveRequestAutomapperProfile : Profile
    {
        public LeaveRequestAutomapperProfile()
        {
            CreateMap<LeaveRequestCreateVM, LeaveRequest>();
        }

    }
}
