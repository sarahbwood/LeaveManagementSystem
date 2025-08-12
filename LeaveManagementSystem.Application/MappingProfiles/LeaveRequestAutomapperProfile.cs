namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveRequestAutomapperProfile : Profile
    {
        public LeaveRequestAutomapperProfile()
        {
            CreateMap<LeaveRequestCreateVM, LeaveRequest>();
        }

    }
}
