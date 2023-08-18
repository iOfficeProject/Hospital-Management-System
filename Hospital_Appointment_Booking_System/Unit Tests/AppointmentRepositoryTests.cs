using Xunit;
using FakeItEasy;
using AutoMapper;
using Hospital_Appointment_Booking_System.Interfaces;
using Hospital_Appointment_Booking_System.Models;
using Hospital_Appointment_Booking_System.Repositories;
using Hospital_Appointment_Booking_System.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital_Appointment_Booking_System.Mapping;



namespace Hospital_Appointment_Booking_System.Unit_Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        [Fact]
        public async Task GetAllAppointments_ReturnsListOfAppointments()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAppointments_ReturnsListOfAppointments")
                .Options;



            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var appointments = new List<Appointment>
                {
                    new Appointment { AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) },
                    new Appointment { AppointmentDate = DateTime.Today.AddDays(1), AppointmentStartTime = DateTime.Now.AddHours(2), AppointmentEndTime = DateTime.Now.AddHours(3) }
                };



                context.Appointments.AddRange(appointments);
                context.SaveChanges();



                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);



                var repository = new AppointmentRepository(context, mapper);



                // Act
                var result = await repository.GetAllAppointments();



                // Assert
                Assert.Equal(appointments.Count, result.Count());
                Assert.Equal(appointments.Select(a => a.AppointmentDate), result.Select(a => a.AppointmentDate));
                Assert.Equal(appointments.Select(a => a.AppointmentStartTime), result.Select(a => a.AppointmentStartTime));
                Assert.Equal(appointments.Select(a => a.AppointmentEndTime), result.Select(a => a.AppointmentEndTime));
            }
        }



        [Fact]
        public async Task GetAppointmentById_ExistingId_ReturnsAppointment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetAppointmentById_ExistingId_ReturnsAppointment")
                .Options;



            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var appointment = new Appointment { AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };



                context.Appointments.Add(appointment);
                context.SaveChanges();



                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);



                var repository = new AppointmentRepository(context, mapper);



                // Act
                var result = await repository.GetAppointmentById(appointment.AppointmentId);



                // Assert
                Assert.Equal(appointment.AppointmentDate, result.AppointmentDate);
                Assert.Equal(appointment.AppointmentStartTime, result.AppointmentStartTime);
                Assert.Equal(appointment.AppointmentEndTime, result.AppointmentEndTime);
            }
        }



        /*  [Fact]
          public async Task AddAppointment_ValidAppointment_SuccessfullyAdded()
          {
              // Arrange
              var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                  .UseInMemoryDatabase(databaseName: "AddAppointment_ValidAppointment_SuccessfullyAdded")
                  .Options;



              using (var context = new Master_Hospital_ManagementContext(options))
              {
                  var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                  var mapper = new Mapper(mapperConfig);



                  var repository = new AppointmentRepository(context, mapper);



                  var appointmentDto = new AppointmentInputDTO
                  {
                      AppointmentDate = DateTime.Today,
                      AppointmentStartTime = DateTime.Now,
                      AppointmentEndTime = DateTime.Now.AddHours(1),
                      SlotId = 1,
                      HospitalId = 1,
                      UserId = 1
                  };



                  // Act
                  await repository.AddAppointment(appointmentDto);



                  // Assert
                  var addedAppointment = context.Appointments.FirstOrDefault();
                  Assert.NotNull(addedAppointment);
                  Assert.Equal(appointmentDto.AppointmentDate, addedAppointment.AppointmentDate);
                  Assert.Equal(appointmentDto.AppointmentStartTime, addedAppointment.AppointmentStartTime);
                  Assert.Equal(appointmentDto.AppointmentEndTime, addedAppointment.AppointmentEndTime);
              }
          }*/



        /*   [Fact]
           public async Task UpdateAppointment_ExistingId_ValidAppointment_SuccessfullyUpdated()
           {
               // Arrange
               var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                   .UseInMemoryDatabase(databaseName: "UpdateAppointment_ExistingId_ValidAppointment_SuccessfullyUpdated")
                   .Options;



               using (var context = new Master_Hospital_ManagementContext(options))
               {
                   var appointment = new Appointment { AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };



                   context.Appointments.Add(appointment);
                   context.SaveChanges();



                   var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                   var mapper = new Mapper(mapperConfig);



                   var repository = new AppointmentRepository(context, mapper);



                   var updatedAppointmentDto = new AppointmentInputDTO
                   {
                       AppointmentId = appointment.AppointmentId,
                       AppointmentDate = DateTime.Today.AddDays(1),
                       AppointmentStartTime = DateTime.Now.AddHours(2),
                       AppointmentEndTime = DateTime.Now.AddHours(3),
                       SlotId = 2,
                       HospitalId = 2,
                       UserId = 2
                   };



                   // Act
                   await repository.UpdateAppointment(updatedAppointmentDto);



                   // Assert
                   var updatedAppointment = context.Appointments.FirstOrDefault();
                   Assert.NotNull(updatedAppointment);
                   Assert.Equal(updatedAppointmentDto.AppointmentDate, updatedAppointment.AppointmentDate);
                   Assert.Equal(updatedAppointmentDto.AppointmentStartTime, updatedAppointment.AppointmentStartTime);
                   Assert.Equal(updatedAppointmentDto.AppointmentEndTime, updatedAppointment.AppointmentEndTime);
                   Assert.Equal(updatedAppointmentDto.SlotId, updatedAppointment.SlotId);
                   Assert.Equal(updatedAppointmentDto.HospitalId, updatedAppointment.HospitalId);
                   Assert.Equal(updatedAppointmentDto.UserId, updatedAppointment.UserId);
               }
           }*/



        [Fact]
        public async Task DeleteAppointment_ExistingId_SuccessfullyDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAppointment_ExistingId_SuccessfullyDeleted")
                .Options;



            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var appointment = new Appointment { AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1) };



                context.Appointments.Add(appointment);
                context.SaveChanges();



                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);



                var repository = new AppointmentRepository(context, mapper);



                // Act
                await repository.DeleteAppointment(appointment.AppointmentId);



                // Assert
                var deletedAppointment = context.Appointments.FirstOrDefault();
                Assert.Null(deletedAppointment);
            }
        }



        [Fact]
        public async Task GetAppointmentsByUserId_ExistingUserId_ReturnsListOfAppointments()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetAppointmentsByUserId_ExistingUserId_ReturnsListOfAppointments")
                .Options;



            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var userId = 1;
                var appointments = new List<Appointment>
                {
                    new Appointment { AppointmentDate = DateTime.Today, AppointmentStartTime = DateTime.Now, AppointmentEndTime = DateTime.Now.AddHours(1), UserId = userId },
                    new Appointment { AppointmentDate = DateTime.Today.AddDays(1), AppointmentStartTime = DateTime.Now.AddHours(2), AppointmentEndTime = DateTime.Now.AddHours(3), UserId = userId }
                };



                context.Appointments.AddRange(appointments);
                context.SaveChanges();



                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);



                var repository = new AppointmentRepository(context, mapper);



                // Act
                var result = await repository.GetAppointmentsByUserId(userId);



                // Assert
                Assert.Equal(appointments.Count, result.Count());
                Assert.Equal(appointments.Select(a => a.AppointmentDate), result.Select(a => a.AppointmentDate));
                Assert.Equal(appointments.Select(a => a.AppointmentStartTime), result.Select(a => a.AppointmentStartTime));
                Assert.Equal(appointments.Select(a => a.AppointmentEndTime), result.Select(a => a.AppointmentEndTime));
                Assert.Equal(appointments.Select(a => a.UserId), result.Select(a => a.UserId));
            }
        }
    }
}