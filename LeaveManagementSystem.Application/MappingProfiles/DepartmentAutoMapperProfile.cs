using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class DepartmentAutoMapperProfile : Profile
    {
        public DepartmentAutoMapperProfile()
        {
            CreateMap<Department, DepartmentReadOnlyVM>().ReverseMap();
            CreateMap<Department, DepartmentCreateVM>().ReverseMap();
        }
    }
}
