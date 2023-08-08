using AutoMapper;
using FakeItEasy;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class UserControllerTests
    {
        private UserController _userController;
        private IUserRepository _fakeUserRepository;
        private IMapper _fakeMapper;

        public UserControllerTests()
        {
            _fakeUserRepository = A.Fake<IUserRepository>();
            _fakeMapper = A.Fake<IMapper>();

            _userController = new UserController(_fakeUserRepository, _fakeMapper);
        }

        private User CreateSampleUser()
        {
            return new User
            {
                UserId = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            };
        }
      /*  private User CreateSampleUsers()
        {
            var users = new List<User>
            {
                new User
                {
                UserId = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
                },
                new User
                {
                UserId = 2,
                Name = "John",
                Email = "john@example.com",
                Password = "password12",
                MobileNumber = 2234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
                }
            };

            return users;
        }*/

        [Fact]
        public async Task CreateUser_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            };

            var user = CreateSampleUser();

            A.CallTo(() => _fakeMapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _fakeUserRepository.AddUser(user)).Returns(true);
            A.CallTo(() => _fakeMapper.Map<UserDTO>(user)).Returns(userDto);

            // Act
            var result = await _userController.CreateUser(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var createdUserDto = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(userDto.Name, createdUserDto.Name);
            Assert.Equal(userDto.Email, createdUserDto.Email);
            Assert.Equal(userDto.Password, createdUserDto.Password);
            Assert.Equal(userDto.MobileNumber, createdUserDto.MobileNumber);
            Assert.Equal(userDto.RoleId, createdUserDto.RoleId);
            Assert.Equal(userDto.SpecializationId, createdUserDto.SpecializationId);
            Assert.Equal(userDto.HospitalId, createdUserDto.HospitalId);
        }

        [Fact]
        public async Task CreateUser_UserAlreadyExists_ReturnsConflictResult()
        {
            // Arrange
            var userDto = new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            };

            var user = CreateSampleUser();

            A.CallTo(() => _fakeMapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _fakeUserRepository.AddUser(user)).Returns(false);

            // Act
            var result = await _userController.CreateUser(userDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
            Assert.Equal("Email or mobile number already exists.", conflictResult.Value);
        }

        [Fact]
        public async Task GetUserById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var user = CreateSampleUser();

            A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(user);
            A.CallTo(() => _fakeMapper.Map<UserDTO>(user)).Returns(new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            });

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetUserById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 999; // Assuming this ID does not exist in the repository
            A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task UpdateUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var updatedUserDto = new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            };

            var existingUser = CreateSampleUser();
            A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(existingUser);

            var updatedUser = CreateSampleUser();
            A.CallTo(() => _fakeMapper.Map<User>(updatedUserDto)).Returns(updatedUser);

            A.CallTo(() => _fakeUserRepository.UpdateUser(updatedUser)).Returns(true);

            // Act
            var result = await _userController.UpdateUser(userId, updatedUserDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 999; // Assuming this ID does not exist in the repository
            var updatedUserDto = new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                MobileNumber = 1234567890,
                RoleId = 1,
                SpecializationId = 1,
                HospitalId = 1
            };

            A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _userController.UpdateUser(userId, updatedUserDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var existingUser = CreateSampleUser();

            A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(existingUser);
            A.CallTo(() => _fakeUserRepository.DeleteUser(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
        
               [Fact]
                public async Task DeleteUser_NonExistingId_ReturnsNotFoundResult()
                {
                    // Arrange
                    int userId = 999; // Assuming this ID does not exist in the repository

                    A.CallTo(() => _fakeUserRepository.GetUserById(userId)).Returns(Task.FromResult<User>(null));

                    // Act
                    var result = await _userController.DeleteUser(userId);

                    // Assert
                    var notFoundResult = Assert.IsType<NotFoundResult>(result);
                    Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                }

        [Fact]
        public async Task GetUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var fakeUserDTOs = new List<UserDTO>
    {
        new UserDTO {
            UserId = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            MobileNumber = 1234567890,
            RoleId = 1,
            SpecializationId = 1,
            HospitalId = 1 },
        new UserDTO {
            UserId = 2,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            MobileNumber = 1234567890,
            RoleId = 1,
            SpecializationId = 1,
            HospitalId = 1 }
    };

            A.CallTo(() => _fakeUserRepository.GetAllUsers()).Returns(Task.FromResult(fakeUserDTOs));

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

    }
}