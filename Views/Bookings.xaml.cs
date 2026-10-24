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

namespace TicketsBookingApp
{

    public partial class Bookings : Page
    {
        public Bookings()
        {
            InitializeComponent();
            loadBookings();
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
        private async void loadBookings()
        {
            calculateTotalPages();
            int pageSize = 22;
            int previousPages = (currentPageNo - 1) * pageSize;
            try
            {
                dgBookings.ItemsSource =await new BookingsDAL().getAllBookings(pageSize, previousPages);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Bookings Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                int totalShows =await new BookingsDAL().getBookingsCount();
                totalPages = totalShows / 22;
                if (TotalPages == 0 || totalShows % 22 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Bookings Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                TotalPages = totalShows / 22;
                if (TotalPages == 0 || totalShows % 22 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Bookings Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SalesReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StartingDate.SelectedDate == null || EndingDate.SelectedDate == null)
                {
                    MessageBox.Show("Please Choose Dates First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (StartingDate.SelectedDate <= EndingDate.SelectedDate)
                    {
                        List<string> salesReport =await new BookingsDAL().getSalesReport((DateTime)StartingDate.SelectedDate, (DateTime)EndingDate.SelectedDate);

                        BookingsLabel.Content = salesReport[0];
                        SalesLabel.Content = salesReport[1];
                        TicketsSold.Content = salesReport[2];
                        BookingsLabel.Visibility = Visibility.Visible;
                        SalesLabel.Visibility = Visibility.Visible;
                        TicketsSold.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Dates", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Generate Sales Report Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }



        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            StartingDate.SelectedDate = null;
            EndingDate.Text = "";
            BookingsLabel.Visibility = Visibility.Hidden;
            SalesLabel.Visibility = Visibility.Hidden;
            TicketsSold.Visibility = Visibility.Hidden;
        }



        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CinemaCombo.SelectedIndex = -1;
            MoviesTextBox.Clear();
            searchMode = false;
            currentPageNo = 1;
            TotalPages = 1;
            loadBookings();
        }

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (SearchTextBox.Text != "")
        //        {
        //            dgBookings.ItemsSource = new BookingsDAL().searchBookingsByMovie(SearchTextBox.Text, bookingslist);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please Fill Search box First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (dgBookings.Items.Count == 0)
        //        {
        //            MessageBox.Show("Shows Not Found By this Name", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}




        public async void SearchBookings()
        {
            try
            {
                int previousPages = (currentPageNo - 1) * 22;
                string CinemaName = "";
                if (CinemaCombo.SelectedIndex != -1)
                {
                    CinemaName = ((ComboBoxItem)CinemaCombo.SelectedItem).Content.ToString();
                }
                dgBookings.ItemsSource =await new BookingsDAL().searchBookings(MoviesTextBox.Text, CinemaName, 22, previousPages);
                calculateTotalPagesOfSearchedList(MoviesTextBox.Text, CinemaName);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Bookings Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
        //        dgBookings.ItemsSource = new BookingsDAL().searchBookings(MoviesTextBox.Text, CinemaName);
        //    }
        //}

        private void bookingsCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (bookingsCategory.SelectedIndex == 1)
            //{

            //}
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
                loadBookings();
            }
        }

        private void IncButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(dgBookings.Items.Count < 22) && (currentPageNo != totalPages) && searchMode == true)
            {
                currentPageNo++;
                SearchBookings();
            }
            if (!(dgBookings.Items.Count < 22) && (currentPageNo != totalPages) && searchMode == false)
            {
                currentPageNo++;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadBookings();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }


    }
}
