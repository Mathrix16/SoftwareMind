using Microsoft.EntityFrameworkCore;
using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public class DesksRepository : IDesksRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DesksRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Desk>> GetDesksAsync(Guid locationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Desks.Where(d => d.LocationId == locationId).ToListAsync(cancellationToken);
    }

    public async Task<Desk?> GetDeskById(Guid locationId, Guid deskId, CancellationToken cancellationToken)
    {
        return await _dbContext.Desks.FirstOrDefaultAsync(d => d.LocationId == locationId && d.Id == deskId,
            cancellationToken);
    }

    public async Task<Guid> AddDesk(Desk desk, CancellationToken cancellationToken)
    {
        await _dbContext.Desks.AddAsync(desk, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return desk.Id;
    }

    public async Task DeleteDesk(Guid locationId, Guid id, CancellationToken cancellationToken)
    {
        var desk = new Desk {Id = id};
        _dbContext.Desks.Attach(desk);
        _dbContext.Desks.Remove(desk);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDesk(Desk inputDesk, CancellationToken cancellationToken)
    {
        var desk = await _dbContext.Desks.FirstAsync(p => p.LocationId == inputDesk.LocationId && p.Id == inputDesk.Id,
            cancellationToken);
        
        desk.IsAvailable = inputDesk.IsAvailable;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeskExists(Guid locationId, Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Desks.AsNoTracking()
            .AnyAsync(p => p.Id == id && p.LocationId == locationId, cancellationToken);
    }

    public async Task<bool> IsDeskAvailable(Guid locationId, Guid id, CancellationToken cancellationToken)
    {
        var desk = await _dbContext.Desks.AsNoTracking()
            .FirstAsync(p => p.Id == id && p.LocationId == locationId, cancellationToken);
        
        return desk.IsAvailable;
    }

    public async Task<bool> DeskHasAnyActiveReservations(Guid locationId, Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations.AsNoTracking()
            .AnyAsync(r => r.LocationId == locationId && r.DeskId == id && r.EndDate >= DateTime.Now, cancellationToken);
    }
}