using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class Guest1Repository
    {
        private const string FilePath = "../../../Resources/Data/guests1.csv";

        private readonly Serializer<Guest1> _serializer;

        private List<Guest1> _guests;

        public Guest1Repository()
        {
            _serializer = new Serializer<Guest1>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public Guest1 GetByUsername(string username)
        {
            _guests = _serializer.FromCSV(FilePath);

            foreach(Guest1 guest in _guests)
            {
                if (guest.Username.Equals(username))
                {
                    return guest;
                }
            }

            return null;
        }
        public Guest1 FindById(int id)
        {
            _guests = _serializer.FromCSV(FilePath);
            foreach (Guest1 guest in _guests)
            {
                if (guest.Id == id)
                {
                    return guest;
                }
            }
            return null;
        }
        public List<Guest1> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ObservableCollection<AccommodationReservation> FindReservationGuest (ObservableCollection<AccommodationReservation> UnratedReservations)
        {
            foreach (AccommodationReservation unratedReservation in UnratedReservations)
            {
                unratedReservation.Guest1 = _guests.Find(g => g.Id == unratedReservation.GuestId);
            }

            return UnratedReservations;
        }
    }
}
