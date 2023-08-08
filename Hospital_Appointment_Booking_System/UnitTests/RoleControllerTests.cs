using AutoMapper;
using Hospital_Appointment_Booking_System.Controllers;
using Hospital_Appointment_Booking_System.DTO;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;

namespace Hospital_Appointment_Booking_System.UnitTests.Controllers
{
    public class RoleControllerTests
    {
        private RoleController _roleController;
        private IRoleRepository _fakeRoleRepository;
        private IMapper _fakeMapper;

        public RoleControllerTests()
        {
            _fakeRoleRepository = A.Fake<IRoleRepository>();
            _fakeMapper = A.Fake<IMapper>();

            _roleController = new RoleController(_fakeRoleRepository, _fakeMapper);
        }

        private Role CreateSampleRole()
        {
            return new Role
            {
                RoleName = "Admin",
            };
        }

        [Fact]
        public async Task AddRole_ValidRole_ReturnsOkResult()
        {
            // Arrange
            var roleDto = new RoleDTO
            {
                RoleName = "Admin"
            };

            var role = CreateSampleRole();

            A.CallTo(() => _fakeMapper.Map<Role>(roleDto)).Returns(role);
            A.CallTo(() => _fakeRoleRepository.AddRole(role)).Returns(true);

            // Act
            var result = await _roleController.AddRole(roleDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var createdRoleDto = Assert.IsType<RoleDTO>(okResult.Value);
            Assert.Equal(roleDto.RoleName, createdRoleDto.RoleName);
        }

        [Fact]
        public async Task AddRole_DuplicateRole_ReturnsConflictResult()
        {
            // Arrange
            var roleDto = new RoleDTO
            {
                RoleName = "Admin",
            };

            var role = CreateSampleRole();
            A.CallTo(() => _fakeMapper.Map<Role>(roleDto)).Returns(role);
            A.CallTo(() => _fakeRoleRepository.AddRole(role)).Returns(false);

            // Act
            var result = await _roleController.AddRole(roleDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
            Assert.Equal("Role already exists.", conflictResult.Value);

        }

        [Fact]
        public async Task DeleteRole_ExistingRoleId_ReturnsOkResult()
        {
            // Arrange
            int roleId = 1;
            var role = CreateSampleRole();

            A.CallTo(() => _fakeRoleRepository.GetRoleById(roleId)).Returns(role);
            A.CallTo(() => _fakeRoleRepository.DeleteRole(roleId)).Returns(Task.CompletedTask);

            // Act
            var result = await _roleController.DeleteRole(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var deletedRoleId = Assert.IsType<int>(okResult.Value);
            Assert.Equal(roleId, deletedRoleId);
        }

        [Fact]
        public async Task DeleteRole_NonExistingRoleId_ReturnsNotFoundResult()
        {
            // Arrange
            int roleId = 5;

            A.CallTo(() => _fakeRoleRepository.GetRoleById(roleId)).Returns(Task.FromResult<Role>(null));

            // Act
            var result = await _roleController.DeleteRole(roleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetAllRoles_RolesExist_ReturnsOkResult()
        {
            // Arrange
            var roles = new List<Role>
        {
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Doctor" },
            new Role { RoleName = "Nurse" }
        };

            var roleDTOs = new List<RoleDTO>
        {
            new RoleDTO { RoleName = "Admin" },
            new RoleDTO { RoleName = "Doctor" },
            new RoleDTO { RoleName = "Nurse" }
        };

            A.CallTo(() => _fakeRoleRepository.GetAllRoles()).Returns(roles);
            A.CallTo(() => _fakeMapper.Map<List<RoleDTO>>(roles)).Returns(roleDTOs);

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<RoleDTO>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        [Fact]
        public async Task GetRoleById_ExistingRoleId_ReturnsOkResult()
        {
            // Arrange
            int roleId = 1;
            var role = CreateSampleRole();

            A.CallTo(() => _fakeRoleRepository.GetRoleById(roleId)).Returns(role);

            var roleDto = new RoleDTO
            {
                RoleName = role.RoleName,
            };

            A.CallTo(() => _fakeMapper.Map<RoleDTO>(role)).Returns(roleDto);

            // Act
            var result = await _roleController.GetRoleById(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var roleResult = Assert.IsType<RoleDTO>(okResult.Value);
            Assert.Equal(roleDto.RoleName, roleResult.RoleName);
        }

        [Fact]
        public async Task GetRoleById_NonExistingRoleId_ReturnsNotFoundResult()
        {
            // Arrange
            int roleId = 5;

            A.CallTo(() => _fakeRoleRepository.GetRoleById(roleId)).Returns(Task.FromResult<Role>(null));

            // Act
            var result = await _roleController.GetRoleById(roleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Role not found", notFoundResult.Value);
        }

    }
}