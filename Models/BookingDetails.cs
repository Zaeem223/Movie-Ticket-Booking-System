using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class BookingsDetails
    {
        public int BookingId { get; set; }

        public string UserName { get; set; }

        public string CinemaName { get; set; }

        public string MovieName { get; set; }

        public int NoOfTickets { get; set; }

        public DateTime ShowDateAndTime { get; set; }

        public DateTime BookingTime { get; set; }
        public decimal Price { get; set; }

        public BookingsDetails(int bookingId, string userName, string cinemaName, string movieName, int noOfTickets, DateTime showDateAndTime, DateTime bookingTime, decimal price)
        {
            BookingId = bookingId;
            UserName = userName;
            CinemaName = cinemaName;
            MovieName = movieName;
            NoOfTickets = noOfTickets;
            ShowDateAndTime = showDateAndTime;
            BookingTime = bookingTime;
            Price = price;
        }
    }
}
