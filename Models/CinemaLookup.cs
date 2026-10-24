using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class CinemaLookup
    {
        public int cinemaId { get; set; }
        public string cinemaName { get; set; }

        public CinemaLookup(int ci, string cn)
        {
            cinemaId = ci;
            cinemaName = cn;
        }
    }
}
