using Xunit;
using FakeItEasy;
using Hospital_Appointment_Booking_System.Repositories;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Appointment_Booking_System.UnitTests.Repositories
{
    public class RoleRepositoryTests
    {
        [Fact]
        public async Task AddRole_ValidRole_SuccessfullyAdded()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "AddRole_ValidRole_SuccessfullyAdded")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var repository = new RoleRepository(context);
                var role = new Role { RoleName = "Admin" };

                // Act
                var result = await repository.AddRole(role);

                // Assert
                Assert.True(result);

                var addedRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
                Assert.NotNull(addedRole);
            }
        }

        [Fact]
        public async Task AddRole_DuplicateRole_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "AddRole_DuplicateRole_ReturnsFalse")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                context.Roles.Add(new Role { RoleName = "Admin" });
                context.SaveChanges();

                var repository = new RoleRepository(context);
                var role = new Role { RoleName = "Admin" };

                // Act
                var result = await repository.AddRole(role);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteRole_ExistingRoleId_SuccessfullyDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteRole_ExistingRoleId_SuccessfullyDeleted")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var role = new Role { RoleName = "Admin" };
                context.Roles.Add(role);
                context.SaveChanges();

                var repository = new RoleRepository(context);

                // Act
                await repository.DeleteRole(role.RoleId);

                // Assert
                var deletedRole = await context.Roles.FindAsync(role.RoleId);
                Assert.Null(deletedRole);
            }
        }

        [Fact]
        public async Task DeleteRole_NonExistingRoleId_NothingDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteRole_NonExistingRoleId_NothingDeleted")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var repository = new RoleRepository(context);

                // Act
                await repository.DeleteRole(999);

                // Assert
                var roles = await context.Roles.ToListAsync();
                Assert.Empty(roles);
            }
        }

        [Fact]
        public async Task GetRoleById_ExistingRoleId_ReturnsRole()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetRoleById_ExistingRoleId_ReturnsRole")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var role = new Role { RoleName = "Admin" };
                context.Roles.Add(role);
                context.SaveChanges();

                var repository = new RoleRepository(context);

                // Act
                var result = await repository.GetRoleById(role.RoleId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(role.RoleName, result.RoleName);
            }
        }
        [Fact]
        public async Task GetRoleById_NonExistingRoleId_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetRoleById_NonExistingRoleId_ReturnsNull")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var repository = new RoleRepository(context);

                // Act
                var result = await repository.GetRoleById(999);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllRoles_RolesExist_ReturnsRolesList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetAllRoles_RolesExist_ReturnsRolesList")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var roles = new List<Role>
                {
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Doctor" },
                    new Role { RoleName = "Nurse" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();

                var repository = new RoleRepository(context);

                // Act
                var result = await repository.GetAllRoles();

                // Assert
                Assert.Equal(roles.Count, result.Count);
                Assert.Equal(roles.Select(r => r.RoleName), result.Select(r => r.RoleName));
            }
        }
    }
}