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
    public class SlotRepositoryTests
    {
       
        [Fact]
        public async Task GetAllSlots_SlotsExist_ReturnsListOfSlots()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetAllSlots_SlotsExist_ReturnsListOfSlots")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var slots = new List<Slot>
                {
                    new Slot { SlotDate = DateTime.Today, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) },
                    new Slot { SlotDate = DateTime.Today.AddDays(1), SlotStartTime = DateTime.Now.AddHours(2), SlotEndTime = DateTime.Now.AddHours(3) }
                };

                context.Slots.AddRange(slots);
                context.SaveChanges();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                // Act
                var result = await repository.GetAllSlots();

                // Assert
                Assert.Equal(slots.Count, result.Count());
                Assert.Equal(slots.Select(s => s.SlotDate), result.Select(s => s.SlotDate));
                Assert.Equal(slots.Select(s => s.SlotStartTime), result.Select(s => s.SlotStartTime));
                Assert.Equal(slots.Select(s => s.SlotEndTime), result.Select(s => s.SlotEndTime));
            }
        }

        [Fact]
        public async Task GetSlotById_ExistingId_ReturnsSlot()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "GetSlotById_ExistingId_ReturnsSlot")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var slot = new Slot { SlotDate = DateTime.Today, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) };

                context.Slots.Add(slot);
                context.SaveChanges();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                // Act
                var result = await repository.GetSlotById(slot.SlotId);

                // Assert
                Assert.Equal(slot.SlotDate, result.SlotDate);
                Assert.Equal(slot.SlotStartTime, result.SlotStartTime);
                Assert.Equal(slot.SlotEndTime, result.SlotEndTime);
            }
        }

        [Fact]
        public async Task AddSlot_ValidSlot_SuccessfullyAdded()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "AddSlot_ValidSlot_SuccessfullyAdded")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                var slotDto = new SlotDTO
                {
                    SlotDate = DateTime.Today.AddDays(2),
                    SlotStartTime = DateTime.Now.AddHours(4),
                    SlotEndTime = DateTime.Now.AddHours(5)
                };

                // Act
                await repository.AddSlot(slotDto);

                // Assert
                var addedSlot = context.Slots.FirstOrDefault();
                Assert.NotNull(addedSlot);
                Assert.Equal(slotDto.SlotDate, addedSlot.SlotDate);
                Assert.Equal(slotDto.SlotStartTime, addedSlot.SlotStartTime);
                Assert.Equal(slotDto.SlotEndTime, addedSlot.SlotEndTime);
            }
        }

        [Fact]
        public async Task UpdateSlot_ExistingSlot_ValidSlot_SuccessfullyUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "UpdateSlot_ExistingSlot_ValidSlot_SuccessfullyUpdated")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var slot = new Slot { SlotDate = DateTime.Today, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) };

                context.Slots.Add(slot);
                context.SaveChanges();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                var updatedSlotDto = new SlotDTO
                {
                    SlotId = slot.SlotId,
                    SlotDate = DateTime.Today.AddDays(1),
                    SlotStartTime = DateTime.Now.AddHours(2),
                    SlotEndTime = DateTime.Now.AddHours(3)
                };

                // Act
                await repository.UpdateSlot(updatedSlotDto);

                // Assert
                var updatedSlot = context.Slots.FirstOrDefault();
                Assert.NotNull(updatedSlot);
                Assert.Equal(updatedSlotDto.SlotDate, updatedSlot.SlotDate);
                Assert.Equal(updatedSlotDto.SlotStartTime, updatedSlot.SlotStartTime);
                Assert.Equal(updatedSlotDto.SlotEndTime, updatedSlot.SlotEndTime);
            }
        }

        [Fact]
        public async Task DeleteSlot_ExistingSlotId_SuccessfullyDeleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteSlot_ExistingSlotId_SuccessfullyDeleted")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var slot = new Slot { SlotDate = DateTime.Today, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) };

                context.Slots.Add(slot);
                context.SaveChanges();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                // Act
                await repository.DeleteSlot(slot.SlotId);

                // Assert
                var deletedSlot = context.Slots.FirstOrDefault();
                Assert.Null(deletedSlot);
            }
        }

        [Fact]
        public async Task DeleteSlot_InvalidSlotId_ThrowsArgumentException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Master_Hospital_ManagementContext>()
                .UseInMemoryDatabase(databaseName: "DeleteSlot_InvalidSlotId_ThrowsArgumentException")
                .Options;

            using (var context = new Master_Hospital_ManagementContext(options))
            {
                var slot = new Slot { SlotDate = DateTime.Today, SlotStartTime = DateTime.Now, SlotEndTime = DateTime.Now.AddHours(1) };

                context.Slots.Add(slot);
                context.SaveChanges();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
                var mapper = new Mapper(mapperConfig);

                var repository = new SlotRepository(context, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteSlot(-1));
            }
        }
    }
}
