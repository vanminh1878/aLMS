using aLMS.Application.Common.Dtos;
using aLMS.Domain.GradeEntity;
using aLMS.Domain.SchoolEntity;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aLMS.Application.Common.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<School, SchoolDto>();
            CreateMap<CreateSchoolDto, School>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty)); // Thay Ignore() tạm thời
            CreateMap<UpdateSchoolDto, School>();
            CreateMap<Grade, GradeDto>();
            CreateMap<CreateGradeDto, Grade>();
            CreateMap<UpdateGradeDto, Grade>();
        }
    }
}