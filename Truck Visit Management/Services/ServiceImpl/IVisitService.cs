using Truck_Visit_Management.Dtos;

namespace Truck_Visit_Management.Services.ServiceImpl
{
    public interface IVisitService
    {
        Task<VisitRecordDto> CreateVisitAsync(VisitRecordDto visitRecord);
        Task<IEnumerable<VisitRecordDto>> GetVisitsAsync();
        Task UpdateVisitStatusAsync(int id, string status);
        Task<VisitRecordDto> GetVisitByIdAsync(int id);
    }

}
