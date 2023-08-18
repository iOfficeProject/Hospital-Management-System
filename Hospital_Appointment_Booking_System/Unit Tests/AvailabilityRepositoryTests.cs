using AutoMapper;
using FakeItEasy;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Mapping;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class AvailabilityRepositoryTests
    {
        private readonly IMapper _fakeMapper;

        public AvailabilityRepositoryTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _fakeMapper = mapperConfig.CreateMapper();
        }

        private DbContextOptions<Master_Hospital_ManagementContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
        }

        [Fact]
        public async Task GetAvailabilityById_ReturnsAvailability()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Availabilities.Add(new Availability { AvailabilityId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) });
                context.SaveChanges();
           
                var repository = new AvailabilityRepository(context);

                // Act
                var availability = await repository.GetAvailabilityById(1);

                // Assert
                Assert.NotNull(availability);
                Assert.Equal(1, availability.AvailabilityId);
            }
        }

        [Fact]
        public async Task GetAllAvailability_ReturnsListOfAvailabilities()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Availabilities.Add(new Availability { AvailabilityId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) });
                context.Availabilities.Add(new Availability { AvailabilityId = 2, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) });
                context.SaveChanges();
            
                var repository = new AvailabilityRepository(context);

                // Act
                var availabilities = await repository.GetAllAvailability();

                // Assert
                Assert.Equal(2, availabilities.Count());
            }
        }

        [Fact]
        public async Task AddAvailability_WithValidData_ReturnsTrue()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new AvailabilityRepository(context);

                // Act
                var availability = new Availability { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) };
                await repository.AddAvailability(availability);

                // Assert
                Assert.NotNull(availability);
                Assert.True(availability.AvailabilityId > 0);
            }
        }

        [Fact]
        public async Task DeleteAvailability_RemovesAvailability()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Availabilities.Add(new Availability { AvailabilityId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) });
                context.SaveChanges();
           
                var repository = new AvailabilityRepository(context);

                // Act
                var availability = await repository.GetAvailabilityById(1);
                await repository.DeleteAvailability(availability);

                // Assert
                var deletedAvailability = await context.Availabilities.FindAsync(1);
                Assert.Null(deletedAvailability);
            }
        }
    }
}
