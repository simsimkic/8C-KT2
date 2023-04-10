using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AccommodationReservationView.xaml
    /// </summary>
    public partial class AccommodationReservationView : Window, INotifyPropertyChanged
    {
        public AccommodationReservationRepository _reservationRepository;
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 LoggedInGuest1 { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public ObservableCollection<Tuple<DateTime, DateTime>> AvailableDatesPair { get; set; }
        public Tuple<DateTime, DateTime> SelectedAvailableDatePair { get; set; }

        public AccommodationReservationView(Accommodation selectedAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;

            _reservationRepository = new AccommodationReservationRepository();
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationRepository.GetAll());
            SelectedAccommodation = selectedAccommodation;
            this.LoggedInGuest1 = guest1;

            startDatePicker.DisplayDateStart = DateTime.Today;
            endDatePicker.DisplayDateStart = DateTime.Today;
            AvailableDatesPair = new ObservableCollection<Tuple<DateTime, DateTime>>();
        }

        private DateTime _checkInDate;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (value != _checkInDate)
                {
                    _checkInDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _checkOutDate;
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value != _checkOutDate)
                {
                    _checkOutDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _lengthOfStay;
        public int LengthOfStay
        {
            get => _lengthOfStay;
            set
            {
                if (value != _lengthOfStay)
                {
                    _lengthOfStay = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guestNumber;
        public int GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRated;
        public bool IsRated
        {
            get => _isRated;
            set
            {
                if (value != _isRated)
                {
                    _isRated = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isOwnerRated;
        public bool IsOwnerRated
        {
            get => _isOwnerRated;
            set
            {
                if (value != _isOwnerRated)
                {
                    _isOwnerRated = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName] => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void CheckAvailability_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            int daysOfStaying = int.Parse(daysOfStayingBox.Text);
            List<Tuple<DateTime, DateTime>> AvailableDateRange = new List<Tuple<DateTime, DateTime>>();
            List<Tuple<DateTime, DateTime>> AvailableDatesOutsideRange = new List<Tuple<DateTime, DateTime>>();

            if (endDate < startDate)
            {
                if (AvailableDatesPair != null)
                {
                    AvailableDatesPair.Clear();
                }
                MessageBox.Show("End date must be greater than start date. Please try again.");
                return;
            }

            if(daysOfStaying < SelectedAccommodation.MinReservationDays)
            {
                if (AvailableDatesPair != null)
                {
                    AvailableDatesPair.Clear();
                }
                MessageBox.Show($"Minimum number of days for reservation is {SelectedAccommodation.MinReservationDays}");
                return;
            }

            AvailableDatesPair.Clear();
            AvailableDateRange = _reservationRepository.FindAvailableDates(SelectedAccommodation, startDate, endDate, daysOfStaying);
            
            foreach(var dateRange in AvailableDateRange)
            {
                AvailableDatesPair.Add(dateRange);
            }

            if(AvailableDateRange.Count == 0)
            {
                NotificationBlock.Text = "All dates in the given range are taken. We recommend the following dates: ";
                AvailableDatesPair.Clear();
                AvailableDatesOutsideRange = _reservationRepository.FindAvailableDatesOutsideRange(SelectedAccommodation, startDate, endDate, daysOfStaying);
                
                foreach (var dateRange in AvailableDatesOutsideRange)
                {
                    AvailableDatesPair.Add(dateRange);
                }
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAvailableDatePair != null)
            {
                if(int.Parse(guestNumberBox.Text) > SelectedAccommodation.MaxGuestNumber)
                {
                    MessageBox.Show($"Maximum number of guests for {SelectedAccommodation.Name} accommodation is {SelectedAccommodation.MaxGuestNumber}");
                    return;
                }

                CheckInDate = SelectedAvailableDatePair.Item1;
                CheckOutDate = SelectedAvailableDatePair.Item2;
                IsRated = false;
                IsOwnerRated = false;
                AccommodationReservation reservation = new AccommodationReservation(SelectedAccommodation.Id, SelectedAccommodation.Name, LoggedInGuest1.Id, SelectedAccommodation.OwnerId, SelectedAccommodation.LocationId, CheckInDate, CheckOutDate, LengthOfStay, GuestNumber, IsRated, IsOwnerRated);
                _reservationRepository.Save(reservation);
                Close();
            }
            else
            {
                MessageBox.Show("Please choose date range for your reservation.");
            }
        }

        private void CloseReservations_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
    }
}
