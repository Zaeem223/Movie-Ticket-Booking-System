using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicketsBookingApp.Models;

namespace TicketsBookingApp.DAL
{
    public class ShowsDAL
    {
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = _TicketsBookingSystem_; Integrated Security = True");
        public async Task addShow(Show newShow)
        {

            SqlCommand smd = new SqlCommand("sp_InsertShows", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("CinemaId", newShow.CinemaId);
            smd.Parameters.AddWithValue("MovieId", newShow.MovieId);
            smd.Parameters.AddWithValue("Tickets", newShow.Tickets);
            smd.Parameters.AddWithValue("TicketPrice", newShow.TicketPrice);
            smd.Parameters.Add("BDateTime", SqlDbType.DateTime2).Value = newShow.BDateTime;
          await  smd.ExecuteReaderAsync();
            con.Close();

        }

        public async Task updateShow(Show show)
        {
            SqlCommand smd = new SqlCommand("sp_UpdateShows", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("ShowId", show.ShowId);
            smd.Parameters.AddWithValue("CinemaId", show.CinemaId);
            smd.Parameters.AddWithValue("Tickets", show.Tickets);
            smd.Parameters.AddWithValue("TicketPrice", show.TicketPrice);
            smd.Parameters.Add("BDateTime", SqlDbType.DateTime2).Value = show.BDateTime;
         await   smd.ExecuteReaderAsync();
            con.Close();
        }

        public async Task deleteShow(int showId)
        {
            SqlCommand smd = new SqlCommand("sp_DeleteShow", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("ShowId", showId);
           await smd.ExecuteReaderAsync();
            con.Close();
        }

        public async Task<List<ShowDetails>> getAllShows(int pageSize, int previousPages)
        {
            List<ShowDetails> showslist = new List<ShowDetails>();
            SqlCommand sqc = new SqlCommand("sp_LoadShowsInPage", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("PageSize", pageSize);
            sqc.Parameters.AddWithValue("PreviousPages", previousPages);
            await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                showslist.Add(new ShowDetails(sdr["MovieName"].ToString(), sdr["CinemaName"].ToString(), Convert.ToInt32(sdr["ShowId"]), Convert.ToInt32(sdr["cinemaId"])
                    , sdr["Genre"].ToString(),
    (TimeSpan)sdr["Duration"],
    sdr["Language"].ToString(),
    Convert.ToDecimal(sdr["TicketPrice"]),
    sdr["Description"].ToString(),
    Convert.ToDateTime(sdr["BDateTime"]),
    Convert.ToInt32(sdr["Tickets"])));
            }
            con.Close();

            return showslist;
        }

        public async Task<List<ShowDetails>> searchShows(string MovieName, string CinemaName, string Genre, int pageSize, int previousPages)
        {
            List<ShowDetails> newlist = new List<ShowDetails>();
            SqlCommand smd = new SqlCommand("sp_SearchShows", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieName", MovieName);
            smd.Parameters.AddWithValue("CinemaName", CinemaName);
            smd.Parameters.AddWithValue("Genre", Genre);
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                newlist.Add(new ShowDetails(sdr["MovieName"].ToString(), sdr["CinemaName"].ToString(), Convert.ToInt32(sdr["ShowId"]), Convert.ToInt32(sdr["cinemaId"])
                    , sdr["Genre"].ToString(),
    (TimeSpan)sdr["Duration"],
    sdr["Language"].ToString(),
    Convert.ToDecimal(sdr["TicketPrice"]),
    sdr["Description"].ToString(),
    Convert.ToDateTime(sdr["BDateTime"]),
    Convert.ToInt32(sdr["Tickets"])));
            }
            con.Close();
            return newlist;
        }
        public async Task<int> getSearchedShowsCount(string MovieName, string CinemaName, string Genre)
        {
            SqlCommand sqc = new SqlCommand("sp_getSearchedShowsCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("MovieName", MovieName);
            sqc.Parameters.AddWithValue("CinemaName", CinemaName);
            sqc.Parameters.AddWithValue("Genre", Genre);
            await con.OpenAsync();
            SqlDataReader sdr = await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                int temp = Convert.ToInt32(sdr["TotalCount"]);
                return temp;
            }
            return 0;
        }
        public async Task<int> getShowsCount()
        {
            SqlCommand sqc = new SqlCommand("sp_getShowsCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
            await con.OpenAsync();
            SqlDataReader sdr = await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                int temp = Convert.ToInt32(sdr["TotalCount"]);
                return temp;
            }
            return 0;
        }
        public List<ShowDetails> searchShowsByMovie(string MovieName, List<ShowDetails> Showslist)
        {
            List<ShowDetails> newlist = new List<ShowDetails>();
            foreach (ShowDetails templist in Showslist)
            {
                if (templist.showName == MovieName)
                {
                    newlist.Add(templist);
                }
            }
            return newlist;
        }
        public List<ShowDetails> searchShowsByGenre(string Genre, List<ShowDetails> Showslist)
        {
            List<ShowDetails> newlist = new List<ShowDetails>();
            foreach (ShowDetails templist in Showslist)
            {
                if (templist.Genre == Genre)
                {
                    newlist.Add(templist);

                }
            }
            return newlist;

        }
        public List<ShowDetails> searchShowsByCinema(string CinemaName, List<ShowDetails> Showslist)
        {
            List<ShowDetails> newlist = new List<ShowDetails>();
            foreach (ShowDetails templist in Showslist)
            {
                if (templist.CinemaName == CinemaName)
                {
                    newlist.Add(templist);
                }
            }
            return newlist;
        }
        public async Task<bool> checkMovieInShows(int movieId)
        {
            SqlCommand smd = new SqlCommand("sp_CheckMovieInShows", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieId", movieId);
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return true;
            }
            con.Close();
            return false;
        }
        //public void insertShowCinema(int cinemaId)
        //{
        //    SqlCommand smd = new SqlCommand("sp_InsertShowCinema", con);
        //    con.Open();
        //    smd.CommandType = CommandType.StoredProcedure;
        //    smd.Parameters.AddWithValue("CinemaId", cinemaId);
        //    smd.ExecuteNonQuery();
        //    con.Close();
        //}
    }
}
