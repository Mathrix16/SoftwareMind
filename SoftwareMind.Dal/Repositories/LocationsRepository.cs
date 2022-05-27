using Microsoft.EntityFrameworkCore;
using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LocationsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Locations.ToListAsync(cancellationToken);
    }

    public async Task<Guid> AddLocation(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return location.Id;
    }

    public async Task DeleteLocation(Guid id, CancellationToken cancellationToken)
    {
        var todo = new Location {Id = id};
        _dbContext.Locations.Attach(todo);
        _dbContext.Locations.Remove(todo);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> LocationExists(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations.AsNoTracking().AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> LocationHasAnyDesks(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Desks.AsNoTracking().AnyAsync(p => p.LocationId == id, cancellationToken);
    }
}