using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Mapping;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class UserRepositoryTests
    {
        private readonly IMapper _fakeMapper;

        public UserRepositoryTests()
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
                context.Users.Add(new User { UserId = 1, Name = "Juli" });
                context.Users.Add(new User { UserId = 2, Name = "Juned"});
                context.SaveChanges();
                var repository = new UserRepository(context, _fakeMapper);

                // Act
                var users = await repository.GetAllUsers();

                // Assert
                var userDto1 = users.FirstOrDefault(h => h.Name == "Juli");
                Assert.NotNull(userDto1);
                Assert.Equal("Juli", userDto1.Name);

                var userDto2 = users.FirstOrDefault(h => h.Name == "Juned");
                Assert.NotNull(userDto2);
                Assert.Equal("Juned", userDto2.Name);

                Assert.Equal(2, users.Count());
            }
        }

        [Fact]
        public async Task GetUserById_ReturnsUser()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                context.Users.Add(new User { UserId = 1, Name = "Juli"});
                context.SaveChanges();
                var repository = new UserRepository(context, _fakeMapper);

                // Act
                var user = await repository.GetUserById(1);

                // Assert
                Assert.NotNull(user);
                Assert.Equal("Juli", user.Name);      
            }
        }

        [Fact]
        public async Task AddUser_WithValidData_ReturnsTrue()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new UserRepository(context, _fakeMapper);

                // Act
                var userDto = new UserDTO
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh", 
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                };
                var isAdded = await repository.AddUser(_fakeMapper.Map<User>(userDto));

                // Assert
                Assert.True(isAdded);
            }
        }

        [Fact]
        public async Task AddUser_WithExistingEmailOrMobileNumber_ReturnsFalse()
        {
            // Arrange
            using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
            {
                var repository = new UserRepository(context, _fakeMapper);

                var existingUserWithEmail = new User
                {
                    Name = "Existing User",
                    Email = "john@example.com",
                    Password = "existingpassword",
                    MobileNumber = 9876543210,
                    RoleId = 2,
                    SpecializationId = 3,
                    HospitalId = 4
                };
                context.Users.Add(existingUserWithEmail);
                context.SaveChanges();

                var userDtoWithEmail = new UserDTO
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                };

                // Act
                var isAddedWithEmail = await repository.AddUser(_fakeMapper.Map<User>(userDtoWithEmail));

                // Assert
                Assert.False(isAddedWithEmail);

                var existingUserWithMobileNumber = new User
                {
                    Name = "Existing User",
                    Email = "existing@example.com",
                    Password = "existingpassword",
                    MobileNumber = 1234567890,
                    RoleId = 2,
                    SpecializationId = 3,
                    HospitalId = 4
                };
                context.Users.Add(existingUserWithMobileNumber);
                context.SaveChanges();

                var userDtoWithMobileNumber = new UserDTO
                {
                    Name = "Jane Doe",
                    Email = "jane@example.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                };

                // Act
                var isAddedWithMobileNumber = await repository.AddUser(_fakeMapper.Map<User>(userDtoWithMobileNumber));

                // Assert
                Assert.False(isAddedWithMobileNumber);
            }
        }


        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsTrue()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                context.Users.Add(new User
                {
                    UserId = 1,
                    Name = "John",
                    Email = "j@gmail.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                }); 
                context.SaveChanges();
            }
            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                var repository = new UserRepository(context, _fakeMapper);

                // Act
                var user = new User
                {
                    UserId = 1,
                    Name = "JohnDoe",
                    Email = "j@gmail.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                };
                var isUpdated = await repository.UpdateUser(user);

                // Assert
                Assert.True(isUpdated);
            }
        }

        [Fact]
        public async Task UpdateUser_WithDuplicateEmailOrMobileNumber_ReturnsFalse()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                context.Users.Add(new User
                {
                    UserId = 1001,
                    Name = "John",
                    Email = "j@gmail.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                });
                context.SaveChanges();
            }

            using (var context = new Master_Hospital_ManagementContext(dbContextOptions))
            {
                var repository = new UserRepository(context, _fakeMapper);

                // Act
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserId == 1001);
                var updatedUser = new User
                {
                    UserId = 1,
                    Name = "JohnDoe",
                    Email = "j@gmail.com",
                    Password = "jhgjghcgduifdgkdljghvhdfighvldfh",
                    MobileNumber = 9876543210, // Duplicate mobile number
                    RoleId = 1,
                    SpecializationId = 2,
                    HospitalId = 3
                };

                var isUpdatedWithDuplicateMobile = await repository.UpdateUser(updatedUser);

                // Assert
                Assert.False(isUpdatedWithDuplicateMobile);

                // Act
                updatedUser.MobileNumber = 1234567890; // Valid mobile number, but duplicate email
                updatedUser.Email = "existing@gmail.com";
                var isUpdatedWithDuplicateEmail = await repository.UpdateUser(updatedUser);

                // Assert
                Assert.False(isUpdatedWithDuplicateEmail);
            }
        }


        [Fact]
            public async Task DeleteUser_RemovesUser()
            {
                // Arrange
                using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
                {
                    context.Users.Add(new User { UserId = 1, Name = "Juli" });
                    context.SaveChanges();
                }

                using (var context = new Master_Hospital_ManagementContext(CreateDbContextOptions()))
                {
                    var repository = new UserRepository(context, _fakeMapper);

                    // Act
                    await repository.DeleteUser(1);

                    // Assert
                    var deletedUser = await context.Users.FindAsync(1);
                    Assert.Null(deletedUser);
                }
            }

        [Fact]
        public async Task DeleteUser_UserExists_RemovesUserAndCommitsChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUser_UserExists_RemovesUserAndCommitsChanges")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var user = new User { UserId = 1, Name = "Juli" };

                context.Users.Add(user);
                context.SaveChanges();

                var repository = new UserRepository(context, _fakeMapper);

                // Act
                await repository.DeleteUser(1);

                // Assert
                var deletedUser = await context.Users.FindAsync(1);
                Assert.Null(deletedUser);
            }
        }


        [Fact]
        public async Task DeleteUser_InvalidUserId_DoesNotRemoveUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteUser_InvalidUserId_DoesNotRemoveUser")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var user = new User { UserId = 1, Name = "Juli" };

                context.Users.Add(user);
                context.SaveChanges();

                var repository = new UserRepository(context, _fakeMapper);

                // Act
                await repository.DeleteUser(-1);

                // Assert
                var notDeletedUser = await context.Users.FindAsync(1);
                Assert.NotNull(notDeletedUser);
            }
        }

    }
}
