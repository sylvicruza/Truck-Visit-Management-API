using AutoMapper;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Exceptions;
using Truck_Visit_Management.Repositories;
using Truck_Visit_Management.Services.ServiceImpl;

namespace Truck_Visit_Management.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IMapper _mapper;

        public VisitService(IVisitRepository visitRepository, IMapper mapper)
        {
            _visitRepository = visitRepository;
            _mapper = mapper;
        }

        public async Task<VisitRecordResponseDto> CreateVisitAsync(VisitRecordRequestDto visitRecordRequest)
        {
            var validationErrors = visitRecordRequest.Validate();
            if (validationErrors.Any())
            {
                throw new BadRequestException($"Validation errors: {string.Join(", ", validationErrors)}");
            }

            // Set additional properties like CreatedTime and CreatedBy
            visitRecordRequest.CreatedTime = DateTime.UtcNow; // Assuming UTC time
            visitRecordRequest.UpdatedTime = visitRecordRequest.CreatedTime;
            visitRecordRequest.UpdatedBy = visitRecordRequest.CreatedBy;

            var entity = _mapper.Map<VisitRecordEntity>(visitRecordRequest);
            var createdEntity = await _visitRepository.CreateVisitAsync(entity);
            return _mapper.Map<VisitRecordResponseDto>(createdEntity);
        }

        public async Task<IEnumerable<VisitRecordResponseDto>> GetVisitsAsync()
        {
            var entities = await _visitRepository.GetVisitsAsync();
            return _mapper.Map<IEnumerable<VisitRecordResponseDto>>(entities);
        }

        public async Task UpdateVisitStatusAsync(int id, string status, string updatedBy)
        {
            var entity = await _visitRepository.GetVisitByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"Visit with id {id} not found");
            }

            entity.Status = status;
            entity.UpdatedTime = DateTime.UtcNow; // Assuming UTC time
            entity.UpdatedBy = updatedBy;

            await _visitRepository.UpdateVisitStatusAsync(entity);
        }
        

        public async Task<VisitRecordResponseDto> GetVisitByIdAsync(int id)
        {
            var entity = await _visitRepository.GetVisitByIdAsync(id);
            return _mapper.Map<VisitRecordResponseDto>(entity);
        }
    }

}
