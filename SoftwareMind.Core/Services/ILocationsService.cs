using SoftwareMind.Core.Dtos;

namespace SoftwareMind.Core.Services;

public interface ILocationsService
{
    Task<IEnumerable<LocationDto>> GetLocationsAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddLocation(NewLocation newLocation, CancellationToken cancellationToken);

    Task DeleteLocation(Guid id, CancellationToken cancellationToken);
}