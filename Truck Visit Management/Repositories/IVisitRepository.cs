using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Repositories
{
    public interface IVisitRepository
    {
        Task<VisitRecordEntity> CreateVisitAsync(VisitRecordEntity visitRecord);
        Task<IEnumerable<VisitRecordEntity>> GetVisitsAsync();
        Task UpdateVisitStatusAsync(VisitRecordEntity visitRecord);
        Task<VisitRecordEntity> GetVisitByIdAsync(int id);
    }

}
