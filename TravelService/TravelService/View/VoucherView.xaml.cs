using System;
using System.Collections;
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
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    public partial class VoucherView : Window
    {
        public readonly GuestVoucherRepository _guestVoucherRepository;
        public static ObservableCollection<GuestVoucher> Vouchers { get; set; }
        public List<GuestVoucher> GuestVouchers { get; set; }
        public Tour SelectedTour { get; set; }
        public GuestVoucher SelectedVoucher { get; set; }
        public Guest2 Guest2 { get; set; }
        private readonly TourReservationView _tourReservationView;
        public VoucherView(TourReservationView tourReservationView,GuestVoucher selectedVoucher,Tour selectedTour, Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            this.Guest2 = guest2;
            SelectedTour = selectedTour;
            SelectedVoucher = selectedVoucher;
            _tourReservationView = tourReservationView;

            _guestVoucherRepository = new GuestVoucherRepository();
            Vouchers = new ObservableCollection<GuestVoucher>(_guestVoucherRepository.GetAll());
            GuestVouchers = new List<GuestVoucher>();

            GuestVouchers = _guestVoucherRepository.showVoucherList(convertVoucherList(Vouchers), Guest2, GuestVouchers);
        }

        public List<GuestVoucher> convertVoucherList(ObservableCollection<GuestVoucher> observableCollection)
        {
            List<GuestVoucher> convertedList = observableCollection.ToList();
            return convertedList;
        }
        public void ResetItemSource(IEnumerable newItemsSource)
        {
            allVouchers.ItemsSource = newItemsSource;
        }
        private void UseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_guestVoucherRepository.IsVoucherUsable(SelectedVoucher, SelectedTour))
            {
                MessageBox.Show("You selected a valid voucher!");
                _tourReservationView.SetSelectedVoucher(SelectedVoucher);
                this.Close();
            }
            else
                MessageBox.Show("This voucher does not apply on selected tour!");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
