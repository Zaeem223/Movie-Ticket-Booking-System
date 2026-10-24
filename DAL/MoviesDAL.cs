using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicketsBookingApp.Models;

namespace TicketsBookingApp.NewFolder1
{
    public class MoviesDAL
    {
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = _TicketsBookingSystem_; Integrated Security = True");

        public async Task insertMovie(Movie movie)
        {

            SqlCommand smd = new SqlCommand("sp_InsertMovies", con);
           await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieName", movie.MovieName);
            smd.Parameters.AddWithValue("genre", movie.Genre);
            smd.Parameters.AddWithValue("duration", movie.Duration);
            smd.Parameters.AddWithValue("MLanguage", movie.Language);
            smd.Parameters.AddWithValue("MDescription", movie.Description);
         await   smd.ExecuteReaderAsync();
            con.Close();
        }

        public async Task updateMovie(Movie movie)
        {
            SqlCommand cmd = new SqlCommand("sp_UpdateMovies", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MovieId", movie.MovieId);
            cmd.Parameters.AddWithValue("@MovieName", movie.MovieName);
            cmd.Parameters.AddWithValue("@Genre", movie.Genre);
            cmd.Parameters.AddWithValue("@Duration", movie.Duration);
            cmd.Parameters.AddWithValue("@MLanguage", movie.Language);
            cmd.Parameters.AddWithValue("@MDescription", movie.Description);

           await con.OpenAsync();
           await cmd.ExecuteReaderAsync();
            con.Close();
        }

        public async Task removeMovie(int movieID)
        {
            SqlCommand smd = new SqlCommand("sp_ChangeMovieStatus", con);
          await  con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieId", movieID);
           await smd.ExecuteReaderAsync();
            con.Close();
        }
        public async Task<List<Movie>> getAllMovies(int pageSize, int previousPages)
        {
            List<Movie> moviesist = new List<Movie>();
            SqlCommand smd = new SqlCommand("sp_getAllMovies", con);
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
          await  con.OpenAsync();
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                moviesist.Add(new Movie(Convert.ToInt32(sdr["MovieId"]), sdr["MovieName"].ToString(),
                    sdr["Genre"].ToString(), (TimeSpan)sdr["Duration"], sdr["Language"].ToString(),
                    sdr["Description"].ToString()));
            }
            con.Close();
            return moviesist;
        }

        public async Task<List<MovieLookup>> GetAllMoviesShortInfo()
        {
            List<MovieLookup> movieslist = new List<MovieLookup>();
            SqlCommand smd = new SqlCommand("Select MovieId,MovieName FROM Movies where status = 1", con);
           await con.OpenAsync();
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                movieslist.Add(new MovieLookup(Convert.ToInt32(sdr["MovieId"]), sdr["MovieName"].ToString()));
            }
            con.Close();
            return movieslist;
        }

        public List<Movie> searchMoviesByName(string MovieName, List<Movie> Showslist)
        {
            List<Movie> newlist = new List<Movie>();
            foreach (Movie templist in Showslist)
            {
                if (templist.MovieName == MovieName)
                {
                    newlist.Add(templist);
                }
            }
            return newlist;
        }
        public async Task<List<Movie>> searchMovies(string movieName, string genre, int pageSize, int previousPages)
        {
            List<Movie> newlist = new List<Movie>();
            SqlCommand smd = new SqlCommand("sp_SearchMovies", con);
           await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieName", movieName);
            smd.Parameters.AddWithValue("Genre", genre);
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                newlist.Add(new Movie(Convert.ToInt32(sdr["MovieId"]), sdr["MovieName"].ToString(),
                     sdr["Genre"].ToString(), (TimeSpan)sdr["Duration"], sdr["Language"].ToString(),
                     sdr["Description"].ToString()));
            }
            con.Close();
            return newlist;
        }
        public async Task<int> getMoviesCount()
        {
            SqlCommand sqc = new SqlCommand("sp_getMoviesCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
           await  con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
        public async Task<int> getSearchedMoviesCount(string movieName, string genre)
        {
            SqlCommand sqc = new SqlCommand("[sp_getSearchedMoviesCount]", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("MovieName", movieName);
            sqc.Parameters.AddWithValue("Genre", genre);
           await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
    }
}
