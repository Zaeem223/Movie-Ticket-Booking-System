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
using TicketsBookingApp.NewFolder1;

namespace TicketsBookingApp
{
    public partial class Movies : Page
    {
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
        public Movies()
        {
            InitializeComponent();
            loadMovies();
        }
        public async  void loadMovies()
        {
            calculateTotalPages();
            int previousPages = (currentPageNo - 1) * 17;
            try
            {
                dgMovies.ItemsSource = await new MoviesDAL().getAllMovies(17,previousPages);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Movies Please try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                int totalShows =await new MoviesDAL().getMoviesCount();
                totalPages = totalShows / 17;
                if (TotalPages == 0 || totalShows % 17 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void calculateTotalPagesOfSearchedList(string MovieName, string Genre)
        {
            try
            {
                int totalShows =await new MoviesDAL().getSearchedMoviesCount(MovieName, Genre);
                TotalPages = totalShows / 17;
                if (TotalPages == 0 || totalShows % 17 != 0)
                {
                    TotalPages++;
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgMovies.SelectedItem == null)
                {
                    MessageBox.Show("Select Movie First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    Movie temp = dgMovies.SelectedItem as Movie;

                    if ( ! await new ShowsDAL().checkMovieInShows(temp.MovieId))
                    {
                      await  new MoviesDAL().removeMovie(temp.MovieId);
                        MessageBox.Show("Movie Removed Succesfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                        loadMovies();
                    }
                    else
                    {
                        MessageBox.Show("Movie Cannot Remove at this moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Remove Movie Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (isClear() || dgMovies.SelectedItem == null)
                {
                    MessageBox.Show("Please Select Movie and Fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (!ValidateDuration())
                    {
                        return;
                    }
                    Movie temp = dgMovies.SelectedItem as Movie;
                    Movie movie = new Movie(temp.MovieId, NameTextBox.Text, GenreCombobox.Text, TimeSpan.Parse(DurationTextBox.Text), LanguageTextBox.Text, DescriptionTextBox.Text);
                 await   new MoviesDAL().updateMovie(movie);
                    MessageBox.Show("Movie Updated Successfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearAll();
                    loadMovies();
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Update Movie Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMovieButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgMovies.SelectedItem == null)
            {
                MessageBox.Show("Please Select Movie First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Movie temp = dgMovies.SelectedItem as Movie;
                NameTextBox.Text = temp.MovieName;
                DurationTextBox.Text = temp.Duration.ToString(); ;
                LanguageTextBox.Text = temp.Language;
                GenreCombobox.Text = temp.Genre;
                DescriptionTextBox.Text = temp.Description;
            }
        }
        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsLetter);
        }

        private bool ValidateDuration()
        {
            if (!TimeSpan.TryParse(
                DurationTextBox.Text,
                out TimeSpan duration))
            {
                MessageBox.Show(
                    "Please enter a valid time, e.g. 02:30",
                    "Invalid Duration",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isClear())
                {
                    MessageBox.Show("Please Enter All Fields First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (!ValidateDuration())
                    {
                        return;
                    }
                    Movie movie = new Movie(NameTextBox.Text, GenreCombobox.Text, TimeSpan.Parse(DurationTextBox.Text), LanguageTextBox.Text, DescriptionTextBox.Text);
                   await  new MoviesDAL().insertMovie(movie);

                    loadMovies();
                    MessageBox.Show("Movie Added Succesfully", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearAll();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("This movie already exists.", "Failed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Unable to Add Movie Please Try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool isClear()
        {
            if (NameTextBox.Text == "" || DescriptionTextBox.Text == "" || DurationTextBox.Text == "" || LanguageTextBox.Text == "" || GenreCombobox.SelectedIndex == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void  clearAll()
        {
            NameTextBox.Clear();
            DescriptionTextBox.Clear();
            DurationTextBox.Clear();
            LanguageTextBox.Clear();
            GenreCombobox.SelectedIndex = -1;
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
        }

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (SearchTextBox.Text != "")
        //        {
        //            dgMovies.ItemsSource = new MoviesDAL().searchMoviesByName(SearchTextBox.Text, movieslist);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Please Fill Search box First", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (dgMovies.Items.Count == 0)
        //        {
        //            MessageBox.Show("Movies Not Found By this Name", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Something went Wrong please Try Again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GenreCombo.SelectedIndex = -1;
            MoviesTextBox.Clear();
            searchMode = false;
            currentPageNo = 1;
            TotalPages = 1;
            loadMovies();
        }

        public async void SearchMovies()
        {
            int previousPages = (currentPageNo - 1) * 17;
            string Genre = "";
            try
            {
                if (GenreCombo.SelectedIndex != -1)
                {
                    Genre = ((ComboBoxItem)GenreCombo.SelectedItem).Content.ToString();
                }
                dgMovies.ItemsSource =await new MoviesDAL().searchMovies(MoviesTextBox.Text, Genre, 17, previousPages);
                calculateTotalPagesOfSearchedList(MoviesTextBox.Text, Genre);
            }
            catch (SqlException)
            {
                MessageBox.Show("Unable to Load Shows Please try again", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (MoviesTextBox.Text == ""  && GenreCombo.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Search Item First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SearchMovies();
            }
        }
        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string Genre = "";
         
        //    if (GenreCombo.SelectedIndex != -1)
        //    {
        //        Genre = ((ComboBoxItem)GenreCombo.SelectedItem).Content.ToString();
        //    }

        //    if (MoviesTextBox.Text == ""  && GenreCombo.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Please Select Search Item First", "Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //    else
        //    {
        //        dgMovies.ItemsSource = new MoviesDAL().searchMovies(MoviesTextBox.Text, Genre);
        //    }

        //}
        private void decButton_Click_1(object sender, RoutedEventArgs e)
        {

            if (currentPageNo != 1 && searchMode == true)
            {
                currentPageNo--;
                SearchMovies();
            }
            if (currentPageNo != 1 && searchMode == false)
            {
                currentPageNo--;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadMovies();
            }
        }

        private void IncButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(dgMovies.Items.Count < 17) && (currentPageNo != totalPages) && searchMode == true)
            {
                currentPageNo++;
                SearchMovies();
            }
            if (!(dgMovies.Items.Count < 17) && (currentPageNo != totalPages) && searchMode == false)
            {
                currentPageNo++;
                PagesCount.Content = currentPageNo.ToString() + " of " + totalPages.ToString();
                loadMovies();
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
    }
}
