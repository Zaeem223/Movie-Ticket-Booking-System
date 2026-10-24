using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class MovieLookup
    {
        public int movieId { get; set; }
        public string movieName { get; set; }

        public MovieLookup(int mi, string mn)
        {
            movieId = mi;
            movieName = mn;
        }
    }
}
