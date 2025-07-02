using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;
using ReservationSystem_WebApp.Services;
using ReservationSystem_WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem_WebApp.Services.Tests
{
    [TestClass()]
    public class ReservationServiceTests
    {
        private readonly Mock<IGenericRepository<Reservation>> _mockRepo;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<Reservation>>();
            _service = new ReservationService(_mockRepo.Object, null, null);
        }

        [TestMethod()]
        public void GetReservation_WhenReservationExistsAndBelongsToUser_ReturnsReservation()
        {
            // Arrange
            var reservationId = 2;
            var userId = "8f1a06e7-fb59-4a81-868b-0f66096773c0";
            var expectedReservation = new Reservation
            {
                Id = reservationId,
                UserId = userId
            };

            _mockRepo.Setup(x => x.GetById(reservationId))
                .Returns(expectedReservation);

            // Act
            var result = _service.GetReservation(reservationId, userId);

            // Assert
            Assert.AreEqual(expectedReservation, result.reservation);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod()]
        public void GetReservation_WhenReservationDoesNotExist_ReturnsErrorMessage()
        {
            // Arrange
            var reservationId = 999;
            var userId = "8f1a06e7-fb59-4a81-868b-0f66096773c0";

            _mockRepo.Setup(x => x.GetById(reservationId))
                .Returns((Reservation)null);

            // Act
            var result = _service.GetReservation(reservationId, userId);

            // Assert
            Assert.IsNull(result.reservation);
            Assert.AreEqual("Reservation not found", result.ErrorMessage);
        }

        [TestMethod()]
        public void GetReservation_WhenReservationBelongsToAnotherUser_ReturnErrorMessage()
        {
            // Arrange
            var reservationId = 2;
            var correctUserId = "8f1a06e7-fb59-4a81-868b-0f66096773c0";
            var userId = "1";

            var existingReservation = new Reservation
            {
                Id = reservationId,
                UserId = correctUserId
            };

            _mockRepo.Setup(x => x.GetById(reservationId))
                .Returns(existingReservation);

            // Act
            var result = _service.GetReservation(reservationId, userId);

            // Assert
            Assert.AreEqual("Not user's reservation", result.ErrorMessage);
            Assert.IsNull(result.reservation);
        }

        [TestMethod()]
        public void AddReservation_WhenViewModelIsValid_ReturnsTrue()
        {
            // Arrange
            var modelId = 1;
            var modelTitle = "Title";
            var modelDescription = "Description";
            var beginDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var conferenceRoomId = 1;
            var purpose = "Purpose";
            var participants = 10;
            var userId = "8f1a06e7-fb59-4a81-868b-0f66096773c0";

            var reservation = new Reservation
            {
                Id = modelId,
                Title = modelTitle,
                BeginDate = beginDate,
                EndDate = endDate,
                ConferenceRoomId = conferenceRoomId,
                UserId= userId,
                Participants = participants,
                Purpose = purpose,
            };

            _mockRepo.Setup(x => x.Insert(reservation));

            // Act
            var reservationViewModel = new ReservationViewModel
            {
                Id = modelId,
                Title = modelTitle,
                BeginDate = beginDate,
                EndDate = endDate,
                ConferenceRoomId = conferenceRoomId,
                Participants = participants,
                Purpose = purpose,
            };
            var result = _service.AddReservation(reservationViewModel, userId);

            // Assert
            Assert.IsNull(result.ErrorMessage);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddReservation_WhenBeginDateIsInThePast_ReturnsFalseErrorMessage()
        {
            // Arrange
            var beginDate = DateTime.Now.AddDays(-1);
            var userId = "8f1a06e7-fb59-4a81-868b-0f66096773c0";

            var reservation = new Reservation
            {
                BeginDate = beginDate,
            };

            _mockRepo.Setup(x => x.Insert(reservation));

            // Act
            var reservationViewModel = new ReservationViewModel
            {
                BeginDate = beginDate
            };
            var result = _service.AddReservation(reservationViewModel, userId);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Start date cannot be in the past", result.ErrorMessage);
        }
        
    }
}