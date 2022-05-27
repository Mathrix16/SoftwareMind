using SoftwareMind.Core.Dtos;
using SoftwareMind.Core.Exceptions;
using SoftwareMind.Dal.Entities;
using SoftwareMind.Dal.Repositories;

namespace SoftwareMind.Core.Services;

public class LocationsService : ILocationsService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IDesksRepository _desksRepository;
    

    public LocationsService(ILocationsRepository locationsRepository, IDesksRepository desksRepository)
    {
        _locationsRepository = locationsRepository;
        _desksRepository = desksRepository;
    }

    public async Task<IEnumerable<LocationDto>> GetLocationsAsync(CancellationToken cancellationToken)
    {
        var results = await _locationsRepository.GetLocationsAsync(cancellationToken);
        return results.Select(x => new LocationDto(x));
    }

    public async Task<Guid> AddLocation(NewLocation newLocation, CancellationToken cancellationToken)
    {
        var location = new Location
        {
            Name = newLocation.Name
        };
        return await _locationsRepository.AddLocation(location, cancellationToken);
    }
    
    public async Task DeleteLocation(Guid id, CancellationToken cancellationToken)
    {
        if (!await _locationsRepository.LocationExists(id, cancellationToken))
            throw new NotFoundException("Location with given id was not found");
        
        if (await _locationsRepository.LocationHasAnyDesks(id, cancellationToken))
            throw new BadRequestException("Can not remove location that has desks");
        
        await _locationsRepository.DeleteLocation(id, cancellationToken);
    }
}