using SoftwareMind.Core.Dtos;

namespace SoftwareMind.Core.Services;

public interface IReservationsService
{
    Task<IEnumerable<ReservationDto>> GetReservationsAsync(Guid locationId, Guid deskId,
        CancellationToken cancellationToken);

    Task<Guid> ReserveDesk(NewReservation newReservation, CancellationToken cancellationToken);

    Task ChangeReservation(UpdatedReservation updatedReservation, CancellationToken cancellationToken);
}