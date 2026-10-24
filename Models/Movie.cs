using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class Movie
    {

        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }

        // Constructor
        public Movie(string movieName, string genre, TimeSpan duration, string language, string description)
        {
            //MovieLookup = movieId;
            MovieName = movieName;
            Genre = genre;
            Duration = duration;
            Language = language;
            Description = description;
        }

        public Movie(int movieId, string movieName, string genre, TimeSpan duration, string language, string description)
        {
            MovieId = movieId;
            MovieName = movieName;
            Genre = genre;
            Duration = duration;
            Language = language;
            Description = description;
        }



    }
}
