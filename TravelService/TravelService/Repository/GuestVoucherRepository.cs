using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class GuestVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<GuestVoucher> _serializer;

        private List<GuestVoucher> _vouchers;

        public GuestVoucherRepository()
        {
            _serializer = new Serializer<GuestVoucher>();
            _vouchers = _serializer.FromCSV(FilePath);
        }

        public List<GuestVoucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestVoucher Save(GuestVoucher voucher)
        {
            voucher.Id = NextId();
            _vouchers = _serializer.FromCSV(FilePath);
            _vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, _vouchers);
            return voucher;
        }

        public int NextId()
        {
            _vouchers = _serializer.FromCSV(FilePath);
            if (_vouchers.Count < 1)
            {
                return 1;
            }
            return _vouchers.Max(c => c.Id) + 1;
        }

        public void Delete(GuestVoucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            GuestVoucher founded = _vouchers.Find(c => c.Id == voucher.Id);
            _vouchers.Remove(founded);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public GuestVoucher Update(GuestVoucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            GuestVoucher current = _vouchers.Find(c => c.Id == voucher.Id);
            int index = _vouchers.IndexOf(current);
            _vouchers.Remove(current);
            _vouchers.Insert(index, voucher);
            _serializer.ToCSV(FilePath, _vouchers);
            return voucher;
        }

        public List<GuestVoucher> showVoucherList(List<GuestVoucher> Vouchers, Guest2 guest2, List<GuestVoucher> guestVouchers)
        {
            foreach (GuestVoucher voucher in Vouchers)
            {
                if (guest2.Id == voucher.GuestId && voucher.Used == false)
                {
                    guestVouchers.Add(voucher);
                }
            }
            return guestVouchers;
        }

        public List<GuestVoucher> GetVouchersForGuest(int guestId)
        {
            List<GuestVoucher> vouchers = new List<GuestVoucher>();

            List<GuestVoucher> allVouchers = GetAll();
            foreach (GuestVoucher voucher in allVouchers)
            {
                if (voucher.GuestId == guestId)
                {
                    vouchers.Add(voucher);
                }
            }

            return vouchers;
        }


        public List<GuestVoucher> showValidVouchers(List<GuestVoucher> Vouchers, Guest2 guest2, List<GuestVoucher> guestVouchers, List<GuestVoucher> validVouchers)
        {
            bool isVoucherValid = false;
            List<GuestVoucher> guestsVouchers = showVoucherList(Vouchers,guest2,guestVouchers);
            foreach (GuestVoucher voucher in guestsVouchers)
            {
                isVoucherValid = checkVoucherExpirationDate(voucher,validVouchers);
                deleteIfNotValid(isVoucherValid, voucher);
            }
            return validVouchers;
        }

        private void deleteIfNotValid(bool isVoucherValid, GuestVoucher voucher)
        {
            if (isVoucherValid == false)
                Delete(voucher);
        }

        private bool checkVoucherExpirationDate(GuestVoucher voucher, List<GuestVoucher> validVouchers)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (voucher.ExpirationDate.Date >= currentDate)
            {
                validVouchers.Add(voucher);
                return true;
            }
            else return false;
        }

        public bool IsVoucherUsable(GuestVoucher selectedVoucher, Tour selectedTour)
        {
            if (selectedTour.Id == selectedVoucher.TourId)
            {
                return true;
            }
            else
                return false;
        }

        public void UseVoucher(GuestVoucher selectedVoucher, bool reservationSuccess)
        {
            if(reservationSuccess)
                selectedVoucher.Used= true;
            Update(selectedVoucher);
        }
    }
}
