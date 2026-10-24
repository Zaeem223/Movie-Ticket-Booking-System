using System;
using System.Collections.Generic;
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
    /// Interaction logic for TicketReciept.xaml
    /// </summary>
    public partial class TicketReciept : Page
    {
        public TicketReciept(BookingsDetails CurrentBooking)
        {
            InitializeComponent();
            currentBooking = CurrentBooking;
            loadRecipt();
        }
        private void loadRecipt()
        {
            MessageBox.Show("Booking Succesfull", "Done", MessageBoxButton.OK,MessageBoxImage.Information);
            BookingIdTextBlock.Text =  currentBooking.BookingId.ToString();
            CustomerTextBlock.Text = currentBooking.UserName;
            CinemaTextBlock.Text = currentBooking.CinemaName;
            ShowDateTextBlock.Text = currentBooking.ShowDateAndTime.ToString();
            TicketsTextBlock.Text = currentBooking.NoOfTickets.ToString();
            PriceTextBlock.Text = currentBooking.Price.ToString();
            BookingonTextBlock.Text = DateTime.Now.ToString();
            ShowNameTextBlock.Text = currentBooking.MovieName;
        }
        BookingsDetails currentBooking;
        private void Download_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Downloaded Succesfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
