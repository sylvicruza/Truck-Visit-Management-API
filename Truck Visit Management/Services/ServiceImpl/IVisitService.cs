using Truck_Visit_Management.Dtos;

namespace Truck_Visit_Management.Services.ServiceImpl
{
    public interface IVisitService
    {
        Task<VisitRecordResponseDto> CreateVisitAsync(VisitRecordRequestDto visitRecordRequest);
        Task<IEnumerable<VisitRecordResponseDto>> GetVisitsAsync();
        Task UpdateVisitStatusAsync(int id, string status, string updatedBy);
        Task<VisitRecordResponseDto> GetVisitByIdAsync(int id);
    }

}
