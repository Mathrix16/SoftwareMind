using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public interface ILocationsRepository
{
    Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddLocation(Location location, CancellationToken cancellationToken);

    Task DeleteLocation(Guid id, CancellationToken cancellationToken);
    
    Task<bool> LocationExists(Guid id, CancellationToken cancellationToken);
    
    Task<bool> LocationHasAnyDesks(Guid id, CancellationToken cancellationToken);
}