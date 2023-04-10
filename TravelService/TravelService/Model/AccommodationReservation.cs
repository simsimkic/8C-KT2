using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public string AccommodationName { get; set; }
        public int GuestId { get; set; }
        public Guest1 Guest1 { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int LengthOfStay { get; set; }
        public int GuestNumber { get; set; }
        public bool IsRated { get; set; }
        public bool IsOwnerRated { get; set; }
        public AccommodationReservation() { }

        public AccommodationReservation(int accommodationId, string accommodationName, int guestId, int ownerId, int locationId, DateTime checkInDate, DateTime checkOutDate, int lengthOfStay, int guestNumber, bool isRated, bool isOwnerRated)
        {
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
            GuestId = guestId;
            OwnerId = ownerId;
            LocationId = locationId;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            LengthOfStay = lengthOfStay;
            GuestNumber = guestNumber;
            IsRated = isRated;
            IsOwnerRated = isOwnerRated;
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            { 
                Id.ToString(),
                AccommodationId.ToString(),
                AccommodationName,
                GuestId.ToString(),
                OwnerId.ToString(),
                LocationId.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString(),
                LengthOfStay.ToString(),
                GuestNumber.ToString(),
                IsRated.ToString(),
                IsOwnerRated.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            AccommodationName = values[2];
            GuestId = Convert.ToInt32(values[3]);
            OwnerId = Convert.ToInt32(values[4]);
            LocationId = Convert.ToInt32(values[5]);
            CheckInDate = DateTime.Parse(values[6]);
            CheckOutDate = DateTime.Parse(values[7]);
            LengthOfStay = Convert.ToInt32(values[8]);
            GuestNumber = Convert.ToInt32(values[9]);
            IsRated = bool.Parse(values[10]);
            IsOwnerRated = bool.Parse(values[11]);
        }
    }
}
