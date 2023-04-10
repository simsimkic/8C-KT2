using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window, INotifyPropertyChanged
    {
        private readonly GuestRatingRepository _guestRatingRepository;

        private readonly AccommodationReservationRepository _reservationRepository;

        private readonly AccommodationRepository _accommodationRepository;

        private readonly Guest1Repository _guest1Repository;
        public Owner Owner { get; set; }
        public Guest1 Guest { get; set; }

        public Window Parent { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        private int ReservationId;

        private int _cleanness;
        public int Cleanness
        {
            get => _cleanness;
            set
            {
                if (value != _cleanness)
                {
                    _cleanness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _rulesFollowing;
        public int RulesFollowing
        {
            get => _rulesFollowing;
            set
            {
                if (value != _rulesFollowing)
                {
                    _rulesFollowing = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _noiseLevel;
        public int NoiseLevel
        {
            get => _noiseLevel;
            set
            {
                if (value != _noiseLevel)
                {
                    _noiseLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _communication;
        public int Communication
        {
            get => _communication;
            set
            {
                if (value != _communication)
                {
                    _communication = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _propertyRespect;
        public int PropertyRespect
        {
            get => _propertyRespect;
            set
            {
                if (value != _propertyRespect)
                {
                    _propertyRespect = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuestRatingView(AccommodationReservation selectedReservation, Owner owner)
        {
            InitializeComponent();
            SelectedReservation = selectedReservation;
            ReservationId = selectedReservation.Id;
            this.Owner = owner;
            _guestRatingRepository = new GuestRatingRepository();
            _reservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            _guest1Repository = new Guest1Repository();

            Guest = _guest1Repository.FindById(SelectedReservation.GuestId);
            AccommodationReservation ratedReservation = _reservationRepository.FindById(ReservationId);
            Accommodation accommodation = _accommodationRepository.FindById(ratedReservation.AccommodationId);

            AccommodationName.Text = accommodation.Name;
            CheckInDate.Text = ratedReservation.CheckInDate.ToString("dd-MMM-yyyy");
            CheckOutDate.Text = ratedReservation.CheckOutDate.ToString("dd-MMM-yyyy");
            GuestName.Text = Guest.Username;

            DataContext = this;
        }

        private void AddGuestRating_Click(object sender, RoutedEventArgs e)
        {
            GuestRating guestRating = new GuestRating(Owner.Id, Guest.Id, Cleanness, RulesFollowing, Communication, NoiseLevel, PropertyRespect, Comment, ReservationId);
            _guestRatingRepository.Save(guestRating);
            AccommodationReservation ratedReservation = _reservationRepository.FindById(ReservationId);
            ratedReservation.IsRated = true;
            _reservationRepository.Update(ratedReservation);

            GuestRatingOverview guestRatingOverview = (GuestRatingOverview)this.Parent;
            guestRatingOverview.UnratedReservations.Remove(SelectedReservation);

            Close();
        }

        private void CancelGuestRating_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
