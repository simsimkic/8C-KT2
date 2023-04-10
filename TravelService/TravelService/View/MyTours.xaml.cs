using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for MyTours.xaml
    /// </summary>
    public partial class MyTours : Window
    {
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly LanguageRepository _languageRepository;
        public readonly CheckPointRepository _checkpointRepository;
        public readonly TourReservationRepository _tourReservationRepository;

        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<Tour> FutureTours { get; set; }
        public static List<TourReservation> TourReservations { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }


        public Tour SelectedTour { get; set; }
        public Guide Guide { get; set; }

        public MyTours(Tour selectedTour,Guide guide)
        {
            InitializeComponent();
            DataContext = this;
            this.Guide = guide;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();
            _tourReservationRepository = new TourReservationRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            TourReservations = new List<TourReservation>(_tourReservationRepository.GetAll());
            FutureTours = new List<Tour>();
            List<Tour> TourList = _tourRepository.GetAll();

            SelectedTour = selectedTour;

            List<Tour> GuideTours = _tourRepository.FindGuidesTours(Guide.Id);

            FutureTours = _tourRepository.ShowFutureTourList(GuideTours, Locations, Languages, CheckPoints,FutureTours, Guide.Id,_tourRepository);






        }

        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CancelTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                bool tourCancelled = _tourRepository.CancelTour(SelectedTour.Id);
                if (tourCancelled)
                {
                    Tours.Remove(SelectedTour);

                    // Display message box with option to send vouchers
                    var result = MessageBox.Show("Tour cancelled successfully! Do you want to send vouchers to guests?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        
                        _tourRepository.SendVouchers(SelectedTour);
                    }
                }
                else
                {
                    MessageBox.Show("You cannot cancel this tour as it starts within 48 hours.");
                }
            }
        }




    }
}
