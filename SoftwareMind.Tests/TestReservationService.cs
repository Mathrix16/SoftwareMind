using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftwareMind.Core.Dtos;
using SoftwareMind.Core.Exceptions;
using SoftwareMind.Core.Services;
using SoftwareMind.Dal;
using SoftwareMind.Dal.Entities;
using SoftwareMind.Dal.Repositories;
using Xunit;

namespace SoftwareMind.Tests;

public class TestReservationService : IDisposable
{

    private readonly ApplicationDbContext _context;

    public TestReservationService()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureCreated();
        
        
    }

    [Fact]
    public async Task AddNewReservation_ResultBadRequestException_ReservationLongerThan7Days()
    {
       //ARRANGE 
       _context.Locations.Add(new Location
       {
           Id = new Guid("daad5939-f71d-4a48-a7fe-930f0b6cb348"),
           Name = "E2.3"
       });

       _context.Desks.Add(new Desk
       {
           LocationId = new Guid("daad5939-f71d-4a48-a7fe-930f0b6cb348"),
           Id = new Guid("9529e901-201f-4ca3-a120-1760113a00fe"),
           IsAvailable = true,
           Name = "Przy Oknie"
       });

       await _context.SaveChangesAsync();

       var newReservation = new NewReservation
       {
           LocationId = new Guid("daad5939-f71d-4a48-a7fe-930f0b6cb348"),
           DeskId = new Guid("9529e901-201f-4ca3-a120-1760113a00fe"),
           StartDate = DateTime.Now.AddDays(1),
           EndDate = DateTime.Now.AddDays(10),
           Surname = "Smith"

       };
       
       var sut = new ReservationsService(new ReservationsRepository(_context), new DesksRepository(_context));
       
       //ACT

       Task Act() => sut.ReserveDesk(newReservation, new CancellationToken());

       //ASSERT
      await Assert.ThrowsAsync<BadRequestException>(Act);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}