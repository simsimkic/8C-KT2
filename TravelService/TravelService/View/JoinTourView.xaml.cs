using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for JoinTourView.xaml
    /// </summary>
    public partial class JoinTourView : Window
    {
        public readonly TourReservationRepository _tourReservationRepository;

        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;

        public readonly GuestRepository _guestRepository;
        public static ObservableCollection<TourReservation> TourReservations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public List<Tour> ActiveTours { get; set; }
        public List<Tour> OtherTours { get; set; }
        public List<Tour> OtherOtherTours { get; set; }
        public static List<CheckPoint> FilteredCheckPoint { get; set; }
        public Tour SelectedTour { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }
        public Guest2 Guest2 { get; set; }
        public JoinTourView(Tour selectedTour, Guest2 guest)
        {
            InitializeComponent();
            DataContext = this;
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();
            _guestRepository = new GuestRepository();


            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            FilteredCheckPoint = new List<CheckPoint>();
            this.Guest2 = guest;
            SelectedTour = selectedTour;
            

            FilteredCheckPoint = _tourRepository.ShowListCheckPointList(SelectedTour.Id, convertTourList(Tours), CheckPoints);
            ListCheckBox.ItemsSource = FilteredCheckPoint;

            SelectedCheckPoint = ListCheckBox.SelectedItem as CheckPoint;
            ListCheckBox.SelectionChanged += ListCheckBox_SelectionChanged;
        }
        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private void ListCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCheckPoint = ListCheckBox.SelectedItem as CheckPoint;
        }
        private void joinTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckPoint.Active == false)
            {
                if (SelectedCheckPoint != null)
                {
                    string username = Guest2.Username;
                    int age = Guest2.Age;
                    Guest guest = new Guest(username, SelectedCheckPoint.CheckPointId, SelectedCheckPoint.TourId, SelectedCheckPoint.Active, age);
                    _guestRepository.Save(guest);
                    Close();
                    MessageBox.Show("You joined the tour!");
                }
                else
                    MessageBox.Show("No checkpoint selected.");
            }
            else
                MessageBox.Show("Checkpoint is all ready visited.");
        }
    }
}
