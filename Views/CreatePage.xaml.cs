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
    /// Interaction logic for CreatePage.xaml
    /// </summary>
    public partial class CreatePage : Page
    {
        public CreatePage()
        {
            InitializeComponent();
        }
        private void clearAll()
        {
            userName_Box.Clear();
            cnic_Box.Clear();
            emailBox.Clear();
            location.Clear();
            PasswordBox.Clear();
            if (Male.IsChecked == true || Female.IsChecked == true)
            {
                Male.IsChecked = false;
            }
            if (Female.IsChecked == true)
            {
                Female.IsChecked = false;
            }
        }
        private string checkGender()
        {
            if (Male.IsChecked == true)
            {
                return (string)Male.Content;
            }
            else if (Female.IsChecked == true)
            {
                return (string)Female.Content;
            }
            else
            {
                return "";
            }
        }

        private bool invalidInput()
        {
            string gender = checkGender();
            if (userName_Box.Text == "" || cnic_Box.Text == "" || emailBox.Text == "" || location.Text == "" || gender == "")
            {
                MessageBox.Show("Please Fill all Fields", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (!emailBox.Text.EndsWith("@gmail.com") || emailBox.Text.Length <= "@gmail.com".Length)
            {
                MessageBox.Show("Please Enter Valid Email like xyz@gmail.com", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (!userName_Box.Text.Take(2).All(char.IsLetter))
            {
                MessageBox.Show("Please Enter Valid Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            var dic = new Dictionary<TextBox, int> { { userName_Box, 6 }, { cnic_Box, 13 }, { emailBox, 8 }, { location, 3 } };
            foreach (var tdic in dic)
            {
                if (tdic.Value > tdic.Key.Text.Length)
                {
                    MessageBox.Show($"{tdic.Key.Name} must have atleast {tdic.Value} chracters", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            if (PasswordBox.Password.Length < 7)
            {
                MessageBox.Show("Password must have atleast 6 chracters ", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
        private async void CreateAccountButton(object sender, RoutedEventArgs e)
        {
            try
            {
                string gender = checkGender();
                if (invalidInput())
                {
                    return;
                }
                User newCustomer = new User(userName_Box.Text, PasswordBox.Password, cnic_Box.Text, gender, emailBox.Text, location.Text);
                if (await new UsersDAL().userNameExists(newCustomer.UserName))
                {
                    MessageBox.Show("UserName Already Exists Try another", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                   await new UsersDAL().insertUser(newCustomer);
                    NavigationService.Navigate(new ConsumerPage(newCustomer));
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Create Account Please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void TicketsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
