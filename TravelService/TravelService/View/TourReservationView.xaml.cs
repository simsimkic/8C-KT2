using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Window
    {
        public string EnteredNumberOfGuests { get; set; } = string.Empty;

        public readonly TourReservationRepository _tourReservationRepository;

        public readonly TourRepository _tourRepository;

        public readonly LocationRepository _locationRepository;

        public readonly LanguageRepository _languageRepository;

        public readonly CheckPointRepository _checkpointRepository;

        public readonly GuestVoucherRepository _guestVoucherRepository;
        public static ObservableCollection<TourReservation> TourReservations { get; set; }
        public static ObservableCollection<GuestVoucher> Vouchers { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static List<Location> Locations { get; set; }
        public static List<Language> Languages { get; set; }
        public static List<CheckPoint> CheckPoints { get; set; }
        public List<Tour> ActiveTours { get; set; }
        public List<Tour> OtherTours { get; set; }
        public List<Tour> OtherOtherTours { get; set; }
        public List<GuestVoucher> ValidVouchers { get; set; }
        public List<GuestVoucher> GuestVouchers { get; set; }
        public List<TourReservation> ReservationsByTour { get; set; }
        public Tour SelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; } = null;
        public Guest2 Guest2 { get; set; }

        public TourReservationView(Tour selectedTour,GuestVoucher selectedVoucher, Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            _tourReservationRepository = new TourReservationRepository();
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();
            _checkpointRepository = new CheckPointRepository();
            _guestVoucherRepository = new GuestVoucherRepository();

            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherRepository.GetAll());
            Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());
            Languages = new List<Language>(_languageRepository.GetAll());
            CheckPoints = new List<CheckPoint>(_checkpointRepository.GetAll());
            ReservationsByTour = new List<TourReservation>();
            ValidVouchers = new List<GuestVoucher>();
            GuestVouchers = new List<GuestVoucher>();
            this.Guest2 = guest2;

            ActiveTours = new List<Tour>();
            OtherTours = new List<Tour>();
            OtherOtherTours = new List<Tour>();

            SelectedTour = selectedTour;
            SelectedVoucher = selectedVoucher;

            ActiveTours = _tourReservationRepository.showAllActiveTours(convertTourList(Tours), Locations, Languages, CheckPoints, ActiveTours);
            ValidVouchers = _guestVoucherRepository.showValidVouchers(convertVoucherList(Vouchers), Guest2,GuestVouchers,ValidVouchers);
        }

        private List<Tour> convertTourList(ObservableCollection<Tour> observableCollection)
        {
            List<Tour> convertedList = observableCollection.ToList();
            return convertedList;
        }
        public List<GuestVoucher> convertVoucherList(ObservableCollection<GuestVoucher> observableCollection)
        {
            List<GuestVoucher> convertedList = observableCollection.ToList();
            return convertedList;
        }

        private List<TourReservation> convertTourReservationList(ObservableCollection<TourReservation> observableCollection)
        {
            List<TourReservation> convertedList = observableCollection.ToList();
            return convertedList;
        }

        public void FindOtherTours(Tour selectedTour)
        {
            OtherTours.Remove(selectedTour);
            ActiveTours.Remove(selectedTour);
            foreach (Tour tour in ActiveTours)
            {
                if (tour.LocationId == selectedTour.LocationId)
                {
                    OtherTours.Add(tour);
                    allActiveTours.ItemsSource = OtherTours;
                }

            }
            OtherTours.Remove(selectedTour);
            ActiveTours.Remove(selectedTour);
            allActiveTours.ItemsSource = OtherTours;

            RunOutOfTours();
        }

        private void RunOutOfTours()
        {
            if (OtherTours.Count() < 1)
            {
                MessageBox.Show("There are no more avaliable tours, please change the number of people!");
                ActiveTours.Clear();
                foreach (Tour tour in Tours)
                {
                    _tourReservationRepository.FindActiveTourList(tour, ActiveTours);
                }
                allActiveTours.ItemsSource = ActiveTours;
            }
        }

        public void SetSelectedVoucher(GuestVoucher selectedVoucher)
        {
            SelectedVoucher = selectedVoucher;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckTourButton_Click(object sender, RoutedEventArgs e)
        {
            bool reservationSuccess = false;
            reservationSuccess = _tourReservationRepository.TryReserving(SelectedTour, EnteredNumberOfGuests, convertTourReservationList(TourReservations), ReservationsByTour, OtherTours, this, Guest2);
            if(SelectedVoucher!=null)
                _guestVoucherRepository.UseVoucher(SelectedVoucher, reservationSuccess); 
        }

        private void UseVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            VoucherView voucherView = new VoucherView(this,SelectedVoucher,SelectedTour,Guest2);
            voucherView.ResetItemSource(ValidVouchers);
            voucherView.Show();
        }
    }
}