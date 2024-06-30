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
            _context.VisitRecordEntity.Add(visitRecord);
            await _context.SaveChangesAsync();
            return visitRecord;
        }

        public async Task<IEnumerable<VisitRecordEntity>> GetVisitsAsync()
        {
            return await _context.VisitRecordEntity.Include(v => v.Activities).Include(v => v.Driver).ToListAsync();
        }
        public async Task UpdateVisitStatusAsync(VisitRecordEntity visitRecord)
        {
            var visit = await _context.VisitRecordEntity.FindAsync(visitRecord.Id);
            if (visit == null)
            {
                throw new NotFoundException($"Visit record with id {visitRecord.Id} not found.");
            }

            visit.Status = visitRecord.Status;
            visit.UpdatedTime = DateTime.UtcNow;
            visit.UpdatedBy = visitRecord.UpdatedBy;

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
