using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hospital_Appointment_Booking_System.Unit_Tests
{
    public class AvailabilityControllerTests
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;
        private readonly AvailabilityController _controller;


        public AvailabilityControllerTests()
        {
            _availabilityRepository = A.Fake<IAvailabilityRepository>();
            _mapper = A.Fake<IMapper>();
            _controller = new AvailabilityController(_availabilityRepository, _mapper);

        }

        [Fact]
        public async Task AvailabilityController_GetAvailability()
        {
            //Arrange
            var availabilities = new List<Availability> 
            { 
            new Availability { UserId = 1,IsAvailable=true,Date=DateTime.Now,StartTime=DateTime.Now,EndTime=DateTime.Now},
            new Availability { UserId = 2,IsAvailable=false,Date=DateTime.Now,StartTime=DateTime.Now,EndTime=DateTime.Now}
            };
            A.CallTo(() => _availabilityRepository.GetAllAvailability()).Returns(availabilities);

            //Act
            var result = _controller.GetAvailability();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAvailabilityById_WithValidId_ReturnsOkResultWithData()
        {
            // Arrange
            int existingId = 1;
            var fakeAvailability = new Availability { UserId = existingId, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(existingId)).Returns(fakeAvailability);

            // Act
            var result = await _controller.GetAvailabilityById(existingId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(fakeAvailability);
        }

        [Fact]
        public async Task GetAvailabilityById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(nonExistingId)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.GetAvailabilityById(nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async Task AvailabilityController_AddAvailability_ReturnOK()
        {
            // Arrange
            var availabilityDTO = new AvailabilityDTO { UserId = 5, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            var availability = new Availability { UserId = 5, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };

            A.CallTo(() => _availabilityRepository.AddAvailability(availability)).Returns(Task.CompletedTask);

            A.CallTo(() => _mapper.Map<Availability>(availabilityDTO)).Returns(availability);

            A.CallTo(() => _mapper.Map<AvailabilityDTO>(availability)).Returns(availabilityDTO);

            // Act
            var result = await _controller.AddAvailability(availabilityDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var mappedAvailabilityDTO = Assert.IsType<AvailabilityDTO>(okResult.Value);
        }

        [Fact]
        public async Task UpdateAvailability_WithValidData_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            var originalStartTime = DateTime.Now.AddDays(-1);
            var updatedStartTime = DateTime.Now;

            var availabilityDTO = new AvailabilityDTO
            {
                UserId = id,
                IsAvailable = true,
                Date = DateTime.Now,
                StartTime = updatedStartTime,
                EndTime = DateTime.Now
            };
            var availability = new Availability
            {
                UserId = id,
                IsAvailable = false,
                Date = DateTime.Now,
                StartTime = originalStartTime,
                EndTime = DateTime.Now
            };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(id)).Returns(availability);
            A.CallTo(() => _availabilityRepository.UpdateAvailability(availability)).Returns(Task.CompletedTask); ;

            // Act
            var result = await _controller.UpdateAvailability(id, availabilityDTO);
            var updatedAvailability = await _availabilityRepository.GetAvailabilityById(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateAvailability_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int id = 99;
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(id)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.UpdateAvailability(id, new AvailabilityDTO());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAvailability_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int id = 1;
            var availability = new Availability { UserId = id, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(id)).Returns(availability);
            A.CallTo(() => _availabilityRepository.DeleteAvailability(availability));

            // Act
            var result = await _controller.DeleteAvailability(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAvailability_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int id = 99; 
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(id)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.DeleteAvailability(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

        }
    }
}