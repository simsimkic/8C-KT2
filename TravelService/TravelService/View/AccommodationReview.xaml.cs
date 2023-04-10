using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AccommodationReview.xaml
    /// </summary>
    public partial class AccommodationReview : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Guest1> Guests { get; set; }
        public OwnerRatingRepository _ownerRatingRepository { get; set; }
        public GuestRatingRepository _guestRatingRepository { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Owner Owner { get; set; }
        public Guest1 SelectedGuest { get; set;  }


        private List<Uri> _ratingImages;

        public List<Uri> RatingImages
        {
            get => _ratingImages;
            set
            {
                if (value != _ratingImages)
                {
                    _ratingImages = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReview(Accommodation selectedAccommodation, Owner owner)
        {
            InitializeComponent();
            SelectedAccommodation = selectedAccommodation;
            this.Owner = owner;
            _ownerRatingRepository = new OwnerRatingRepository();
            _guestRatingRepository = new GuestRatingRepository();
            Guests = new ObservableCollection<Guest1>();

            List<Guest1> guestsList = _ownerRatingRepository.FindGuestsByAccommodation(SelectedAccommodation);  //gosti koji su ocenili ovaj smestaj
            List<Guest1> ratedGuests = _guestRatingRepository.FindRatedGuests(Owner.Id);    //gosti koje je ovaj vlasnik ocenio

            List<Guest1> commonGuests = new List<Guest1>();     //gosti koji su ocenili smestaj a i ocenjeni su od ovog vlasnika

            foreach (Guest1 guest in guestsList)
            {
                foreach(Guest1 ratedGuest in ratedGuests)
                {
                    if (guest.Id == ratedGuest.Id)
                    {
                        commonGuests.Add(guest);
                    }
                }
            }

            foreach (Guest1 g1 in commonGuests)
            {
                Guests.Add(g1);
            }

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ShowReview_Click(object sender, RoutedEventArgs e)
        {
            OwnerRating ownerRating = _ownerRatingRepository.FindByGuestOwnerIds(SelectedGuest.Id, Owner.Id, SelectedAccommodation.Id);
            Correctness.Text = ownerRating.Correctness.ToString();
            Cleanliness.Text = ownerRating.Cleanliness.ToString();
            Location.Text = ownerRating.Location.ToString();
            Comfort.Text = ownerRating.Comfort.ToString();
            Content.Text = ownerRating.Content.ToString();
            RatingImages = ownerRating.Pictures;
        }
    }
}
