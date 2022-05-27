using SoftwareMind.Core.Dtos;
using SoftwareMind.Core.Exceptions;
using SoftwareMind.Dal.Entities;
using SoftwareMind.Dal.Repositories;

namespace SoftwareMind.Core.Services;

public class ReservationsService : IReservationsService
{
    private readonly IDesksRepository _desksRepository;
    private readonly IReservationsRepository _reservationsRepository;

    public ReservationsService(IReservationsRepository reservationsRepository, IDesksRepository desksRepository)
    {
        _reservationsRepository = reservationsRepository;
        _desksRepository = desksRepository;
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsAsync(Guid locationId, Guid deskId,
        CancellationToken cancellationToken)
    {
        if (!await _desksRepository.DeskExists(locationId, deskId, cancellationToken))
            throw new NotFoundException("Desk with given id was not found");
        
        var result = await _reservationsRepository.GetReservationsAsync(locationId, deskId, cancellationToken);

        return result.Select(r => new ReservationDto(r));
    }

    public async Task<Guid> ReserveDesk(NewReservation newReservation, CancellationToken cancellationToken)
    {
        var locationId = (Guid) newReservation.LocationId!;
        var deskId = (Guid) newReservation.DeskId!;

        var desk = await _desksRepository.GetDeskById(locationId, deskId, cancellationToken);

        if(desk is null)
            throw new NotFoundException("Desk with given id was not found");

        if (!desk.IsAvailable)
            throw new BadRequestException("Given desk is not available");
        
        if (newReservation.StartDate < DateTime.Now || newReservation.EndDate < newReservation.StartDate)
            throw new BadRequestException("Given dates are incorrect");

        if (newReservation.StartDate.AddDays(7) < newReservation.EndDate)
            throw new BadRequestException("Can not make a reservation for longer than 7 days");

        var reservation = new Reservation
        {
            LocationId = locationId,
            DeskId = deskId,
            StartDate = newReservation.StartDate,
            EndDate = newReservation.EndDate,
            Surname = newReservation.Surname
        };

        return await _reservationsRepository.ReserveDesk(reservation, cancellationToken);
    }

    public async Task ChangeReservation(UpdatedReservation updatedReservation, CancellationToken cancellationToken)
    {
        var oldLocationId = (Guid) updatedReservation.OldLocationId!;
        var oldDeskId = (Guid) updatedReservation.OldDeskId!;
        var id = (Guid) updatedReservation.Id!;
        var newLocationId = updatedReservation.NewLocationId;
        var newDeskId = updatedReservation.NewDeskId;


        var oldReservation =
            await _reservationsRepository.GetReservationById(oldLocationId, oldDeskId, id, cancellationToken);

        if (oldReservation is null)
            throw new NotFoundException("Reservation with given id was not found");

        var desk = await _desksRepository.GetDeskById(newLocationId, newDeskId, cancellationToken);
        
        if (desk is null)
            throw new NotFoundException("Target desk with given id was not found");
        
        if (!desk.IsAvailable)
            throw new BadRequestException("Given desk is not available");

        if (updatedReservation.StartDate.AddDays(-1) < oldReservation.StartDate)
            throw new BadRequestException("Can not change reservation later than 24 hours before");

        if (updatedReservation.StartDate < DateTime.Now || updatedReservation.EndDate < updatedReservation.StartDate)
            throw new BadRequestException("Given dates are incorrect");

        if (updatedReservation.StartDate.AddDays(7) < updatedReservation.EndDate)
            throw new BadRequestException("Can not make a reservation for longer than 7 days");

        var newReservation = new Reservation
        {
            LocationId = newLocationId,
            DeskId = newDeskId,
            StartDate = updatedReservation.StartDate,
            EndDate = updatedReservation.EndDate
        };

        oldReservation = new Reservation
        {
            LocationId = oldLocationId,
            DeskId = oldDeskId,
            Id = id
        };

        await _reservationsRepository.ChangeReservation(newReservation, oldReservation, cancellationToken);
    }
}