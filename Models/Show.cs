using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class Show
    {
        public int MovieId { get; set; }
        public int ShowId { get; set; }
        public int CinemaId { get; set; }
        public DateTime BDateTime { get; set; }
        public int Tickets { get; set; }
        public decimal TicketPrice { get; set; }


        public Show(int movieId, int cinemaId, int tickets, decimal ticketPrice, DateTime bDateTime)
        {
            MovieId = movieId;
            CinemaId = cinemaId;
            BDateTime = bDateTime;
            Tickets = tickets;
            TicketPrice = ticketPrice;
        }
        public Show(DateTime bDateTime, int showId, int cinemaId, int tickets, decimal ticketPrice)
        {
            ShowId = showId;
            CinemaId = cinemaId;
            BDateTime = bDateTime;
            Tickets = tickets;
            TicketPrice = ticketPrice;
        }

    }
}
