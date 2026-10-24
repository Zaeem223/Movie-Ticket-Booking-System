using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class Bookings_
    {
        public int CustomerId { get; set; }
        public int ShowId { get; set; }
        public int NoOfTickets { get; set; }
        public DateTime BookingTime { get; set; }

        public Bookings_(int userId, int showId, int noOfTickets, DateTime bookingTime)
        {
            CustomerId = userId;
            ShowId = showId;
            NoOfTickets = noOfTickets;
            BookingTime = bookingTime;
        }

    }
}
