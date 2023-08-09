using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hospital_Appointment_Booking_System.Tests.Controllers
{
    public class SpecializationControllerTests
    {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IMapper _mapper;
        private readonly SpecializationController _controller;

        public SpecializationControllerTests()
        {
            _specializationRepository = A.Fake<ISpecializationRepository>();
            _mapper = A.Fake<IMapper>();
            _controller = new SpecializationController(_specializationRepository);

        }

        [Fact]
        public async Task TestGetAllSpecializations_ReturnsOkResultWithSpecializations()
        {
            // Arrange
            var expectedSpecializations = new List<SpecializationDTO>
            {
                new SpecializationDTO { SpecializationId = 1, SpecializationName = "Cardio"},
                new SpecializationDTO { SpecializationId = 2, SpecializationName = "Dental Care", }
            };

            
            A.CallTo(() => _specializationRepository.GetAllSpecializations()).Returns(expectedSpecializations);

            // Act
            var result = await _controller.GetAllSpecializations();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var actualSpecializations = Assert.IsType<List<SpecializationDTO>>(okResult.Value);
            Assert.Equal(expectedSpecializations.Count, actualSpecializations.Count);
            Assert.Equal(expectedSpecializations[0].SpecializationId, actualSpecializations[0].SpecializationId);
            Assert.Equal(expectedSpecializations[1].SpecializationName, actualSpecializations[1].SpecializationName);
        }

        [Fact]
        public async Task TestGetAllSpecializations_ReturnsEmptyResult()
        {
            // Arrange
            A.CallTo(() => _specializationRepository.GetAllSpecializations()).Returns(new List<SpecializationDTO>());

            // Act
            var result = await _controller.GetAllSpecializations();

            // Assert
            Assert.IsType<OkObjectResult>(result);          
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var actualSpecializations = Assert.IsType<List<SpecializationDTO>>(okResult.Value);
            Assert.Empty(actualSpecializations);
        }

        [Fact]
        public async Task GetSpecializationsByHospitalId_ReturnsListOfSpecializationDTOs()
        {
            // Arrange
            int hospitalId = 1;
            var expectedSpecializations = new List<SpecializationDTO>
            {
                new SpecializationDTO { SpecializationId = 1, SpecializationName = "Cardio" },
                new SpecializationDTO { SpecializationId = 2, SpecializationName = "Dental Care" }
            };
            A.CallTo(() => _specializationRepository.GetSpecializationsByHospitalId(hospitalId)).Returns(expectedSpecializations);

            // Act
            var result = await _controller.GetSpecializationsByHospitalId(hospitalId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSpecializations = Assert.IsType<List<SpecializationDTO>>(okResult.Value);
            Assert.Equal(expectedSpecializations.Count, actualSpecializations.Count);
            Assert.Equal(expectedSpecializations[0].SpecializationId, actualSpecializations[0].SpecializationId);
            Assert.Equal(expectedSpecializations[1].SpecializationName, actualSpecializations[1].SpecializationName);
        }

        [Fact]
        public async Task GetSpecializationById_ReturnsSpecializationDTO()
        {
            // Arrange
            int specializationId = 1;
            var expectedSpecialization = new SpecializationDTO { SpecializationId = specializationId, SpecializationName = "Cardio" };
            A.CallTo(() => _specializationRepository.GetSpecializationById(specializationId)).Returns(expectedSpecialization);

            // Act
            var result = await _controller.GetSpecializationById(specializationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSpecialization = Assert.IsType<SpecializationDTO>(okResult.Value);
            Assert.Equal(expectedSpecialization.SpecializationId, returnedSpecialization.SpecializationId);
            Assert.Equal(expectedSpecialization.SpecializationName, returnedSpecialization.SpecializationName);
        }

        [Fact]
        public async Task AddSpecialization_ReturnsOkResult()
        {
            // Arrange
            var specializationDto = new SpecializationDTO
            {
                SpecializationName = "Dental Care"
            };
            A.CallTo(() => _specializationRepository.AddSpecialization(specializationDto)).Returns(true);

            // Act
            var result = await _controller.AddSpecialization(specializationDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateSpecialization_ReturnsOkResult()
        {
            // Arrange
            int specializationId = 1;
            var specializationDto = new SpecializationDTO
            {
                SpecializationId = specializationId,
                SpecializationName = "Cardio"
            };
            A.CallTo(() => _specializationRepository.UpdateSpecialization(specializationDto));

            // Act
            var result = await _controller.UpdateSpecialization(specializationId, specializationDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteSpecialization_ReturnsOkResult()
        {
            // Arrange
            int specializationId = 1;
            A.CallTo(() => _specializationRepository.DeleteSpecialization(specializationId));

            // Act
            var result = await _controller.DeleteSpecialization(specializationId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }
    }
}
