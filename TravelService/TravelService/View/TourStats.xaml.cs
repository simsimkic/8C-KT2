using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TourStats.xaml
    /// </summary>
    public partial class TourStats : Window,INotifyPropertyChanged
    {
        public readonly GuestRepository _guestRepository;
        public Tour SelectedTour { get; set; }
        public List<TourStatistics> TourStatisticsList { get; set; }


        public TourStats(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = this;
            SelectedTour = selectedTour;

            // Create an instance of GuestRepository
            _guestRepository = new GuestRepository();

            // Call the ShowTourStatistics method with the selected tour's Id
            TourStatisticsList= new List<TourStatistics> { _guestRepository.ShowTourStatistics(selectedTour.Id) };

            // Update the properties in TourStats with the results from the repository
            Under18Count = _guestRepository.Under18Count;
            Between18And50Count = _guestRepository.Between18And50Count;
            Over50Count = _guestRepository.Over50Count;
            WithVoucherPercentage = _guestRepository.WithVoucherPercentage;
            WithoutVoucherPercentage = _guestRepository.WithoutVoucherPercentage;

            Stats.ItemsSource = TourStatisticsList;


        }


        private int _under18Count;
        public int Under18Count
        {
            get { return _under18Count; }
            set
            {
                _under18Count = value;
                OnPropertyChanged(nameof(Under18Count));
            }
        }

        private int _between18And50Count;
        public int Between18And50Count
        {
            get { return _between18And50Count; }
            set
            {
                _between18And50Count = value;
                OnPropertyChanged(nameof(Between18And50Count));
            }
        }

        private int _over50Count;
        public int Over50Count
        {
            get { return _over50Count; }
            set
            {
                _over50Count = value;
                OnPropertyChanged(nameof(Over50Count));
            }
        }

        private double _withVoucherPercentage;
        public double WithVoucherPercentage
        {
            get { return _withVoucherPercentage; }
            set
            {
                _withVoucherPercentage = value;
                OnPropertyChanged(nameof(WithVoucherPercentage));
            }
        }

        private double _withoutVoucherPercentage;
        public double WithoutVoucherPercentage
        {
            get { return _withoutVoucherPercentage; }
            set
            {
                _withoutVoucherPercentage = value;
                OnPropertyChanged(nameof(WithoutVoucherPercentage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

