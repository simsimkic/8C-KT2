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
using System.Xaml.Schema;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for RatingView.xaml
    /// </summary>
    public partial class RatingView : Window
    {
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly OwnerRepository _ownerRepository;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public ObservableCollection<AccommodationReservation> UnratedOwners { get; set; }
        public Guest1 LoggedInGuest1 { get; set; }
        public GuestRatingView Child { get; set; }

        public RatingView(Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;
            this.LoggedInGuest1 = guest1;
            _reservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _ownerRepository = new OwnerRepository();

            UnratedOwners = new ObservableCollection<AccommodationReservation>(_reservationRepository.FindUnratedOwners(guest1.Id));
            List<Accommodation> Accommodations = new List<Accommodation>(_accommodationRepository.GetAll());
            List<Location> Locations = new List<Location>(_locationRepository.GetAll());
            List<Owner> Owners = new List<Owner>(_ownerRepository.GetAll());
            _reservationRepository.SetAccommodationForUnratedOwners(Accommodations);
            _reservationRepository.SetLocationForUnratedOwners(Locations);
            _reservationRepository.SetNameForUnratedOwners(Owners);
        }

        private void rateOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnratedOwner != null)
            {
                Dispatcher.Invoke(() =>
                {
                    OwnerRatingView ownerRatingView = new OwnerRatingView(SelectedUnratedOwner);
                    ownerRatingView.Parent = this;
                    ownerRatingView.ShowDialog();
                });
            }
            else
            {
                MessageBox.Show("Select accommodation and owner that you want to rate.");
            }
        }
    }
}
