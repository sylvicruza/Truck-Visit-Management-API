using Microsoft.EntityFrameworkCore;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Exceptions;

namespace Truck_Visit_Management.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly TruckVisitDbContext _context;

        public VisitRepository(TruckVisitDbContext context)
        {
            _context = context;
        }

        public async Task<VisitRecordEntity> CreateVisitAsync(VisitRecordEntity visitRecord)
        {
            visitRecord.CreatedTime = DateTime.UtcNow;
            _context.VisitRecordEntity.Add(visitRecord);
            await _context.SaveChangesAsync();
            return visitRecord;
        }

        public async Task<IEnumerable<VisitRecordEntity>> GetVisitsAsync()
        {
            return await _context.VisitRecordEntity.Include(v => v.Activities).Include(v => v.Driver).ToListAsync();
        }

        public async Task UpdateVisitStatusAsync(int id, string status)
        {
            var visit = await _context.VisitRecordEntity.FindAsync(id);
            if (visit == null)
            {
                throw new NotFoundException($"Visit record with id {id} not found.");
            }

            visit.Status = status;
            visit.UpdatedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<VisitRecordEntity> GetVisitByIdAsync(int id)
        {
            var visit = await _context.VisitRecordEntity.Include(v => v.Activities).Include(v => v.Driver).FirstOrDefaultAsync(v => v.Id == id);
            if (visit == null)
            {
                throw new NotFoundException($"Visit record with id {id} not found.");
            }
            return visit;
        }
    }

}
