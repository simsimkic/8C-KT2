using System;
using TravelService.Serializer;

public enum VOUCHERTYPE { QUIT, CANCELLATION, BONUS };

namespace TravelService.Model
{
    public class GuestVoucher : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public VOUCHERTYPE VoucherType { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
        public int GuestId { get; set; }
        public int TourId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public GuestVoucher() { }

        public GuestVoucher(string name, VOUCHERTYPE voucherType, int value, string code, bool used, int guestId, int tourId, DateTime expirationDate)
        {

            Name = name;
            VoucherType = voucherType;
            Value = value;
            Code = code;
            Used = used;
            GuestId = guestId;
            TourId = tourId;
            ExpirationDate = expirationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                VoucherTypeToCSV(),
                Value.ToString(),
                Code,
                Used.ToString(),
                GuestId.ToString(),
                TourId.ToString(),
                ExpirationDate.ToString(),
            };
            return csvValues;
        }



        public string VoucherTypeToCSV()

        {
            if (this.VoucherType == VOUCHERTYPE.QUIT)
                return "Quit";
            else if (this.VoucherType == VOUCHERTYPE.CANCELLATION)
                return "Cancellation";
            else
                return "Bonus";
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            VoucherType = VoucherTypeFromCSV(values[2]);
            Value = Convert.ToInt32(values[3]);
            Code = values[4];
            Used = Boolean.Parse(values[5]);
            GuestId = Convert.ToInt32(values[6]);
            TourId = Convert.ToInt32(values[7]);
            ExpirationDate = DateTime.Parse(values[8]);
        }

        public VOUCHERTYPE VoucherTypeFromCSV(string voucherType)
        {
            if (string.Equals(voucherType, "Quit"))
                return VOUCHERTYPE.QUIT;
            else if (string.Equals(voucherType, "Cancellation"))
                return VOUCHERTYPE.CANCELLATION;
            else
                return VOUCHERTYPE.BONUS;
        }
    }
}