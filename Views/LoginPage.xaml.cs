using System;
using System.Collections.Generic;
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
using TicketsBookingApp.DAL;
using TicketsBookingApp.Models;

namespace TicketsBookingApp
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userName.Text == "" || PasswordBox.Password == "")
                {
                    MessageBox.Show("Please Enter All Fields", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (userName.Text.Length < 5 || PasswordBox.Password.Length < 5)
                {
                    MessageBox.Show("Please enter at least 5 characters", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                User currentUser = await new UsersDAL().getAllDetails(userName.Text, PasswordBox.Password);
                if (currentUser.UserId == 0 && currentUser.Password == null)
                {
                    MessageBox.Show("Invalid username or password.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (userName.Text != currentUser.UserName || PasswordBox.Password != currentUser.Password)
                {
                    MessageBox.Show("Invalid username or password.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (userName.Text == currentUser.UserName && PasswordBox.Password == currentUser.Password && currentUser.Role == "Customer")
                {
                    Check.Visibility = Visibility.Collapsed;
                    NavigationService.Navigate(new ConsumerPage(currentUser));
                }
                else if (userName.Text == currentUser.UserName && PasswordBox.Password == currentUser.Password && currentUser.Role == "Admin")
                {
                    Check.Visibility = Visibility.Collapsed;
                    NavigationService.Navigate(new AdminPage());
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Complete Login Please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreatePage());
        }
    }
}
