using SoftwareMind.Core.Dtos;
using SoftwareMind.Core.Exceptions;
using SoftwareMind.Dal.Entities;
using SoftwareMind.Dal.Repositories;

namespace SoftwareMind.Core.Services;

public class DesksService : IDesksService
{
    private readonly IDesksRepository _desksRepository;
    private readonly ILocationsRepository _locationsRepository;

    public DesksService(IDesksRepository desksRepository, ILocationsRepository locationsRepository)
    {
        _desksRepository = desksRepository;
        _locationsRepository = locationsRepository;
    }

    public async Task<IEnumerable<DeskDto>> GetDesksAsync(Guid locationId, CancellationToken cancellationToken)
    {
        if (!await _locationsRepository.LocationExists(locationId, cancellationToken))
            throw new NotFoundException("Location with given id was not found");

        var results = await _desksRepository.GetDesksAsync(locationId, cancellationToken);

        return results.Select(d => new DeskDto(d));
    }

    public async Task<Guid> AddDesk(NewDesk newDesk, CancellationToken cancellationToken)
    {
        if (!await _locationsRepository.LocationExists((Guid) newDesk.LocationId!, cancellationToken))
            throw new NotFoundException("Location with given id was not found");

        var desk = new Desk
        {
            IsAvailable = newDesk.IsAvailable,
            LocationId = (Guid) newDesk.LocationId!,
            Name = newDesk.Name
        };
        return await _desksRepository.AddDesk(desk, cancellationToken);
    }

    public async Task DeleteDesk(Guid locationId, Guid id, CancellationToken cancellationToken)
    {
        if (!await _desksRepository.DeskExists(locationId, id, cancellationToken))
            throw new NotFoundException("Desk with given id was not found");

        if (await _desksRepository.DeskHasAnyActiveReservations(locationId, id, cancellationToken))
            throw new BadRequestException("Can not remove desk that has active reservations");

        await _desksRepository.DeleteDesk(locationId, id, cancellationToken);
    }

    public async Task UpdateDesk(UpdatedDesk updatedDesk, CancellationToken cancellationToken)
    {
        var locationId = (Guid) updatedDesk.LocationId!;
        var id = (Guid) updatedDesk.Id!;

        if (!await _desksRepository.DeskExists(locationId, id, cancellationToken))
            throw new NotFoundException("Desk with given id was not found");

        if (await _desksRepository.DeskHasAnyActiveReservations(locationId, id, cancellationToken))
            throw new BadRequestException("Can not update desk that has active reservations");

        var desk = new Desk
        {
            Id = id,
            LocationId = locationId,
            IsAvailable = updatedDesk.IsAvailable
        };

        await _desksRepository.UpdateDesk(desk, cancellationToken);
    }
}