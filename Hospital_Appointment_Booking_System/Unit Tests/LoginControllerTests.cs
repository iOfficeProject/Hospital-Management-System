using AutoMapper;
using FakeItEasy;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Helpers;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json.Linq;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class LoginControllerTests
    {
        private readonly IConfiguration _fakeConfiguration;

        public LoginControllerTests()
        {
            _fakeConfiguration = A.Fake<IConfiguration>();
            A.CallTo(() => _fakeConfiguration["Jwt:Key"]).Returns("my-secret-key");
            A.CallTo(() => _fakeConfiguration["Jwt:Issuer"]).Returns("my-issuer");
            A.CallTo(() => _fakeConfiguration["Jwt:Audience"]).Returns("my-audience");
            A.CallTo(() => _fakeConfiguration["Jwt:Subject"]).Returns("my-subject");
        }

        private Master_Hospital_ManagementContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new Master_Hospital_ManagementContext(options);
        }

        [Fact]
        public async Task Post_ValidCredentials_ReturnsToken()
        {
            // Arrange
             using (var context = CreateDbContext())
             {
              
                var user = new User
                {
                UserId = 1,
                Name = "Jay Sharma",
                Email = "jay@gmail.com",
                Password = PasswordHasher.EncryptPassword("Pass@123"),
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
                };

                context.Users.Add(user);
                context.Roles.Add(new Role { RoleId = 1, RoleName = "User" });
                context.SaveChanges();


                var controller = new LoginController(_fakeConfiguration, context);
                A.CallTo(() => _fakeConfiguration["Jwt:Key"]).Returns("01234567890123456789012345678901");

                // Act
                var result = await controller.Post(new UserDTO { Email = "jay@gmail.com", Password = "Pass@123" });

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = okResult.Value as dynamic;

                Assert.NotNull(response);

                var token = response.Token;
                var roleName = response.roleName;

                Assert.NotNull(token);
                Assert.NotNull(roleName);
                  
            }
        }

        [Fact]
        public async Task Post_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                context.Users.Add(new User { UserId = 1, Email = "jay@gmail.com", Password = PasswordHasher.EncryptPassword("Pass@123") });
                context.SaveChanges();

                var controller = new LoginController(_fakeConfiguration, CreateDbContext());

                // Act
                var result = await controller.Post(new UserDTO { Email = "jay@gmail.com", Password = "Pass12" });

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public async Task Post_InvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            using (var context = CreateDbContext())
            {
                var user = new User
                {
                    UserId = 1,
                    Name = "Jay Sharma",
                    Email = "jay@gmail.com",
                    Password = PasswordHasher.EncryptPassword("Pass@123"),
                    MobileNumber = 1234567890,
                    RoleId = 1,
                    SpecializationId = 1,
                    HospitalId = 1
                };

                context.Users.Add(user);
                context.Roles.Add(new Role { RoleId = 1, RoleName = "User" });
                context.SaveChanges();

                var controller = new LoginController(_fakeConfiguration, context);
                A.CallTo(() => _fakeConfiguration["Jwt:Key"]).Returns("01234567890123456789012345678901");

                // Act
                var result = await controller.Post(new UserDTO { Email = "jay@gmail.com", Password = "WrongPassword" });

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
                Assert.Equal("Invalid credentials", badRequestResult.Value);
            }
        }
    }
}
