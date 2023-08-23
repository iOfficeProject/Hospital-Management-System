using AutoMapper;
using FakeItEasy;
using Hospital_Appointment_Booking_System.DTO;
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
    public class SpecializationRepositoryTests
    {
        private readonly IMapper _fakeMapper;

        public SpecializationRepositoryTests()
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
        public async Task GetAllSpecializations_ReturnsListOfSpecializations()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Specializations.Add(new Specialization { SpecializationId = 1, SpecializationName = "Cardio" });
                context.Specializations.Add(new Specialization { SpecializationId = 2, SpecializationName = "Dental Care" });
                context.SaveChanges(); 
            
                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act
                var specializations = await repository.GetAllSpecializations();

                // Assert
                Assert.Equal(2, specializations.Count());
            }
        }


        [Fact]
        public async Task GetSpecializationById_ReturnsSpecialization()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Specializations.Add(new Specialization { SpecializationId = 1, SpecializationName = "Dental Care" });
                context.SaveChanges();
        
                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act
                var specialization = await repository.GetSpecializationById(1);

                // Assert
                Assert.NotNull(specialization);
                Assert.Equal("Dental Care", specialization.SpecializationName);
            }
        }

        [Fact]
        public async Task AddSpecialization_WithValidData_ReturnsTrue()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act
                var specializationDto = new SpecializationDTO { SpecializationName = "Cardio" };
                var isAdded = await repository.AddSpecialization(specializationDto);

                // Assert
                Assert.True(isAdded);
            }
        }

        [Fact]
        public async Task UpdateSpecialization_WithValidData_ReturnsTrue()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Specializations.Add(new Specialization { SpecializationId = 1, SpecializationName = "Dental Care" });
                context.SaveChanges();
       
                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act
                var specializationDto = new SpecializationDTO { SpecializationId = 1, SpecializationName = "Cardio" };
                await repository.UpdateSpecialization(specializationDto);

                // Assert
                var updatedSpecialization = await context.Specializations.FindAsync(1);
                Assert.NotNull(updatedSpecialization);
                Assert.Equal("Cardio", updatedSpecialization.SpecializationName);
            }
        }

        [Fact]
        public async Task DeleteSpecialization_RemovesSpecialization()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Specializations.Add(new Specialization { SpecializationId = 1, SpecializationName = "Cardio" });
                context.SaveChanges();
           
                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act
                await repository.DeleteSpecialization(1);

                // Assert
                var deletedSpecialization = await context.Specializations.FindAsync(1);
                Assert.Null(deletedSpecialization);
            }
        }

        [Fact]
        public async Task DeleteSpecialization_InvalidSpecializationId_ThrowsArgumentException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteSpecialization_InvalidSpecializationId_ThrowsArgumentException")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var specialization = new Specialization { SpecializationName = "Cardio" };

                context.Specializations.Add(specialization);
                context.SaveChanges();

                var repository = new SpecializationRepository(context, _fakeMapper);

                // Act and Assert
                await Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteSpecialization(-1));
            }
        }

    }
}
