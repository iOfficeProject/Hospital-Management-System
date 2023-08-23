using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hospital_Appointment_Booking_System.UnitTests
{
    public class HospitalControllerTests
    {
        private readonly IHospitalRepository _fakeHospitalRepository;
        private readonly IMapper _fakeMapper;
        private readonly HospitalController _controller;

        public HospitalControllerTests()
        {
            _fakeHospitalRepository = A.Fake<IHospitalRepository>();
            _fakeMapper = A.Fake<IMapper>();

            _controller = new HospitalController(_fakeHospitalRepository, _fakeMapper);
        }

        [Fact]
        public async Task GetHospitals_ReturnsOkResultWithHospitals()
        {
            // Arrange
            var expectedHospitals = new List<HospitalDTO>
            {
                new HospitalDTO { HospitalName = "Ruby", Location = "Pune" },
                new HospitalDTO { HospitalName = "Metro", Location = "Kolhapur" }
            };

            A.CallTo(() => _fakeHospitalRepository.GetAllHospital()).Returns(expectedHospitals);

            // Act
            var result = await _controller.GetHospitals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var actualHospitals = Assert.IsType<List<HospitalDTO>>(okResult.Value);
            Assert.Equal(expectedHospitals.Count, actualHospitals.Count);
            Assert.Equal(expectedHospitals[0].HospitalName, actualHospitals[0].HospitalName);
            Assert.Equal(expectedHospitals[1].Location, actualHospitals[1].Location);
        }

        [Fact]
        public async Task GetHospital_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            A.CallTo(() => _fakeHospitalRepository.GetByIdHospital(nonExistingId)).Returns(Task.FromResult<HospitalDTO>(null));

            // Act
            var result = await _controller.GetHospital(nonExistingId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllHospitals_ReturnsEmptyResult()
        {
            // Arrange
            A.CallTo(() => _fakeHospitalRepository.GetAllHospital()).Returns(new List<HospitalDTO>());

            // Act
            var result = await _controller.GetHospitals();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var actualHospitals = Assert.IsType<List<HospitalDTO>>(okResult.Value);
            Assert.Empty(actualHospitals);
        }


        [Fact]
        public async Task GetHospitalById_ReturnsHospitalDTO()
        {
            // Arrange
            int hospitalId = 1;
            var expectedHospital = new HospitalDTO { HospitalId = hospitalId, HospitalName = "Ruby", Location="Pune" };
            A.CallTo(() => _fakeHospitalRepository.GetByIdHospital(hospitalId)).Returns(expectedHospital);

            // Act
            var result = await _controller.GetHospital(hospitalId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHospitals = Assert.IsType<HospitalDTO>(okResult.Value);
            Assert.Equal(expectedHospital.HospitalId, returnedHospitals.HospitalId);
            Assert.Equal(expectedHospital.HospitalName, returnedHospitals.HospitalName);
            Assert.Equal(expectedHospital.Location, returnedHospitals.Location);

        }

        [Fact]
        public async Task CreateHospital_WithValidDto_ReturnsOkResult()
        {
            // Arrange
            var hospitalDto = new HospitalDTO { HospitalName = "Metro Hospital",Location="Pune"};
            A.CallTo(() => _fakeHospitalRepository.AddHospital(hospitalDto)).Returns(true);

            // Act
            var result = await _controller.CreateHospital(hospitalDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateHospital_WithExistingName_ReturnsConflictResult()
        {
            // Arrange
            var hospitalDto = new HospitalDTO { HospitalName = "Metro Hospital", Location = "Pune" };

            A.CallTo(() => _fakeHospitalRepository.AddHospital(hospitalDto)).Returns(false);

            // Act
            var result = await _controller.CreateHospital(hospitalDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
            Assert.Equal("Hospital name already exists.", conflictResult.Value);
        }

        [Fact]
        public async Task UpdateHospital_WithValidData_ReturnsOkResultWithUpdatedHospital()
        {
            // Arrange
            int fakeHospitalId = 1;
            var fakeHospitalDTO = new HospitalDTO { HospitalName = "Updated Ruby", Location = "Updated Pune" };

            A.CallTo(() => _fakeHospitalRepository.UpdateHospital(fakeHospitalId, fakeHospitalDTO))
                .Returns(true);

            A.CallTo(() => _fakeHospitalRepository.GetByIdHospital(fakeHospitalId))
                .Returns(fakeHospitalDTO);

            // Act
            var result = await _controller.UpdateHospital(fakeHospitalId, fakeHospitalDTO);

            // Assert
            Assert.IsType<ActionResult<HospitalDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var updatedHospitalDTO = Assert.IsType<HospitalDTO>(okResult.Value);
            Assert.Equal(fakeHospitalDTO.HospitalName, updatedHospitalDTO.HospitalName);
            Assert.Equal(fakeHospitalDTO.Location, updatedHospitalDTO.Location);
        }
        [Fact]
        public async Task UpdateHospital_WithValidDataAndNoHospitalFound_ReturnsNotFoundResult()
        {
            // Arrange
            int fakeHospitalId = 1;
            var fakeHospitalDTO = new HospitalDTO { HospitalName = "Updated Ruby", Location = "Updated Pune" };

            A.CallTo(() => _fakeHospitalRepository.UpdateHospital(fakeHospitalId, fakeHospitalDTO))
                .Returns(true);

            A.CallTo(() => _fakeHospitalRepository.GetByIdHospital(fakeHospitalId))
                .Returns(Task.FromResult<HospitalDTO>(null));

            // Act
            var result = await _controller.UpdateHospital(fakeHospitalId, fakeHospitalDTO);

            // Assert
            var notFoundResult = Assert.IsType<ActionResult<HospitalDTO>>(result);
            notFoundResult.Result.Should().BeOfType<NotFoundResult>();

        }


        [Fact]
        public async Task DeleteHospital_ReturnsOkResult()
        {
            // Arrange
            int hospitalId = 1;
            A.CallTo(() => _fakeHospitalRepository.DeleteHospital(hospitalId));

            // Act
            var result = await _controller.DeleteHospital(hospitalId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Hospital is deleted.", okResult.Value);

        }

    }
}
