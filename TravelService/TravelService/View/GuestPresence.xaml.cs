using System;
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
    /// Interaction logic for GuestPresence.xaml
    /// </summary>
    public partial class GuestPresence : Window
    {
        public Guest SelectedGuest { get; set; }
        public List<Guest> SelectedGuests { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        private GuestRepository _repositoryGuest;
        private ObservableCollection<Guest> _guests;
        private CheckPointRepository _repositoryCheckPoint;
        private TourRepository _repositoryTour;
        public CheckPoint SelectedCheckPoint { get; set; }
        public Tour SelectedTour { get; set; }
        public InvitationRepository _repositoryInvitation { get; set; }

        public GuestPresence(Tour selectedTour, CheckPoint selectedcCheckPoint)
        {
            InitializeComponent();
            DataContext = this;
            _repositoryTour = new TourRepository();
            _repositoryGuest = new GuestRepository();
            _repositoryCheckPoint = new CheckPointRepository();
            _repositoryInvitation = new InvitationRepository();
            SelectedTour = selectedTour;
            SelectedCheckPoint = selectedcCheckPoint;
            _guests = new ObservableCollection<Guest>(_repositoryGuest.GetAll());

            SelectedGuests = new List<Guest>();

            _guests = new ObservableCollection<Guest>(_repositoryGuest.filterGuestsByCheckpointAndTour(convertGuestList(_guests), SelectedCheckPoint, SelectedTour));
            GuestDataGrid.ItemsSource = _guests;



        }

        private List<Guest> convertGuestList(ObservableCollection<Guest> observableCollection)
        {
            List<Guest> convertedList = observableCollection.ToList();
            return convertedList;
        }






        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedGuest != null)
            {
                Invitation invitation = new Invitation(SelectedGuest.Id, false);
                _repositoryInvitation.Save(invitation);

            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CheckPointView checkPointView = new CheckPointView(SelectedTour);
            checkPointView.Show();
        }
    }
}