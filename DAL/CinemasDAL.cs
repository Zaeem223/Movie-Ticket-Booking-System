using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBookingApp.Models;

namespace TicketsBookingApp.DAL
{
    class CinemasDAL
    {
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = _TicketsBookingSystem_; Integrated Security = True");
        public async Task<List<CinemaLookup>> GetAllCinemasShortInfo()
        {
            List<CinemaLookup> cinemaslist = new List<CinemaLookup>();
            SqlCommand smd = new SqlCommand("Select CinemaName,CinemaId FROM Cinemas", con);
          await  con.OpenAsync();
            SqlDataReader sdr =await smd.ExecuteReaderAsync();
            while (await sdr.ReadAsync())
            {
                cinemaslist.Add(new CinemaLookup(Convert.ToInt32(sdr["CinemaId"]), sdr["CinemaName"].ToString()));
            }
            con.Close();
            return cinemaslist;
        }
    }
}
