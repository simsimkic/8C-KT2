using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    public partial class TourTrackingView : Window
    {
        public readonly TourReservationRepository _tourReservationRepository;

        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;
        public static ObservableCollection<TourReservation> TourReservations { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public List<Tour> ActiveTours { get; set; }
        public Tour SelectedTour { get; set; }
        public Guest2 Guest2 { get; set; }
        public TourTrackingView(Tour selectedTour,Guest2 guest)
        {
            InitializeComponent();
            DataContext = this;
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            ActiveTours = new List<Tour>();

            this.Guest2 = guest;
            SelectedTour = selectedTour;            

            ActiveTours = _tourReservationRepository.showGuestsTours(convertTourList(Tours), Locations, Languages, CheckPoints, ActiveTours, convertTourReservationList(TourReservations),Guest2);
        }
        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        private List<TourReservation> convertTourReservationList(ObservableCollection<TourReservation> observableCollection)
        {
            List<TourReservation> convertedList = observableCollection.ToList();
            return convertedList;
        }
        
        private void trackTourButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                JoinTourView joinTourView = new JoinTourView(SelectedTour,Guest2);
                joinTourView.Show();
                Close();
            }
        }
    }
}
