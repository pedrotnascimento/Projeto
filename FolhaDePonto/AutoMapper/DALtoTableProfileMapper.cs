using AutoMapper;
using Repository.DataAccessLayer;
using Repository.Models;

namespace Application.AutoMapper
{
    public class DALtoTableProfileMapper : Profile
    {

        public DALtoTableProfileMapper()
        {
            CreateMap<TimeMomentDAL, TimeMoment>().ReverseMap();
            CreateMap<TimeAllocationDAL, TimeAllocation>().ReverseMap();
        }

    }
}
