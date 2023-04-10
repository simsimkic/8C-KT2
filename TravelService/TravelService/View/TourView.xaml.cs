using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    public partial class TourView : Window
    {
        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public ObservableCollection<Tour> FilteredTours { get; set; }
        public static List<CheckPoint> FilteredCheckPoints { get; set; }
        public static ObservableCollection<Guest> Guests { get; set; }
        public Guest2 Guest2 { get; set; }
        public TourView(Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            FilteredTours = new ObservableCollection<Tour>();
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            this.Guest2 = guest2;

            _tourRepository.ShowTourList(convertList(Tours), Locations, Languages, CheckPoints);
        }
        public List<Tour> convertList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private void searchTour_Click(object sender, RoutedEventArgs e)
        {
            FilteredTours.Clear();

            string inputDuration = (string)durationComboBox.Text;
            string inputLocation = (string)locationComboBox.Text.Replace(",", "").Replace(" ", "");
            string inputLanguage = (string)languageComboBox.Text;
            string inputGuestNumber = guestsTextBox.Text;

            foreach (Tour tour in Tours)
            {
                if (_tourRepository.isTourSearchable(tour, inputLocation, inputDuration, inputLanguage, inputGuestNumber))
                {
                    if (!FilteredTours.Contains(tour))
                        FilteredTours.Add(tour);

                    allTours.ItemsSource = FilteredTours;
                }
            }
            allTours.ItemsSource = FilteredTours;
        }

        private void showAllTours_Click(object sender, RoutedEventArgs e)
        {
            allTours.ItemsSource = Tours;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}