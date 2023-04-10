using System.Windows;
using TravelService.Model;

namespace TravelService.View
{
    public partial class SecondGuestView : Window
    {
        public Tour SelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; }
        public Guest2 Guest2 { get; set; }
        public SecondGuestView(Guest2 guest2)
        {
            InitializeComponent();
            this.Guest2 = guest2;
        }
        private void TourTrackingViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourTrackingView tourTrackingView = new TourTrackingView(SelectedTour, Guest2);
            tourTrackingView.Show();
        }
        private void TourViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourView tourView = new TourView(Guest2);
            tourView.Show();
        }
        private void TourReservationButton_Click(object sender, RoutedEventArgs e)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour,SelectedVoucher, Guest2);
            tourReservationView.Show();
        }

        private void VoucherViewButton_CLick(object sender, RoutedEventArgs e)
        {
            TourReservationView tourReservationView = new TourReservationView(SelectedTour, SelectedVoucher, Guest2);
            VoucherView voucherView = new VoucherView(tourReservationView,SelectedVoucher,SelectedTour,Guest2);
            voucherView.ResetItemSource(voucherView.GuestVouchers);
            voucherView.Show();
        }
    }
}
