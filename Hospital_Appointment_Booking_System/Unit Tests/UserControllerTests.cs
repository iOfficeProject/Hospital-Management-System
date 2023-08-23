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
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserControllerTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();

            _userController = new UserController(_userRepository, _mapper);
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
    
        [Fact]
        public async Task CreateUser_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var expectedUserDto = new UserDTO
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

            A.CallTo(() => _mapper.Map<User>(expectedUserDto)).Returns(user);
            A.CallTo(() => _userRepository.AddUser(user)).Returns(true);
            A.CallTo(() => _mapper.Map<UserDTO>(user)).Returns(expectedUserDto);

            // Act
            var result = await _userController.CreateUser(expectedUserDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var actualUserDto = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(expectedUserDto.Name, actualUserDto.Name);
            Assert.Equal(expectedUserDto.Email, actualUserDto.Email);
            Assert.Equal(expectedUserDto.Password, actualUserDto.Password);
            Assert.Equal(expectedUserDto.MobileNumber, actualUserDto.MobileNumber);
            Assert.Equal(expectedUserDto.RoleId, actualUserDto.RoleId);
            Assert.Equal(expectedUserDto.SpecializationId, actualUserDto.SpecializationId);
            Assert.Equal(expectedUserDto.HospitalId, actualUserDto.HospitalId);
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

            A.CallTo(() => _mapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _userRepository.AddUser(user)).Returns(false);

            // Act
            var result = await _userController.CreateUser(userDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
            Assert.Equal("Email or mobile number already exists.", conflictResult.Value);
        }

        [Fact]
        public async Task CreateUser_Exception_ReturnsInternalServerError()
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

            A.CallTo(() => _mapper.Map<User>(userDto)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _userController.CreateUser(userDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var user = CreateSampleUser();

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(user);
            A.CallTo(() => _mapper.Map<UserDTO>(user)).Returns(new UserDTO
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
            int userId = 999;
            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task GetUserById_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int userId = 1;

            A.CallTo(() => _userRepository.GetUserById(userId)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    
    [Fact]
        public async Task UpdateUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
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

            var existingUser = CreateSampleUser();

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(existingUser);

            var updatedUser = CreateSampleUser();

            A.CallTo(() => _mapper.Map<User>(userDto)).Returns(updatedUser);

            A.CallTo(() => _userRepository.UpdateUser(updatedUser)).Returns(true);

            // Act
            var result = await _userController.UpdateUser(userId, userDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 999;
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

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _userController.UpdateUser(userId, userDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int userId = 1;
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

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(new User()); // Simulating an existing user

            A.CallTo(() => _userRepository.UpdateUser(A<User>._)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _userController.UpdateUser(userId, userDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
  
    [Fact]
        public async Task DeleteUser_ExistingId_ReturnsOkResult()
        {
            // Arrange
            int userId = 1;
            var existingUser = CreateSampleUser();

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(existingUser);
            A.CallTo(() => _userRepository.DeleteUser(userId)).Returns(Task.CompletedTask);

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
                    int expectedUserId = 999;

                    A.CallTo(() => _userRepository.GetUserById(expectedUserId)).Returns(Task.FromResult<User>(null));

                    // Act
                    var result = await _userController.DeleteUser(expectedUserId);

                    // Assert
                    var notFoundResult = Assert.IsType<NotFoundResult>(result);
                    Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                }
        [Fact]
        public async Task DeleteUser_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int userId = 1;

            A.CallTo(() => _userRepository.GetUserById(userId)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _userController.DeleteUser(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var userDTO = new List<UserDTO>
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

            A.CallTo(() => _userRepository.GetAllUsers()).Returns(Task.FromResult(userDTO));

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

    }
}