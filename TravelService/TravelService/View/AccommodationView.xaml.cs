using System;
using System.Collections.Generic;
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
using TravelService.Observer;
using TravelService.Repository;
using System.Collections.ObjectModel;
using TravelService.Model;
using System.Printing;
using System.Diagnostics.Metrics;
using System.ComponentModel;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Window
    {
        private readonly AccommodationRepository _accomodationRepository;
        private readonly LocationRepository _locationRepository;
        public Guest1 Guest1 { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public static List<Location> Locations { get; set; }
        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<string> Types { get; set; }

        public AccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;
            _accomodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();

            this.Guest1 = guest1;
            Accommodations = new ObservableCollection<Accommodation>(_accomodationRepository.GetAll());
            Locations = new List<Location>(_locationRepository.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }

            foreach(Accommodation accommodation in Accommodations)
            {
                if(accommodation.Type == TYPE.HOUSE)
                {
                    accommodation.TypeText = "House";
                }
                else if(accommodation.Type == TYPE.APARTMENT)
                {
                    accommodation.TypeText = "Apartment";
                }
                else
                {
                    accommodation.TypeText = "Cottage";
                }
            }

            foreach(Accommodation accommodation in Accommodations)
            {
                LocationComboBox.Items.Add(accommodation.Location.CityAndCountry);
            }
            LocationComboBox.Items.Insert(0, "");

            FilteredAccommodations = new ObservableCollection<Accommodation>();
            Types = new ObservableCollection<string>();
            Types.Add("");
            Types.Add("Apartment");
            Types.Add("House");
            Types.Add("Cottage");
        }

        private void Find_Accommodation_Click(object sender, RoutedEventArgs e)
        {
            Locations = new List<Location>(_locationRepository.GetAll());
            List<Accommodation> Filtered = new List<Accommodation>();

            string name = NameBox.Text.ToLower();
            string[] nameWords = name.Split(' ');
            string location = (string)LocationComboBox.Text.Replace(",", "").Replace(" ", "");
            string type = (string)AccommodationTypeComboBox.SelectedItem;
            string guestNumber = GuestNumberBox.Text;
            string daysForReservation = NumberOfDaysForReservationBox.Text;

            FilteredAccommodations.Clear();

            Filtered = _accomodationRepository.Search(name, nameWords, location, type, guestNumber, daysForReservation, Locations);

            foreach(var accommodation in Filtered)
            {
                FilteredAccommodations.Add(accommodation);
            }

            dataGridAccommodations.ItemsSource = FilteredAccommodations;
        }

        private void AccommodationTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccommodationTypeComboBox.SelectedItem != null)
            {
                string inputType = (string)AccommodationTypeComboBox.SelectedItem;
            }
        }

        private void ReserveAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationReservationView reservationView = new AccommodationReservationView(SelectedAccommodation, Guest1);
                reservationView.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation for reservation.");
            }
        }

        private void OwnerRating_Click(object sender, RoutedEventArgs e)
        {
            RatingView ratingView = new RatingView(Guest1);
            ratingView.Show();
;        }
    }
}

       

