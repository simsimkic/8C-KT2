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
    /// <summary>
    /// Interaction logic for PastTours.xaml
    /// </summary>
    public partial class PastTours : Window
    { 
      public readonly TourRepository _tourRepository;
    public readonly LocationRepository _locationRepository;
    public readonly LanguageRepository _languageRepository;
    public readonly CheckPointRepository _checkpointRepository;
    public readonly TourReservationRepository _tourReservationRepository;

    public static List<Location> Locations { get; set; }
    public static List<CheckPoint> CheckPoints { get; set; }
    public static List<Language> Languages { get; set; }
    public static List<Tour> PastTour { get; set; }
    public static List<TourReservation> TourReservations { get; set; }

    public static ObservableCollection<Tour> Tours { get; set; }


    public Tour SelectedTour { get; set; }
    public Guide Guide { get; set; }

    public PastTours(Tour selectedTour, Guide guide)
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
        PastTour = new List<Tour>();
        List<Tour> TourList = _tourRepository.GetAll();

        SelectedTour = selectedTour;

        List<Tour> GuideTours = _tourRepository.FindGuidesTours(Guide.Id);

        PastTour = _tourRepository.ShowPastTourList(GuideTours, Locations, Languages, CheckPoints, PastTour, Guide.Id, _tourRepository);






    }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");
                return;
            }

            // Show the statistics for the selected tour
            TourStats statsWindow = new TourStats(SelectedTour);
            statsWindow.ShowDialog();
        }
    }
}
