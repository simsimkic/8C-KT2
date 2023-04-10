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
    public class Guest2Repository
    {
        private const string FilePath = "../../../Resources/Data/guests2.csv";

        private readonly Serializer<Guest2> _serializer;

        private List<Guest2> _guests;

        public Guest2Repository()
        {
            _serializer = new Serializer<Guest2>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public Guest2 GetByUsername(string username)
        {
            _guests = _serializer.FromCSV(FilePath);

            foreach (Guest2 guest in _guests)
            {
                if (guest.Username.Equals(username))
                {
                    return guest;
                }
            }

            return null;
        }
        public Guest2 FindById(int id)
        {
            _guests = _serializer.FromCSV(FilePath);
            foreach (Guest2 guest in _guests)
            {
                if (guest.Id == id)
                {
                    return guest;
                }
            }
            return null;
        }
        public List<Guest2> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

       /* public ObservableCollection<AccommodationReservation> FindReservationGuest(ObservableCollection<AccommodationReservation> UnratedReservations)
        {
            foreach (AccommodationReservation unratedReservation in UnratedReservations)
            {
                unratedReservation.Guest1 = _guests.Find(g => g.Id == unratedReservation.GuestId);
            }

            return UnratedReservations;
        }*/
    }
}
