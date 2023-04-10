using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for MarkAttendence.xaml
    /// </summary>
    public partial class MarkAttendence : Window
    {
        public CheckPoint SelectedCheckPoint;

        public Tour SelectedTour;

        public Guest2 SelectedGuest;

        public readonly TourRepository _repositoryTour;

        public readonly Guest2Repository _repositoryGuest;

        public readonly InvitationRepository _invitationRepository;

        public List<Tour> _tours;
        public static ObservableCollection<Guest2> Guests { get; set; }
        public static ObservableCollection<Invitation> Invitations { get; set; }
        public Guest2 Guest2 { get; set; }

        public MarkAttendence(Tour selectedTour, CheckPoint selectedCheckPoint, Guest2 guest)
        {
            InitializeComponent();
            SelectedCheckPoint = selectedCheckPoint;
            SelectedTour = selectedTour;
            this.Guest2 = guest;

            _repositoryTour = new TourRepository();
            _repositoryGuest = new Guest2Repository();
            _invitationRepository = new InvitationRepository();

            Guests = new ObservableCollection<Guest2>(_repositoryGuest.GetAll());
            Invitations = new ObservableCollection<Invitation>(_invitationRepository.GetAll());
        }

        private List<Invitation> convertInvitationList(ObservableCollection<Invitation> invitations)
        {
            List<Invitation> convertedList = invitations.ToList();
            return convertedList;

        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thank you for answering!");
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _invitationRepository.confirmInvitation(convertInvitationList(Invitations), Guest2);
            MessageBox.Show("Thank you for confirming!");
            SecondGuestView secondGuestView = new SecondGuestView(Guest2);
            secondGuestView.Show();
            Close();
        }
    }
}
