using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicketsBookingApp.Models;

namespace TicketsBookingApp
{
    /// <summary>
    /// Interaction logic for BookingsReport.xaml
    /// </summary>
    public partial class BookingsReport : Page
    {
        public BookingsReport()
        {
            InitializeComponent();
            addBookings();
        }
        SqlConnection con = new SqlConnection(@"Data Source = localhost; Initial Catalog = TicketsBookingSystem; Integrated Security = True");

        public void addBookings()
        {
            List<BookingsDetails> myBookings = new List<BookingsDetails>();
            SqlCommand smd = new SqlCommand("sp_GetAllBookings", con);
            con.Open();
            smd.CommandType = CommandType.StoredProcedure;
            SqlDataReader sdr = smd.ExecuteReader();
            while (sdr.Read())
            {
                myBookings.Add(new BookingsDetails(Convert.ToInt32(sdr["BookingId"]), sdr["UserName"].ToString(),
                   sdr["CinemaName"].ToString(), sdr["MovieName"].ToString(), Convert.ToInt32(sdr["NoOfTickets"]),
                   Convert.ToDateTime(sdr["BDateTime"]),
                   Convert.ToDateTime(sdr["DateTime"]), Convert.ToDecimal(sdr["TotalPrice"])));
            }
            dgBookings.ItemsSource = myBookings;
            con.Close();
        }
        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
