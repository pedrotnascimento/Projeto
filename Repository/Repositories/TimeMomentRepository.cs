using AutoMapper;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class TimeMomentRepository : ITimeMomentRepository
    {
        private readonly AppDatabaseContext context;
        private readonly IMapper mapper;

        public TimeMomentRepository(AppDatabaseContext context, AutoMapper.IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(TimeMomentDAL timeMoment)
        {
            var instance = mapper.Map<TimeMomentDAL, TimeMoment>(timeMoment);
            this.context.TimeMoments.Add(instance);
            context.SaveChanges();
        }

        public IList<TimeMomentDAL> QueryByUserIdAndDate(int userId, DateTime date)
        {

            var query = this.context.TimeMoments.Where(x => x.UserId == userId
            && x.DateTime.Date == date.Date)
                .ToList();
            if (query == null)
            {
                return new List<TimeMomentDAL>();
            }
            var queryReturn = mapper.Map<List<TimeMoment>, List<TimeMomentDAL>>(query);
            return queryReturn;
        }

        public List<TimeMomentDAL> QueryByUserIdAndMonth(int id, DateTime month)
        {
            var query = this.context.TimeMoments.Where(x => x.UserId == id &&
                x.DateTime.Month == month.Month &&
                x.DateTime.Year == month.Year)
                .ToList();

            if (query == null)
            {
                return new List<TimeMomentDAL>();
            }

            var queryReturn = mapper.Map<List<TimeMoment>, List<TimeMomentDAL>>(query);
            return queryReturn;
        }
    }
}
