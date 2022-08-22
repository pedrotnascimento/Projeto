using AutoMapper;
using BusinessRule.Domain;
using Common;
using Application.DTO;
using Repository.DataAccessLayer;

namespace Application.AutoMapper
{
    public class BRtoDALProfileMapper : Profile
    {

        public BRtoDALProfileMapper()
        {
            CreateMap<TimeMomentBR, TimeMomentDAL>().ReverseMap();
            CreateMap<TimeAllocationBR, TimeAllocationDAL>().ReverseMap();
            CreateMap<UserBR, UserDAL>().ReverseMap();
        }

    }
}
