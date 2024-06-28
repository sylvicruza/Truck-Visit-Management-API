using AutoMapper;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
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

        public async Task<VisitRecordDto> CreateVisitAsync(VisitRecordDto visitRecord)
        {
            var entity = _mapper.Map<VisitRecordEntity>(visitRecord);
            var createdEntity = await _visitRepository.CreateVisitAsync(entity);
            return _mapper.Map<VisitRecordDto>(createdEntity);
        }

        public async Task<IEnumerable<VisitRecordDto>> GetVisitsAsync()
        {
            var entities = await _visitRepository.GetVisitsAsync();
            return _mapper.Map<IEnumerable<VisitRecordDto>>(entities);
        }

        public async Task UpdateVisitStatusAsync(int id, string status)
        {
            await _visitRepository.UpdateVisitStatusAsync(id, status);
        }

        public async Task<VisitRecordDto> GetVisitByIdAsync(int id)
        {
            var entity = await _visitRepository.GetVisitByIdAsync(id);
            return _mapper.Map<VisitRecordDto>(entity);
        }
    }

}
