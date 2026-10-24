using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
using TicketsBookingApp.NewFolder1;

namespace TicketsBookingApp
{

    public partial class ShowsPage : Page
    {
        public ShowsPage()
        {
            InitializeComponent();
            loadMovies();
            loadCinemas();
            loadShows();
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




        private  void ManageMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Movies());
        }
        public async void calculateTotalPages()
        {
            int totalShows = await new ShowsDAL().getShowsCount();
            TotalPages = totalShows / 17;
            if (TotalPages == 0 || totalShows % 17 != 0)
            {
                TotalPages++;
            }
        }
        public async void calculateTotalPagesOfSearchedList(string MovieName, string CinemaName, string Genre)
        {
            try
            {
                int totalShows = await new ShowsDAL().getSearchedShowsCount(MovieName, CinemaName, Genre);
                TotalPages = totalShows / 17;
                if (TotalPages == 0 || totalShows % 17 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void loadMovies()
        {
            try
            {
                MoviesComboBox.ItemsSource = await new MoviesDAL().GetAllMoviesShortInfo();
                MoviesComboBox.DisplayMemberPath = "movieName";
                MoviesComboBox.SelectedValuePath = "movieId";
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void SearchShows()
        {
            try
            {
                int previousPages = (currentPageNo - 1) * 17;
                string CinemaName = "";
                string Genre = "";
                if (CinemaCombo.SelectedIndex != -1)
                {
                    CinemaName = ((ComboBoxItem)CinemaCombo.SelectedItem).Content.ToString();
                }
                if (GenreCombo.SelectedIndex != -1)
                {
                    Genre = ((ComboBoxItem)GenreCombo.SelectedItem).Content.ToString();
                }
                dgShowslist.ItemsSource =await new ShowsDAL().searchShows(MoviesTextBox.Text, CinemaName, Genre, 17, previousPages);
                calculateTotalPagesOfSearchedList(MoviesTextBox.Text, CinemaName, Genre);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (MoviesTextBox.Text == "" && CinemaCombo.SelectedIndex == -1 && GenreCombo.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Search Item First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SearchShows();
            }
        }

        private async void loadCinemas()
        {
            try
            {
                CinemaComboBox.ItemsSource =await new CinemasDAL().GetAllCinemasShortInfo();
                CinemaComboBox.DisplayMemberPath = "cinemaName";
                CinemaComboBox.SelectedValuePath = "cinemaId";
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Cinemas Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void loadShows()
        {
            calculateTotalPages();
            int previousPages = (currentPageNo - 1) * 17;
            try
            {
                dgShowslist.ItemsSource =await new ShowsDAL().getAllShows(17, previousPages);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private bool isClear()
        {
            if (TicketsTextBox.Text == "" || PriceTextBox.Text == "" || cmbTime.Text == "" || ShowDatePicker.Text == "" || ShowDatePicker.SelectedDate == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void clearall()
        {
            MoviesComboBox.Visibility = Visibility.Visible;
            CinemaComboBox.Visibility = Visibility.Visible;
            MoviesLabel.Visibility = Visibility.Visible;
            CinemasLabel.Visibility = Visibility.Visible;
            TicketsTextBox.Clear();
            PriceTextBox.Clear();
            ShowDatePicker.SelectedDate = null;
            ShowDatePicker.Text = "";
            cmbTime.SelectedIndex = -1;
            cmbTime.Text = "";

            CinemaComboBox.SelectedIndex = -1;
            MoviesComboBox.SelectedIndex = -1;
        }
        private void ClearButton_Click_(object sender, RoutedEventArgs e)
        {
            clearall();
        }

        private void EditShowButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgShowslist.SelectedItem == null)
                {
                    MessageBox.Show("Please Select Show First", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    ShowDetails temp = dgShowslist.SelectedItem as ShowDetails;

                    TicketsTextBox.Text = temp.Tickets.ToString();
                    PriceTextBox.Text = temp.ticketPrice.ToString();
                    TicketsTextBox.Text = temp.Tickets.ToString();
                    ShowDatePicker.SelectedDate = temp.BDateTime.Date;
                    cmbTime.Text = temp.BDateTime.ToString("HH:mm");

                    MoviesComboBox.Visibility = Visibility.Hidden;
                    CinemaComboBox.Visibility = Visibility.Hidden;
                    MoviesLabel.Visibility = Visibility.Hidden;
                    CinemasLabel.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isClear() && dgShowslist.SelectedItem != null && ValidateShowTime())
                {
                    ShowDetails temp = dgShowslist.SelectedItem as ShowDetails;
                    DateTime showDateTime = combineDateTime();

                    Show newShow = new Show(showDateTime, temp.showId, temp.cinemaId, Convert.ToInt32(TicketsTextBox.Text), Convert.ToDecimal(PriceTextBox.Text));
                   await new ShowsDAL().updateShow(newShow);
                    MessageBox.Show("Updated Succesfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearall();
                    loadShows();
                }
                else
                {
                    MessageBox.Show("Please Enter fields First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Update Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private DateTime combineDateTime()
        {
            DateTime date = ShowDatePicker.SelectedDate.Value;
            string timeText = cmbTime.Text;
            TimeSpan time = TimeSpan.Parse(timeText);
            return date.Date + time;
        }

        private bool ValidateShowTime()
        {
            if (!TimeSpan.TryParseExact(
                cmbTime.Text,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                out TimeSpan time))
            {
                MessageBox.Show(
                    "Please enter time in HH:mm format, e.g. 19:30",
                    "Invalid Time",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private bool IsTimeInPast()
        {
            DateTime currentTime = ShowDatePicker.SelectedDate.Value.Date + TimeSpan.Parse(cmbTime.Text);
            if (currentTime < DateTime.Now)
            {
                MessageBox.Show("Invalid Time", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }
        private async void AddButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isClear() || MoviesComboBox.SelectedIndex == -1 || CinemaComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Enter Fields First", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (!ValidateShowTime() || IsTimeInPast())
                    {
                        return;
                    }
                    if (Convert.ToInt32(TicketsTextBox.Text) <= 0)
                    {
                        MessageBox.Show("Invalid Number of Tickets", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    MovieLookup temp1 = MoviesComboBox.SelectedItem as MovieLookup;
                    CinemaLookup temp2 = CinemaComboBox.SelectedItem as CinemaLookup;
                    DateTime showDateTime = combineDateTime();

                    Show newShow = new Show(temp1.movieId, temp2.cinemaId, Convert.ToInt32(TicketsTextBox.Text), Convert.ToDecimal(PriceTextBox.Text), showDateTime);
                  await  new ShowsDAL().addShow(newShow);
                    MessageBox.Show("Show Added Succesfully", "Added", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearall();
                    loadShows();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Show already Exists", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Unable to Add Shows Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (SearchTextBox.Text != "")
        //        {
        //            dgShowslist.ItemsSource = new ShowsDAL().searchShowsByMovie(SearchTextBox.Text, showsList);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please Fill Search box First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (dgShowslist.Items.Count == 0)
        //        {
        //            MessageBox.Show("Shows Not Found By this Name", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CinemaCombo.SelectedIndex = -1;
            GenreCombo.SelectedIndex = -1;
            MoviesTextBox.Clear();
            searchMode = false;
            currentPageNo = 1;
            TotalPages = 1;
            loadShows();
        }

        //private void SelectShowsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dgShowslist.SelectedItem == null)
        //        {
        //            MessageBox.Show("Please Select Show First", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        }
        //        else
        //        {
        //            ShowDetails temp = dgShowslist.SelectedItem as ShowDetails;

        //            TicketsTextBox.Text = temp.Tickets.ToString();
        //            PriceTextBox.Text = temp.ticketPrice.ToString();
        //            ShowDatePicker.SelectedDate = temp.BDateTime.Date;
        //            cmbTime.Text = temp.BDateTime.ToString("HH:mm");
        //            MoviesComboBox.SelectedItem = temp.showName;

        //            var movie = MoviesComboBox.Items
        //               .Cast<MovieLookup>()
        //               .FirstOrDefault(m => m.movieName == temp.showName);
        //            if (movie != null)
        //            {
        //                MoviesComboBox.SelectedItem = movie;
        //            }

        //            MoviesComboBox.Visibility = Visibility.Visible;
        //            CinemaComboBox.Visibility = Visibility.Visible;
        //            MoviesLabel.Visibility = Visibility.Visible;
        //            CinemasLabel.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void ManageBookingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Bookings());

        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void decButton_Click_1(object sender, RoutedEventArgs e)
        {

            if (currentPageNo != 1 && searchMode == true)
            {
                currentPageNo--;
                SearchShows();
            }
            if (currentPageNo != 1 && searchMode == false)
            {
                currentPageNo--;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadShows();
            }
        }

        private void IncButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(dgShowslist.Items.Count < 17) && (currentPageNo != totalPages) && searchMode == true)
            {
                currentPageNo++;
                SearchShows();
            }
            if (!(dgShowslist.Items.Count < 17) && (currentPageNo != totalPages) && searchMode == false)
            {
                currentPageNo++;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadShows();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgShowslist.SelectedItem == null)
                {
                    MessageBox.Show("Select Show First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ShowDetails temp = dgShowslist.SelectedItem as ShowDetails;
                   await new ShowsDAL().deleteShow(temp.showId);
                    MessageBox.Show("Show Removed Succesfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadShows();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Remove Show Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
