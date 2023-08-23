using Xunit;
using FakeItEasy;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.DTO;

namespace Hospital_Appointment_Booking_System.Unit_Tests
{
    public class AppointmentControllerTests
    {
        private readonly AppointmentController _appointmentController;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentControllerTests()
        {
            _appointmentRepository = A.Fake<IAppointmentRepository>();
            _mapper = A.Fake<IMapper>();

            _appointmentController = new AppointmentController(_appointmentRepository);
        }

        [Fact]
        public async Task GetAllAppointments_ReturnsOkResult()
        {
            // Arrange
            var expectedAppointmentDTOs = new List<AppointmentDTO>
            {
                new AppointmentDTO { AppointmentId = 1, AppointmentDate = DateTime.Today, AppointmentStartTime=DateTime.Now, AppointmentEndTime=DateTime.Now.AddHours(1),SlotId=1, HospitalId=1, UserId=1 },
                new AppointmentDTO { AppointmentId = 2, AppointmentDate = DateTime.Today, AppointmentStartTime=DateTime.Now.AddHours(2), AppointmentEndTime=DateTime.Now.AddHours(3),SlotId=2, HospitalId=1, UserId=2 }
            };
            A.CallTo(() => _appointmentRepository.GetAllAppointments()).Returns(expectedAppointmentDTOs);

            // Act
            var result = await _appointmentController.GetAllAppointments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualAppointmentDTOs = Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(okResult.Value);
            Assert.Equal(expectedAppointmentDTOs.Count, actualAppointmentDTOs.Count());
        }

        [Fact]
        public async Task GetAppointmentById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var appointmentId = 1;
            var expectedAppointmentDTOs = new AppointmentDTO { AppointmentId = 1, AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1), SlotId = 1, HospitalId = 1, UserId = 1 };
            A.CallTo(() => _appointmentRepository.GetAppointmentById(appointmentId)).Returns(expectedAppointmentDTOs);

            // Act
            var result = await _appointmentController.GetAppointmentById(appointmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualAppointmentDTOs = Assert.IsType<AppointmentDTO>(okResult.Value);
            Assert.Equal(expectedAppointmentDTOs.AppointmentId, actualAppointmentDTOs.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentById_AppointmentIsNull_ReturnsNotFoundResult()
        {
            // Arrange
            int appointmentId = 1;
            AppointmentDTO expectedAppointment = null;

            A.CallTo(() => _appointmentRepository.GetAppointmentById(appointmentId)).Returns(expectedAppointment);

            // Act
            var result = await _appointmentController.GetAppointmentById(appointmentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task AddAppointment_ValidAppointment_ReturnsOkResult()
        {
            // Arrange
            var appointmentDto = new AppointmentInputDTO
            {
                AppointmentDate = DateTime.Today,
                AppointmentStartTime = DateTime.Today.AddHours(9),
                AppointmentEndTime = DateTime.Today.AddHours(10),
                SlotId = 1,
                HospitalId = 1,
                UserId = 1
            };

            A.CallTo(() => _appointmentRepository.AddAppointment(appointmentDto)).DoesNothing();

            // Act
            var result = await _appointmentController.AddAppointment(appointmentDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateAppointment_ExistingId_ValidAppointment_ReturnsOkResult()
        {
            // Arrange
            var appointmentId = 1;
            var appointmentDto = new AppointmentInputDTO
            {
                AppointmentId = appointmentId,
                AppointmentDate = DateTime.Today,
                AppointmentStartTime = DateTime.Today.AddHours(10),
                AppointmentEndTime = DateTime.Today.AddHours(11),
                SlotId = 2,
                HospitalId = 2,
                UserId = 2
            };

            A.CallTo(() => _appointmentRepository.UpdateAppointment(appointmentDto)).DoesNothing();
            A.CallTo(() => _appointmentRepository.GetAppointmentById(appointmentId));

            // Act
            var result = await _appointmentController.UpdateAppointment(appointmentId, appointmentDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAppointment_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var appointmentId = 1;

            A.CallTo(() => _appointmentRepository.DeleteAppointment(appointmentId)).DoesNothing();
            A.CallTo(() => _appointmentRepository.GetAppointmentById(appointmentId));

            // Act
            var result = await _appointmentController.DeleteAppointment(appointmentId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetAppointmentsByUserId_ExistingUserId_ReturnsOkResultWithAppointments()
        {
            // Arrange
            var userId = 1;
            var expectedAppointmentDTOs = new List<AppointmentDTO>
            {
                new AppointmentDTO { AppointmentId = userId, AppointmentDate = DateTime.Today, AppointmentStartTime=DateTime.Now, AppointmentEndTime=DateTime.Now.AddHours(1),SlotId=1, HospitalId=1, UserId=1 },
                new AppointmentDTO { AppointmentId = userId, AppointmentDate = DateTime.Today, AppointmentStartTime=DateTime.Now.AddHours(2), AppointmentEndTime=DateTime.Now.AddHours(3),SlotId=2, HospitalId=1, UserId=2 }
            };

            A.CallTo(() => _appointmentRepository.GetAppointmentsByUserId(userId)).Returns(expectedAppointmentDTOs);

            // Act
            var result = await _appointmentController.GetAppointmentsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualAppointmentDTOs = Assert.IsType<List<AppointmentDTO>>(okResult.Value);
            Assert.Equal(expectedAppointmentDTOs.Count, actualAppointmentDTOs.Count);
        }

        [Fact]
        public async Task GetAppointmentsByUserId_NonExistingUserId_ReturnsOkResultWithNoAppointments()
        {
            // Arrange
            var userId = 1;
            var expectedAppointmentDTOs = new List<AppointmentDTO>();

            A.CallTo(() => _appointmentRepository.GetAppointmentsByUserId(userId)).Returns(expectedAppointmentDTOs);

            // Act
            var result = await _appointmentController.GetAppointmentsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualAppointmentDTOs = Assert.IsType<List<AppointmentDTO>>(okResult.Value);
            Assert.Empty(actualAppointmentDTOs);
        }    
    }
}