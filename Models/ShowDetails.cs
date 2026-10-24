using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class ShowDetails
    {
        public int showId { get; set; }
        public int cinemaId { get; set; }
        public string showName { get; set; }
        public string CinemaName { get; set; }

        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public string Language { get; set; }
        public decimal ticketPrice { get; set; }
        public string Description { get; set; }
        public DateTime BDateTime { get; set; }
        public int Tickets { get; set; }
        public ShowDetails(string m, string C, int si, int ci, string genre, TimeSpan duration, string language, decimal TicketPrice, string description, DateTime bDateTime, int tickets)
        {
            showId = si;
            cinemaId = ci;
            showName = m;
            CinemaName = C;
            Genre = genre;
            Duration = duration;
            Language = language;
            ticketPrice = TicketPrice;
            Description = description;
            BDateTime = bDateTime;
            Tickets = tickets;
        }
        public ShowDetails()
        {

        }
    }
}
