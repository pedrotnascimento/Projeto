using Repository.DataAccessLayer;
using Repository.Models;

namespace Repository.RepositoryInterfaces
{
    public interface ITimeAllocationRepository
    {
        void Create(TimeAllocationDAL timeAlocation);
        TimeAllocationDAL? GetByUserIdAndDate(int userId, DateTime dateTime);
        void Update(TimeAllocationDAL timeAlocation);
        List<TimeAllocationDAL> QueryByUserIdAndMonth(int id, DateTime month);
    }
}
