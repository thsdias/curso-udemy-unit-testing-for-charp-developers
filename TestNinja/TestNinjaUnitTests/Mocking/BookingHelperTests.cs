using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinjaUnitTests.Mocking
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingsExistTests
    {
        private Booking _booking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void Setup()
        {
            var idBooking = 1;
            
            _booking = new Booking
            {
                Id = 2,
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "a"
            };
            
            _repository = new Mock<IBookingRepository>();
            _repository
                .Setup(r => r.GetActiveBookings(idBooking))
                .Returns(new List<Booking>
                    {
                        _booking
                    }.AsQueryable()
                );
        }

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = Before(_booking.ArrivalDate, 2),
                    DepartureDate = Before(_booking.ArrivalDate)
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.Empty);
        }
        
        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingsReference()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = Before(_booking.ArrivalDate),
                    DepartureDate = After(_booking.ArrivalDate)
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingsReference()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = Before(_booking.ArrivalDate),
                    DepartureDate = After(_booking.DepartureDate)
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        
        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingsReference()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = After(_booking.ArrivalDate),
                    DepartureDate = Before(_booking.DepartureDate)
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.EqualTo(_booking.Reference));
        }
        
        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBooking_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = After(_booking.DepartureDate),
                    DepartureDate = After(_booking.DepartureDate, days: 2)
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingOverlapButNewBookingIsCancelled_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist
            (
                new Booking
                {
                    Id = 1,
                    ArrivalDate = After(_booking.ArrivalDate),
                    DepartureDate = After(_booking.DepartureDate),
                    Status = "Cancelled"
                }, 
                _repository.Object
            );
            
            Assert.That(result, Is.Empty);
        }
        
        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }
        
        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }
        
        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}