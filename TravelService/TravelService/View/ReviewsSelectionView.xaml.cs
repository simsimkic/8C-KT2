using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReviewsSelectionView.xaml
    /// </summary>
    public partial class ReviewsSelectionView : Window, INotifyPropertyChanged
    {
        public AccommodationRepository _accommodationRepository;

        public LocationRepository _locationRepository;

        public Accommodation SelectedAccommodation { get; set; }
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static List<Location> Locations { get; set; }
        public Owner Owner { get; set; }

        public ReviewsSelectionView(Owner owner)
        {
            InitializeComponent();
            this.Owner = owner;
            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationRepository.GetOwnersAccommodations(Owner.Id));
            Locations = new List<Location>(_locationRepository.GetAll());

            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = Locations.Find(l => l.Id == accommodation.LocationId);
            }

            foreach (Accommodation accommodation in Accommodations)
            {
                if (accommodation.Type == TYPE.HOUSE)
                {
                    accommodation.TypeText = "House";
                }
                else if (accommodation.Type == TYPE.APARTMENT)
                {
                    accommodation.TypeText = "Apartment";
                }
                else
                {
                    accommodation.TypeText = "Cottage";
                }
            }

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowReview_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null && Owner != null)
            {
                AccommodationReview accommodationReview = new AccommodationReview(SelectedAccommodation, Owner);
                accommodationReview.Show();
            }
            else
            {
                MessageBox.Show("Please select accommodation to show the review.");
            }
        }
    }
}
