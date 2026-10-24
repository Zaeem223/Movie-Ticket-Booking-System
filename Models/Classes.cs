using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicketsBookingApp
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

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Cnic { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }

        public User(string userName, string password,
                string cnic, string gender, string email, string location)
        {
            //UserId = userId;
            UserName = userName;
            Password = password;

            Cnic = cnic;
            Gender = gender;
            Email = email;
            Location = location;

        }
        public User()
        {

        }
    }

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
