using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for CheckPointView.xaml
    /// </summary>
    public partial class CheckPointView : Window
    {

        public Tour SelectedTour { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }
        private ObservableCollection<CheckPoint> _checkPoints;
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public static List<CheckPoint> FilteredCheckPoint { get; set; }
        public readonly CheckPointRepository _repositoryCheckPoint;
        public List<Tour> ActiveTours { get; set; }
        public readonly TourRepository _tourRepository;
        public readonly GuestRepository _guestRepository;
        public List<Guest> _guests { get; set; }


        public CheckPointView(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;

            _tourRepository = new TourRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            _guestRepository = new GuestRepository();   

            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_repositoryCheckPoint.GetAll());
            SelectedTour = selectedTour;
            FilteredCheckPoint = new List<CheckPoint>();
            ActiveTours = new List<Tour>();
            _guests = new List<Guest>();

            FilteredCheckPoint = _tourRepository.ShowListCheckPointList(SelectedTour.Id, convertTourList(Tours), CheckPoints);

            _checkPoints = new ObservableCollection<CheckPoint>(_repositoryCheckPoint.GetAll());
            _checkPoints.ElementAt(0).Active = true;
            SelectedCheckPoint = _checkPoints.ElementAt(0);

            ListCheckBox.ItemsSource = FilteredCheckPoint;
            _repositoryCheckPoint.FirstCheckPointActive(FilteredCheckPoint);
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
        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)this.FindName("myCheckBox");
            if (SelectedCheckPoint != null)
            {
                List<Guest> guestsAtCheckpoint = _guestRepository.filterGuestsByCheckpointAndTour(_guests, SelectedCheckPoint, SelectedTour);
                GuestPresence guestPresence = new GuestPresence(SelectedTour, SelectedCheckPoint);
                guestPresence.Show();
                Close();
            }
        }


        private int numChecked = 0;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            CheckPoint selectedCheckPoint = (CheckPoint)chkBox.DataContext;
            selectedCheckPoint.Active = true;
            _repositoryCheckPoint.Update(selectedCheckPoint);
            numChecked++;
            ListCheckBox.ItemsSource = FilteredCheckPoint;
            if (numChecked + 1 == ListCheckBox.Items.Count)
            {
                EndButton.IsEnabled = true;
            }
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            SelectedTour.Done = true;
            _tourRepository.Update(SelectedTour);
            MessageBox.Show("The tour was successfully completed");
            TourOverview tourOverview = new TourOverview(SelectedTour);
            tourOverview.Show();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The tour is cancelled");
            TourOverview tourOverview = new TourOverview(SelectedTour);
            tourOverview.Show();
            Close();
        }
    }
}