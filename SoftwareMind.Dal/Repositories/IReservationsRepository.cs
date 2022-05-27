using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public interface IReservationsRepository
{
    Task<IEnumerable<Reservation>> GetReservationsAsync(Guid locationId, Guid deskId,
        CancellationToken cancellationToken);

    Task<Reservation?> GetReservationById(Guid locationId, Guid deskId, Guid id, CancellationToken cancellationToken);
    
    Task<Guid> ReserveDesk(Reservation reservation, CancellationToken cancellationToken);

    Task ChangeReservation(Reservation newReservation, Reservation oldReservation, CancellationToken cancellationToken);
    
}