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
        public async Task AvailabilityController_GetAvailability_ReturnOK()
        {
            //Arrange
            var fakeAvailabilities = new List<Availability> 
            { 
            new Availability { UserId = 1,IsAvailable=true,Date=DateTime.Now,StartTime=DateTime.Now,EndTime=DateTime.Now},
            new Availability { UserId = 2,IsAvailable=false,Date=DateTime.Now,StartTime=DateTime.Now,EndTime=DateTime.Now}
            };
            A.CallTo(() => _availabilityRepository.GetAllAvailability()).Returns(fakeAvailabilities);

            //Act
            var result = _controller.GetAvailability();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAvailabilityById_WithValidId_ReturnsOkResultWithData()
        {
            // Arrange
            int fakeId = 1;
            var fakeAvailability = new Availability { UserId = fakeId, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(fakeAvailability);

            // Act
            var result = await _controller.GetAvailabilityById(fakeId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(fakeAvailability);
        }

        [Fact]
        public async Task GetAvailabilityById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int fakeId = 999; // Non-existing ID
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.GetAvailabilityById(fakeId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public async Task AvailabilityController_AddAvailability_ReturnOK()
        {
            // Arrange
            var fakeAvailabilityDTO = new AvailabilityDTO { UserId = 5, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            var fakeAvailability = new Availability { UserId = 5, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };

            A.CallTo(() => _availabilityRepository.AddAvailability(fakeAvailability)).Returns(Task.CompletedTask);

            A.CallTo(() => _mapper.Map<Availability>(fakeAvailabilityDTO)).Returns(fakeAvailability);

            //A.CallTo(() => _mapper.Map<Availability>(A<AvailabilityDTO>._)).Returns(fakeAvailability);
            A.CallTo(() => _mapper.Map<AvailabilityDTO>(fakeAvailability)).Returns(fakeAvailabilityDTO);

            // Act
            var result = await _controller.AddAvailability(fakeAvailabilityDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var mappedAvailabilityDTO = Assert.IsType<AvailabilityDTO>(okResult.Value);
            // Add more assertions to compare the properties of mappedAvailabilityDTO if necessary
        }

        [Fact]
        public async Task UpdateAvailability_WithValidData_ReturnsNoContent()
        {
            // Arrange
            int fakeId = 1;
            var originalStartTime = DateTime.Now.AddDays(-1); // Some initial start time
            var updatedStartTime = DateTime.Now; // The updated start time

            var fakeAvailabilityDTO = new AvailabilityDTO
            {
                UserId = fakeId,
                IsAvailable = true,
                Date = DateTime.Now,
                StartTime = updatedStartTime,
                EndTime = DateTime.Now
            };
            var fakeAvailability = new Availability
            {
                UserId = fakeId,
                IsAvailable = false,
                Date = DateTime.Now,
                StartTime = originalStartTime,
                EndTime = DateTime.Now
            };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(fakeAvailability);

            // Act
            var result = await _controller.UpdateAvailability(fakeId, fakeAvailabilityDTO);
            var updatedAvailability = await _availabilityRepository.GetAvailabilityById(fakeId);


            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateAvailability_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int fakeId = 99; // Non-existing ID
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.UpdateAvailability(fakeId, new AvailabilityDTO());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAvailability_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int fakeId = 1;
            var fakeAvailability = new Availability { UserId = fakeId, IsAvailable = true, Date = DateTime.Now, StartTime = DateTime.Now, EndTime = DateTime.Now };
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(fakeAvailability);

            // Act
            var result = await _controller.DeleteAvailability(fakeId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAvailability_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int fakeId = 99; 
            A.CallTo(() => _availabilityRepository.GetAvailabilityById(fakeId)).Returns(Task.FromResult<Availability>(null));

            // Act
            var result = await _controller.DeleteAvailability(fakeId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}