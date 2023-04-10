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
    /// Interaction logic for GuestRatingOverview.xaml
    /// </summary>
    public partial class GuestRatingOverview : Window, INotifyPropertyChanged
    {
        private readonly AccommodationReservationRepository _reservationRepository;

        private readonly AccommodationRepository _accommodationRepository;

        private readonly Guest1Repository _guest1Repository;
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public Owner Owner { get; set; }
        public GuestRatingView Child { get; set;  }
        public GuestRatingOverview(Owner owner)
        {
            InitializeComponent();
            this.Owner = owner;
            _reservationRepository = new AccommodationReservationRepository();
            _accommodationRepository = new AccommodationRepository();
            _guest1Repository = new Guest1Repository();
            UnratedReservations = new ObservableCollection<AccommodationReservation>();

            List<AccommodationReservation> reservationList = _reservationRepository.GetAll();

            UnratedReservations = _reservationRepository.FindUnratedReservations(_accommodationRepository, Owner.Id);

            UnratedReservations = _guest1Repository.FindReservationGuest(UnratedReservations);

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation, Owner);
                guestRatingView.Parent = this;
                guestRatingView.ShowDialog();
            });
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
