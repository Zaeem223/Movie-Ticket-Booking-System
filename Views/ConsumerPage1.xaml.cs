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
    public partial class ConsumerPage1 : Page
    {
        User currentConsumer;
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
        List<ShowDetails> showsList = new List<ShowDetails>();
        public ConsumerPage1(User consumer)
        {
            InitializeComponent();
            currentConsumer = consumer;
            loadShows();
        }

        public async void calculateTotalPages()
        {
            int totalShows =await new ShowsDAL().getShowsCount();
            TotalPages = totalShows / 10;
            if (TotalPages == 0 || totalShows % 10 != 0)
            {
                TotalPages++;
            }
        }
        public async void calculateTotalPagesOfSearchedList(string MovieName, string CinemaName, string Genre)
        {
            int totalShows = await new ShowsDAL().getSearchedShowsCount(MovieName, CinemaName, Genre);
            TotalPages = totalShows / 10;
            if (TotalPages == 0 || totalShows % 10 != 0)
            {
                TotalPages++;
            }
        }
        public async void loadShows()
        {
            calculateTotalPages();
            int previousPages = (currentPageNo - 1) * 10;
            UserNameLabel.Content = currentConsumer.UserName;
            try
            {
                showsList =await new ShowsDAL().getAllShows(10, previousPages);
                dgShows.ItemsSource = showsList;
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
        //private void View_ButtonClick(object sender, RoutedEventArgs e)
        //{
        //    Button view = sender as Button;
        //    ShowDetails linkedData = view.DataContext as ShowDetails;
        //    MovieNameLabel.Content = linkedData.showName;
        //    MovieNameLabel.Visibility = Visibility.Visible;
        //    MessageBox.Show(linkedData.CinemaName + " " + linkedData.showName);
        //}

        private async void BookNowButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgShows.SelectedItem != null && NoOfTicketsTextBox.Text != "")
                {
                    ShowDetails temp = dgShows.SelectedItem as ShowDetails;
                    if (Convert.ToInt32(NoOfTicketsTextBox.Text) <= 0)
                    {
                        MessageBox.Show("Invalid Tickets", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    Bookings_ booking = new Bookings_(currentConsumer.UserId, temp.showId, Convert.ToInt32(NoOfTicketsTextBox.Text), DateTime.Now);
                    await new BookingsDAL().addBooking(booking);
                    int BookingId =await new BookingsDAL().getRecentBookingId();
                    //getcurrentbooking()
                    MessageBox.Show($"Booking Successful Booking Id: {BookingId} ", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    loadShows();
                    dgShows.SelectedItem = null;
                    PriceLabel.Content = "";
                    NoOfTicketsTextBox.Text = "1";
                }
                else
                {
                    MessageBox.Show("Please Select Show First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Book Show", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //private void ViewDetailsButton_Click_1(object sender, RoutedEventArgs e)
        //{
        //    ShowDetails temp = dgShows.SelectedItem as ShowDetails;
        //    MovieNameLabel.Content = "Movie Name: " + temp.showName;
        //    GenreLabel.Content = "Genre: " + temp.Genre;
        //    DurationLabel.Content = "Duration: " + temp.Duration;
        //    LanguageLabel.Content = "Language: " + temp.Language;
        //    TicketPriceLabel.Content = "Ticket Price: " + temp.ticketPrice;
        //    DescriptionLbael.Content = "Description : " + temp.Description;
        //    DateTimeLabel.Content = "DateTime: " + temp.BDateTime;


        //    DetailsOfShow.Visibility = Visibility.Visible;
        //}

        private void MyBooking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MyBookingsList(currentConsumer));
        }

        private void BookNowTextBox(object sender, TextChangedEventArgs e)
        {
            if (dgShows.SelectedItem != null)
            {

                ShowDetails temp = dgShows.SelectedItem as ShowDetails;
                if (NoOfTicketsTextBox.Text != "")
                {
                    PriceLabel.Content = temp.ticketPrice * Convert.ToInt32(NoOfTicketsTextBox.Text);
                    if (Convert.ToInt32(NoOfTicketsTextBox.Text) > temp.Tickets)
                    {
                        MessageBox.Show("Invalid Tickets Count", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        NoOfTicketsTextBox.Text = "1";
                    }
                }
            }
        }
        private void TicketsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
        private void TicketsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void TicketsTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            string text = e.DataObject.GetData(DataFormats.Text) as string;

            if (!text.All(char.IsDigit))
            {
                e.CancelCommand();
            }
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgShows.SelectedItem != null)
            {
                int noOfTick = Convert.ToInt32(NoOfTicketsTextBox.Text);
                noOfTick++;
                NoOfTicketsTextBox.Text = noOfTick.ToString();
            }
            else
            {
                MessageBox.Show("Please Select Show First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (SearchTextBox.Text != "")
        //        {
        //            if (SearchCombo.SelectedIndex == -1)
        //            {
        //                MessageBox.Show("Please Select Search type First", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            if (SearchCombo.SelectedIndex == 0)
        //            {
        //                dgShows.ItemsSource = new ShowsDAL().searchShowsByMovie(SearchTextBox.Text, showsList);
        //            }
        //            else if (SearchCombo.SelectedIndex == 1)
        //            {
        //                dgShows.ItemsSource = new ShowsDAL().searchShowsByGenre(SearchTextBox.Text, showsList);
        //            }
        //            else if (SearchCombo.SelectedIndex == 2)
        //            {
        //                dgShows.ItemsSource = new ShowsDAL().searchShowsByCinema(SearchTextBox.Text, showsList);
        //            }
        //            if (dgShows.Items.Count == 0)
        //            {
        //                MessageBox.Show("Shows Not Found By this Name", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please Fill Search box First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        public async void SearchShows()
        {
            int previousPages = (currentPageNo - 1) * 10;
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
            dgShows.ItemsSource =await new ShowsDAL().searchShows(MoviesTextBox.Text, CinemaName, Genre, 10, previousPages);
            calculateTotalPagesOfSearchedList(MoviesTextBox.Text, CinemaName, Genre);
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


        private void dgShows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgShows.SelectedItem != null)
            {
                PriceLabel.Content = "";
                NoOfTicketsTextBox.Text = "1";
                NoOfTicketsTextBox.IsEnabled = dgShows.SelectedItem != null;
            }
        }


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            if (!(dgShows.Items.Count < 10) && (currentPageNo != totalPages) && searchMode == true)
            {
                currentPageNo++;
                SearchShows();
            }
            if (!(dgShows.Items.Count < 10) && (currentPageNo != totalPages) && searchMode == false)
            {
                currentPageNo++;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadShows();
            }
        }

    }
}
