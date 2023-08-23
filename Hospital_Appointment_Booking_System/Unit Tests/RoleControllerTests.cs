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
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public RoleControllerTests()
        {
            _roleRepository = A.Fake<IRoleRepository>();
            _mapper = A.Fake<IMapper>();

            _roleController = new RoleController(_roleRepository, _mapper);
        }

        private Role CreateSampleRole()
        {
            return new Role
            {
                RoleId=1,
                RoleName = "Admin",
            };
        }

        [Fact]
        public async Task AddRole_ValidRole_ReturnsOkResult()
        {
            // Arrange
            var expectedRoleDto = new RoleDTO
            {
                RoleId = 1,
                RoleName = "Admin"
            };

            var role = CreateSampleRole();

            A.CallTo(() => _mapper.Map<Role>(expectedRoleDto)).Returns(role);
            A.CallTo(() => _roleRepository.AddRole(role)).Returns(true);

            // Act
            var result = await _roleController.AddRole(expectedRoleDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var actualRoleDto = Assert.IsType<RoleDTO>(okResult.Value);
            Assert.Equal(expectedRoleDto.RoleName, actualRoleDto.RoleName);
        }

        [Fact]
        public async Task AddRole_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var roleDto = new RoleDTO
            {
                RoleId = 1,
                RoleName = "Admin"
            };

            A.CallTo(() => _mapper.Map<Role>(roleDto)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _roleController.AddRole(roleDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddRole_DuplicateRole_ReturnsConflictResult()
        {
            // Arrange
            var roleDto = new RoleDTO
            {
                RoleId = 1,
                RoleName = "Admin",
            };

            var role = CreateSampleRole();
            A.CallTo(() => _mapper.Map<Role>(roleDto)).Returns(role);
            A.CallTo(() => _roleRepository.AddRole(role)).Returns(false);

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
            int expectedRoleId = 1;
            var role = CreateSampleRole();

            A.CallTo(() => _roleRepository.GetRoleById(expectedRoleId)).Returns(role);
            A.CallTo(() => _roleRepository.DeleteRole(expectedRoleId)).Returns(Task.CompletedTask);

            // Act
            var result = await _roleController.DeleteRole(expectedRoleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var actualRoleId = Assert.IsType<int>(okResult.Value);
            Assert.Equal(expectedRoleId, actualRoleId);
        }

        [Fact]
        public async Task DeleteRole_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int roleId = 1;

            A.CallTo(() => _roleRepository.GetRoleById(roleId)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _roleController.DeleteRole(roleId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRole_NonExistingRoleId_ReturnsNotFoundResult()
        {
            // Arrange
            int roleId = 5;

            A.CallTo(() => _roleRepository.GetRoleById(roleId)).Returns(Task.FromResult<Role>(null));

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

            A.CallTo(() => _roleRepository.GetAllRoles()).Returns(roles);
            A.CallTo(() => _mapper.Map<List<RoleDTO>>(roles)).Returns(roleDTOs);

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<RoleDTO>>>(result);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetAllRoles_RolesExist_ReturnsOkResultWithRoleDTOs()
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

            A.CallTo(() => _roleRepository.GetAllRoles()).Returns(roles);
            A.CallTo(() => _mapper.Map<List<RoleDTO>>(roles)).Returns(roleDTOs);

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<RoleDTO>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(okResult.Result);

            var returnedRoleDTOs = Assert.IsType<List<RoleDTO>>(okObjectResult.Value);
            Assert.Equal(roleDTOs.Count, returnedRoleDTOs.Count);

            for (int i = 0; i < roleDTOs.Count; i++)
            {
                Assert.Equal(roleDTOs[i].RoleName, returnedRoleDTOs[i].RoleName);
            }
        }

        [Fact]
        public async Task GetAllRoles_NullRoles_ReturnsNotFoundResult()
        {
            // Arrange
            List<Role> roles = null;

            A.CallTo(() => _roleRepository.GetAllRoles()).Returns(roles);

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task GetAllRoles_Exception_ReturnsInternalServerError()
        {
            // Arrange
            A.CallTo(() => _roleRepository.GetAllRoles()).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _roleController.GetAllRoles();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetRoleById_ExistingRoleId_ReturnsOkResult()
        {
            // Arrange
            int roleId = 1;
            var role = CreateSampleRole();

            A.CallTo(() => _roleRepository.GetRoleById(roleId)).Returns(role);

            var expectedRoleDto = new RoleDTO
            {
                RoleName = role.RoleName,
            };

            A.CallTo(() => _mapper.Map<RoleDTO>(role)).Returns(expectedRoleDto);

            // Act
            var result = await _roleController.GetRoleById(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var actualRoleDto = Assert.IsType<RoleDTO>(okResult.Value);
            Assert.Equal(expectedRoleDto.RoleName, actualRoleDto.RoleName);
        }

        [Fact]
        public async Task GetRoleById_Exception_ReturnsInternalServerError()
        {
            // Arrange
            int roleId = 1;

            A.CallTo(() => _roleRepository.GetRoleById(roleId)).Throws(new Exception("Simulated exception"));

            // Act
            var result = await _roleController.GetRoleById(roleId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetRoleById_NonExistingRoleId_ReturnsNotFoundResult()
        {
            // Arrange
            int roleId = 5;

            A.CallTo(() => _roleRepository.GetRoleById(roleId)).Returns(Task.FromResult<Role>(null));

            // Act
            var result = await _roleController.GetRoleById(roleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Role not found", notFoundResult.Value);
        }

    }
}