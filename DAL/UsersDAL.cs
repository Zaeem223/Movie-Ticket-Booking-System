using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBookingApp.Models;

namespace TicketsBookingApp.DAL
{
    class UsersDAL
    {
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = _TicketsBookingSystem_; Integrated Security = True");


        public async Task insertUser(User newUser)
        {
            SqlCommand smd = new SqlCommand("INSERT INTO Users(UserName, CNIC, Password, Gender, Email, Location) " +
                       "VALUES (@UserName, @CNIC, @Password, @Gender, @Email, @Location)", con);
            await con.OpenAsync();
            smd.Parameters.AddWithValue("@UserName", newUser.UserName);
            smd.Parameters.AddWithValue("@CNIC", newUser.Cnic);
            smd.Parameters.AddWithValue("@Password", newUser.Password);
            smd.Parameters.AddWithValue("@Gender", newUser.Gender);
            smd.Parameters.AddWithValue("@Email", newUser.Email);
            smd.Parameters.AddWithValue("@Location", newUser.Location);
            await smd.ExecuteNonQueryAsync();
            con.Close();
        }

        public async Task<bool> userNameExists(string Username)
        {
            SqlCommand cmd = new SqlCommand("Select UserName FROM Users WhERE UserName = '" + Username + "' ", con);
            await con.OpenAsync();
            SqlDataReader sdr = await cmd.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }

        public async Task<User> getAllDetails(string username, string password)
        {
            User user = new User();
            string nameQuery = "SELECT UserName,UserId,Password,Role,CNIC,Gender,Location,Email FROM Users WHERE UserName = '" + username + "' AND Password ='" + password + "' ";
            SqlCommand smd = new SqlCommand(nameQuery, con);
            await con.OpenAsync();
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {

                user.UserId = Convert.ToInt32(sdr["UserId"]);
                user.UserName = sdr["UserName"].ToString();
                user.Password = sdr["Password"].ToString();
                user.Role = sdr["Role"].ToString();
                user.Cnic = sdr["Cnic"].ToString();
                user.Gender = sdr["Gender"].ToString();
                user.Email = sdr["Email"].ToString();
                user.Location = sdr["Location"].ToString();
            }
            sdr.Close();
            con.Close();
            return user;
        }
    }

    //public void updateName(string movieName, int movieId)
    //{
    //    SqlCommand smd = new SqlCommand("sp_UpdateMovieName", con);
    //    con.Open();
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.Parameters.AddWithValue("MovieLookup", movieId);
    //    smd.Parameters.AddWithValue("MovieName", movieName);
    //    smd.ExecuteNonQuery();
    //    con.Close();
    //}
    //public void updateGenre(string genre, int movieId)
    //{
    //    SqlCommand smd = new SqlCommand("sp_UpdateMovieGenre", con);
    //    con.Open();
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.Parameters.AddWithValue("MovieLookup", movieId);
    //    smd.Parameters.AddWithValue("Genre", genre);
    //    smd.ExecuteNonQuery();
    //    con.Close();
    //}
    //public void updateDuration(TimeSpan duration, int movieId)
    //{
    //    SqlCommand smd = new SqlCommand("sp_UpdateMovieDuration", con);
    //    con.Open();
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.Parameters.AddWithValue("MovieLookup", movieId);
    //    smd.Parameters.AddWithValue("Duration", duration);
    //    smd.ExecuteNonQuery();
    //    con.Close();
    //}
    //public void updateLanguage(string movieName, int movieId)
    //{
    //    SqlCommand smd = new SqlCommand("sp_UpdateMovieName", con);
    //    con.Open();
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.Parameters.AddWithValue("MovieLookup", movieId);
    //    smd.Parameters.AddWithValue("MovieName", movieName);
    //    smd.ExecuteNonQuery();
    //    con.Close();
    //}
    //public void updateDescription(string movieName, int movieId)
    //{
    //    SqlCommand smd = new SqlCommand("sp_UpdateMovieName", con);
    //    con.Open();
    //    smd.CommandType = CommandType.StoredProcedure;
    //    smd.Parameters.AddWithValue("MovieLookup", movieId);
    //    smd.Parameters.AddWithValue("MovieName", movieName);
    //    smd.ExecuteNonQuery();
    //    con.Close();
    //}
}
