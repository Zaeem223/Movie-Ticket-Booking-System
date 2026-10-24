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
using TicketsBookingApp.DAL;
using TicketsBookingApp.Models;

namespace TicketsBookingApp
{
    public partial class MyBookingsList : Page
    {
        User CurrentUser;

        public MyBookingsList(User user)
        {
            InitializeComponent();
            CurrentUser = user;
        }
        private bool searchMode = false;
        private int CurrentPageNo = 1;
        public int currentPageNo
        {
            get => CurrentPageNo;
            set
            {
                CurrentPageNo = value;
                PagesCount.Content = $"{currentPageNo} of {TotalPages}";
            }
        }

        private int totalPages = 1;
        public int TotalPages
        {
            get => totalPages;
            set
            {
                totalPages = value;
                PagesCount.Content = $"{currentPageNo} of {TotalPages}";
            }
        }
        private async void myBookings()
        {
            UserNameLabel.Content = "👤" + CurrentUser.UserName;
            calculateTotalPages();
            int pageSize = 24;
            int previousPages = (currentPageNo - 1) * pageSize;
            try
            {
                dgMyBookingsList.ItemsSource =await  new BookingsDAL().getMyBookings(CurrentUser.UserId, pageSize, previousPages);
                if (dgMyBookingsList.Items.Count == 0)
                {
                    MessageBox.Show("You have no previous Bookings.", "Not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    NavigationService.GoBack();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void calculateTotalPages()
        {
            try
            {
                int totalShows =await new BookingsDAL().getMyBookingsCount(CurrentUser.UserId);
                totalPages = totalShows / 24;
                if (TotalPages == 0 || totalShows % 24 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void calculateTotalPagesOfSearchedList(string MovieName, string CinemaName)
        {
            try
            {
                int totalShows =await new BookingsDAL().getSearchedBookingsCount(MovieName, CinemaName);
                TotalPages = totalShows / 24;
                if (TotalPages == 0 || totalShows % 24 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void SearchBookings()
        {
            try
            {
                int previousPages = (currentPageNo - 1) * 24;
                string CinemaName = "";
                if (CinemaCombo.SelectedIndex != -1)
                {
                    CinemaName = ((ComboBoxItem)CinemaCombo.SelectedItem).Content.ToString();
                }
                dgMyBookingsList.ItemsSource =await new BookingsDAL().searchBookings(MoviesTextBox.Text, CinemaName, 24, previousPages);
                calculateTotalPagesOfSearchedList(MoviesTextBox.Text, CinemaName);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchMode = true;
            currentPageNo = 1;
            TotalPages = 1;
            if (MoviesTextBox.Text == "" && CinemaCombo.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Search Item First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SearchBookings();
            }
        }
        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string CinemaName = "";
        //    if (CinemaCombo.SelectedIndex != -1)
        //    {
        //        CinemaName = ((ComboBoxItem)CinemaCombo.SelectedItem).Content.ToString();
        //    }

        //    if (MoviesTextBox.Text == "" && CinemaCombo.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Please Select Search Item First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //    else
        //    {
        //        //dgMyBookingsList.ItemsSource = new BookingsDAL().searchBookings(MoviesTextBox.Text, CinemaName);
        //    }
        //}
        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {

            CinemaCombo.SelectedIndex = -1;
            MoviesTextBox.Clear();
            searchMode = false;
            currentPageNo = 1;
            TotalPages = 1;
            myBookings();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myBookings();
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void decButton_Click_1(object sender, RoutedEventArgs e)
        {

            if (currentPageNo != 1 && searchMode == true)
            {
                currentPageNo--;
                SearchBookings();
            }
            if (currentPageNo != 1 && searchMode == false)
            {
                currentPageNo--;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                myBookings();
            }
        }

        private void IncButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(dgMyBookingsList.Items.Count < 24) && (currentPageNo != totalPages) && searchMode == true)
            {
                currentPageNo++;
                SearchBookings();
            }
            if (!(dgMyBookingsList.Items.Count < 24) && (currentPageNo != totalPages) && searchMode == false)
            {
                currentPageNo++;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                myBookings();
            }
        }

     

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }


    }
}
