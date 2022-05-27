using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using SoftwareMind.Core.Dtos;
using SoftwareMind.Core.Services;

namespace SoftwareMind.Api.Controllers;

[ApiController]
[Authorize]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public sealed class LocationsController : ControllerBase
{
    private readonly IDesksService _desksService;
    private readonly ILocationsService _locationsService;
    private readonly IReservationsService _reservationsService;

    public LocationsController(IDesksService desksService, ILocationsService locationsService,
        IReservationsService reservationsService)
    {
        _desksService = desksService;
        _locationsService = locationsService;
        _reservationsService = reservationsService;
    }

    /// <summary>
    ///     Get method used for retrieving data of all locations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of locations. If any was found, empty list is returned</returns>
    /// <response code="200">Locations were returned successfully</response>
    [HttpGet]
    [Authorize(Roles = "Administrator,Employee")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations(CancellationToken cancellationToken)
    {
        return Ok(await _locationsService.GetLocationsAsync(cancellationToken));
    }

    /// <summary>
    ///     Post method used for adding new location
    /// </summary>
    /// <param name="newLocation"> NewLocation object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The guid of newly added location</returns>
    /// <response code="201">Location was added successfully</response>
    /// <response code="400">Some input body errors occurred </response>
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddLocation(NewLocation newLocation, CancellationToken cancellationToken)
    {
        var result = await _locationsService.AddLocation(newLocation, cancellationToken);
        return CreatedAtAction(nameof(AddLocation), new {Id = result});
    }

    /// <summary>
    ///     Delete method used for removing location with given id
    /// </summary>
    /// <param name="id">Location id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    /// <response code="204">Location was removed successfully</response>
    /// <response code="400">Location had one or more desk and removal was not possible</response>
    /// <response code="404">Location with given id was not found</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrator")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation(Guid id, CancellationToken cancellationToken)
    {
        await _locationsService.DeleteLocation(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    ///     Get method used for retrieving all desks in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of desks. If any was found, empty list is returned</returns>
    /// <response code="200">Desks were returned successfully</response>
    /// <response code="404">Location with given id was not found</response>
    [HttpGet("{locationId:guid}/desks")]
    [Authorize(Roles = "Administrator,Employee")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetDesks(Guid locationId, CancellationToken cancellationToken)
    {
        return Ok(await _desksService.GetDesksAsync(locationId, cancellationToken));
    }

    /// <summary>
    ///     Post method used for adding new desk in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="newDesk">NewDesk object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>guid of newly added desk</returns>
    /// <response code="201">Desk was added successfully</response>
    /// <response code="400">Some input body errors occurred</response>
    /// <response code="404">Location with given id was not found</response>
    [HttpPost("{locationId:guid}/desks")]
    [Authorize(Roles = "Administrator")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddDesk(Guid locationId, NewDesk newDesk, CancellationToken cancellationToken)
    {
        newDesk.LocationId = locationId;
        var result = await _desksService.AddDesk(newDesk, cancellationToken);
        return CreatedAtAction(nameof(AddLocation), new {Id = result});
    }

    /// <summary>
    ///     Put method used for updating desk with given id in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="id">Desk id</param>
    /// <param name="updatedDesk">UpdatedDesk object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Desk was updated successfully</response>
    /// <response code="400">
    ///     Desk had one or more active reservations and removal was not possible or some
    ///     input body errors occurred
    /// </response>
    /// <response code="404">Desk or location with given id was not found</response>
    [HttpPut("{locationId:guid}/desks/{id:guid}")]
    [Authorize(Roles = "Administrator,Employee")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDesk(Guid locationId, Guid id, UpdatedDesk updatedDesk,
        CancellationToken cancellationToken)
    {
        updatedDesk.LocationId = locationId;
        updatedDesk.Id = id;
        await _desksService.UpdateDesk(updatedDesk, cancellationToken);

        return NoContent();
    }

    /// <summary>
    ///     Delete method used for removing desk with given id from location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="id">Desk id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Desk was removed successfully</response>
    /// <response code="400">Desk had one or more active reservations and removal was not possible</response>
    /// <response code="404">Desk or location with given id was not found</response>
    [HttpDelete("{locationId:guid}/desks/{id:guid}")]
    [Authorize(Roles = "Administrator")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDesk(Guid locationId, Guid id,
        CancellationToken cancellationToken)
    {
        await _desksService.DeleteDesk(locationId, id, cancellationToken);

        return NoContent();
    }

    /// <summary>
    ///     Get method used for retrieving all reservations of desk with given id in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="deskId">Desk id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations. If any were found, empty list is returned</returns>
    /// <response code="200">Reservations were returned successfully</response>
    /// <response code="404">Desk or location with given id was not found</response>
    [HttpGet("{locationId:guid}/desks/{deskId:guid}/reservations")]
    [Authorize(Roles = "Administrator")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations(Guid locationId, Guid deskId,
        CancellationToken cancellationToken)
    {
        return Ok(await _reservationsService.GetReservationsAsync(locationId, deskId, cancellationToken));
    }

    /// <summary>
    ///     Post method used for making new reservation for desk with given id in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="deskId">Desk id</param>
    /// <param name="newReservation">NewReservation object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Id of newly created reservation</returns>
    /// <response code="201">Reservation was created successfully</response>
    /// <response code="400">
    ///     Given desk is not available or some input body errors occurred or dates are incorrect or
    ///     reservation was for period longer than 7 days
    /// </response>
    /// <response code="404">Desk or location with given id was not found</response>
    [HttpPost("{locationId:guid}/desks/{deskId:guid}/reservations")]
    [Authorize(Roles = "Administrator, Employee")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ReserveDesk(Guid locationId, Guid deskId,
        NewReservation newReservation,
        CancellationToken cancellationToken)
    {
        newReservation.LocationId = locationId;
        newReservation.DeskId = deskId;

        var result = await _reservationsService.ReserveDesk(newReservation, cancellationToken);
        return CreatedAtAction(nameof(ReserveDesk), new {Id = result});
    }

    /// <summary>
    ///     Put method used for changing existing reservation of desk with given id in location with given id
    /// </summary>
    /// <param name="locationId">Location id</param>
    /// <param name="deskId">Desk id</param>
    /// <param name="id">Reservation id</param>
    /// <param name="updatedReservation">UpdatedReservation object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Reservation was changed successfully</response>
    /// <response code="400">
    ///     Given desk is not available or some input body errors occurred or dates are incorrect or
    ///     reservation was for period longer than 7 days or it is less time than 24 hours till reservation start
    /// </response>
    /// <response code="404">Desk or location with given id was not found</response>
    [HttpPut("{locationId:guid}/desks/{deskId:guid}/reservations/{id:guid}")]
    [Authorize(Roles = "Administrator,Employee")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeReservation(Guid locationId, Guid deskId, Guid id,
        UpdatedReservation updatedReservation, CancellationToken cancellationToken)
    {
        updatedReservation.OldLocationId = locationId;
        updatedReservation.OldDeskId = deskId;
        updatedReservation.Id = id;

        await _reservationsService.ChangeReservation(updatedReservation, cancellationToken);

        return NoContent();
    }
}