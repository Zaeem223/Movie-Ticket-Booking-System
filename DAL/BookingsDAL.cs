using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBookingApp.Models;

namespace TicketsBookingApp.DAL
{
    class BookingsDAL
    {
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = _TicketsBookingSystem_; Integrated Security = True");
        public async Task<List<BookingsDetails>> getMyBookings(int userid, int pageSize, int previousPages)
        {
            List<BookingsDetails> mybookings = new List<BookingsDetails>();
            SqlCommand smd = new SqlCommand("sp_GetMyBookings", con);
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("@UserId", userid);
            await con.OpenAsync();
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                mybookings.Add(new BookingsDetails(Convert.ToInt32(sdr["BookingId"]), sdr["UserName"].ToString(),
                    sdr["CinemaName"].ToString(), sdr["MovieName"].ToString(), Convert.ToInt32(sdr["NoOfTickets"]),
                    Convert.ToDateTime(sdr["BDateTime"]),
                    Convert.ToDateTime(sdr["DateTime"]), Convert.ToDecimal(sdr["TotalPrice"])));
            }
            con.Close();
            return mybookings;
        }

        public async Task<List<BookingsDetails>> getAllBookings(int pageSize, int previousPages)
        {
            List<BookingsDetails> allbookings = new List<BookingsDetails>();

            SqlCommand smd = new SqlCommand("sp_GetAllBookings", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
            SqlDataReader sdr = await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                allbookings.Add(new BookingsDetails(Convert.ToInt32(sdr["BookingId"]), sdr["UserName"].ToString(),
                   sdr["CinemaName"].ToString(), sdr["MovieName"].ToString(), Convert.ToInt32(sdr["NoOfTickets"]),
                   Convert.ToDateTime(sdr["BDateTime"]),
                   Convert.ToDateTime(sdr["DateTime"]), Convert.ToDecimal(sdr["TotalPrice"])));
            }
            return allbookings;
        }
        public async Task addBooking(Bookings_ newbooking)
        {
            SqlCommand smd = new SqlCommand("sp_BookTicket", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("UserId", newbooking.CustomerId);
            smd.Parameters.AddWithValue("ShowId", newbooking.ShowId);
            //smd.Parameters.AddWithValue("CinemaId", newbooking.CinemaId);
            smd.Parameters.AddWithValue("NoOfTickets", newbooking.NoOfTickets);
            smd.Parameters.AddWithValue("@BookingDateTime", newbooking.BookingTime);
            await smd.ExecuteNonQueryAsync();
            con.Close();
        }

        public async Task<List<string>> getSalesReport(DateTime startingDate, DateTime endDate)
        {
            SqlCommand smd = new SqlCommand("sp_SalesReport", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("Date1", startingDate);
            smd.Parameters.AddWithValue("Date2", endDate);
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            List<string> data = new List<string>();
            while  (await sdr.ReadAsync())
            {
                data.Add("Total Bookings : " + sdr["TotalBookings"].ToString());
                data.Add("Tickets Sold : " + sdr["TotalTickets"].ToString());
                data.Add("Sales : " + Convert.ToDecimal(sdr["TotalSales"]));
            }
            con.Close();
            return data;
        }

        //public async Task<List<BookingsDetails>> searchBookingsByMovie(string MovieName, List<BookingsDetails> Showslist)
        //{
        //    List<BookingsDetails> newlist = new List<BookingsDetails>();
        //    foreach (BookingsDetails templist in Showslist)
        //    {
        //        if (templist.MovieName == MovieName)
        //        {
        //            newlist.Add(templist);
        //        }
        //    }
        //    return newlist;
        //}
        public async Task<int> getRecentBookingId()
        {
            int bookingId = -1;
            SqlCommand smd = new SqlCommand("sp_getRecentaddedBookingId", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                bookingId = (int)sdr["BookingId"];
            }
            return bookingId;
        }
        public async Task<int> getBookingsCount()
        {
            SqlCommand sqc = new SqlCommand("sp_getBookingsCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
            await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
        public async Task<int> getSearchedBookingsCount(string MovieName, string CinemaName)
        {
            SqlCommand sqc = new SqlCommand("[sp_getSearchedBookingsCount]", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("MovieName", MovieName);
            sqc.Parameters.AddWithValue("CinemaName", CinemaName);
            await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
        public async Task<int> getMyBookingsCount(int userId)
        {
            SqlCommand sqc = new SqlCommand("sp_getMyBookingsCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("@UserId", userId);
            await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
        public async Task<int> getMySearchedBookingsCount(int userId, string MovieName, string CinemaName)
        {
            SqlCommand sqc = new SqlCommand("sp_getMySearchedBookingsCount", con);
            sqc.CommandType = CommandType.StoredProcedure;
            sqc.Parameters.AddWithValue("MovieName", MovieName);
            sqc.Parameters.AddWithValue("CinemaName", CinemaName);
            sqc.Parameters.AddWithValue("@UserId", userId);
            await con.OpenAsync();
            SqlDataReader sdr =await sqc.ExecuteReaderAsync();
            if (await sdr.ReadAsync())
            {
                return (int)sdr["TotalCount"];
            }
            return 0;
        }
        public async Task<List<BookingsDetails>> searchBookings(string MovieName, string CinemaName, int pageSize, int previousPages)
        {
            List<BookingsDetails> newlist = new List<BookingsDetails>();
            SqlCommand smd = new SqlCommand("sp_SearchBookings", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieName", MovieName);
            smd.Parameters.AddWithValue("CinemaName", CinemaName);
            smd.Parameters.AddWithValue("PageSize", pageSize);
            smd.Parameters.AddWithValue("PreviousPages", previousPages);
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                newlist.Add(new BookingsDetails(Convert.ToInt32(sdr["BookingId"]), sdr["UserName"].ToString(),
                   sdr["CinemaName"].ToString(), sdr["MovieName"].ToString(), Convert.ToInt32(sdr["NoOfTickets"]),
                   Convert.ToDateTime(sdr["BDateTime"]),
                   Convert.ToDateTime(sdr["DateTime"]), Convert.ToDecimal(sdr["TotalPrice"])));
            }
            con.Close();
            return newlist;
        }
        public async Task<List<BookingsDetails>> searchMyBookings(string MovieName, string CinemaName)
        {
            List<BookingsDetails> newlist = new List<BookingsDetails>();
            SqlCommand smd = new SqlCommand("[sp_SearchMyBookings]", con);
            await con.OpenAsync();
            smd.CommandType = CommandType.StoredProcedure;
            smd.Parameters.AddWithValue("MovieName", MovieName);
            smd.Parameters.AddWithValue("CinemaName", CinemaName);
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                newlist.Add(new BookingsDetails(Convert.ToInt32(sdr["BookingId"]), sdr["UserName"].ToString(),
                   sdr["CinemaName"].ToString(), sdr["MovieName"].ToString(), Convert.ToInt32(sdr["NoOfTickets"]),
                   Convert.ToDateTime(sdr["BDateTime"]),
                   Convert.ToDateTime(sdr["DateTime"]), Convert.ToDecimal(sdr["TotalPrice"])));
            }
            con.Close();
            return newlist;
        }
    }
}
