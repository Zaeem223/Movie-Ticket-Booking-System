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

namespace TicketsBookingApp
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void MoviesButton(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Movies());
        }

        private void ShowsButton(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShowsPage());
        }

        private void BookingsButton(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Bookings());

        }

        private void SignOutButton(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
