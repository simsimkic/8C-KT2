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
    /// Interaction logic for TourOverview.xaml
    /// </summary>
    public partial class TourOverview : Window
    {
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly LanguageRepository _languageRepository;
        public readonly CheckPointRepository _checkpointRepository;

        public static List<Location> Locations { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<Tour> ActiveTours { get; set; }
        public static List<Language> Languages { get; set; }

        public static ObservableCollection<Tour> Tours { get; set; }
        private ObservableCollection<Tour> _activeTours;

        public Tour SelectedTour { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }


        public TourOverview(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            ActiveTours = new List<Tour>();

            SelectedTour = selectedTour;
            ActiveTours = _tourRepository.showAllActiveTours(convertTourList(Tours), Locations, Languages, CheckPoints, ActiveTours);
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

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedTour != null)
            {
                CheckPointView checkPointView = new CheckPointView(SelectedTour);

                checkPointView.Show();
                Close();
            }

        }

    }
}