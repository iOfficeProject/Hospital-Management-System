using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Mapping;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class HospitalRepositoryTests
    {
        private readonly IMapper _fakeMapper;

        public HospitalRepositoryTests()
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
        public async Task GetAllHospital_ReturnsListOfHospitals()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Hospitals.Add(new Hospital { HospitalId = 1, HospitalName = "Hospital 1", Location = "Location 1" });
                context.Hospitals.Add(new Hospital { HospitalId = 2, HospitalName = "Hospital 2", Location = "Location 2" });
                context.SaveChanges();

                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                var hospitals = await repository.GetAllHospital();

                // Assert
                // Check individual hospitals
                var hospitalDto1 = hospitals.FirstOrDefault(h => h.HospitalName == "Hospital 1");
                Assert.NotNull(hospitalDto1);
                Assert.Equal("Location 1", hospitalDto1.Location);

                var hospitalDto2 = hospitals.FirstOrDefault(h => h.HospitalName == "Hospital 2");
                Assert.NotNull(hospitalDto2);
                Assert.Equal("Location 2", hospitalDto2.Location);

                // Check the count of hospitals
                Assert.Equal(2, hospitals.Count());
            }
        }

        [Fact]
        public async Task GetByIdHospital_ReturnsHospital()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Hospitals.Add(new Hospital { HospitalId = 1, HospitalName = "Hospital 1", Location = "Location 1" });
                context.SaveChanges();

                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                var hospital = await repository.GetByIdHospital(1);

                // Assert
                Assert.NotNull(hospital);
                Assert.Equal("Hospital 1", hospital.HospitalName);
                Assert.Equal("Location 1", hospital.Location);
            }
        }

        [Fact]
        public async Task AddHospital_WithValidData_ReturnsTrue()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                var hospitalDto = new HospitalDTO { HospitalName = "New Hospital", Location = "New Location" };
                var isAdded = await repository.AddHospital(hospitalDto);

                // Assert
                Assert.True(isAdded);
            }
        }

        [Fact]
        public async Task AddHospital_HospitalAlreadyExists_ReturnsFalse()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var existingHospital = new Hospital { HospitalName = "Existing Hospital", Location = "Existing Location" };
                context.Hospitals.Add(existingHospital);
                context.SaveChanges();

                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                var hospitalDto = new HospitalDTO { HospitalName = "Existing Hospital", Location = "Existing Location" };
                var isAdded = await repository.AddHospital(hospitalDto);

                // Assert
                Assert.False(isAdded);
            }
        }


        [Fact]
        public async Task UpdateHospital_WithValidData_ReturnsTrue()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                context.Hospitals.Add(new Hospital { HospitalId = 1, HospitalName = "Hospital 1", Location = "Location 1" });
                context.SaveChanges();
            }

            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                var hospitalDto = new HospitalDTO { HospitalName = "Updated Hospital", Location = "Updated Location" };
                var isUpdated = await repository.UpdateHospital(1, hospitalDto);

                // Assert
                Assert.True(isUpdated);
            }
        }

        [Fact]
        public async Task DeleteHospital_RemovesHospital()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Hospitals.Add(new Hospital { HospitalId = 1, HospitalName = "Hospital 1", Location = "Location 1" });
                context.SaveChanges();
            }

            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                await repository.DeleteHospital(1);

                // Assert
                var deletedHospital = await context.Hospitals.FindAsync(1);
                Assert.Null(deletedHospital);
            }
        }

        [Fact]
        public async Task DeleteHospital_HospitalExists_RemovesHospital()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var hospital = new Hospital { HospitalId = 1, HospitalName = "Hospital 1", Location = "Location 1" };
                context.Hospitals.Add(hospital);
                context.SaveChanges();

                var repository = new HospitalRepository(context, _fakeMapper);

                // Act
                await repository.DeleteHospital(1);

                // Assert
                var deletedHospital = await context.Hospitals.FindAsync(1);
                Assert.Null(deletedHospital);
            }
        }
    }
}
