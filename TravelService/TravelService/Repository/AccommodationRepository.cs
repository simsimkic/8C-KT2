using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class AccommodationRepository 
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetOwnersAccommodations(int ownerId)
        {
            List<Accommodation> ownersAccommodations = new List<Accommodation>();
            foreach(Accommodation accommodation in _accommodations)
            {
                if(accommodation.OwnerId == ownerId)
                {
                    ownersAccommodations.Add(accommodation);
                }
            }

            return ownersAccommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation founded = _accommodations.Find(c => c.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodations);
        }
        public Accommodation FindById(int id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            foreach (Accommodation accommodation in _accommodations)
            {
                if (accommodation.Id == id)
                {
                    return accommodation;
                }
            }
            return null;
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            Accommodation current = _accommodations.Find(c => c.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);      
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public void SetLocationForAccommodation(List<Location> locations, List<Accommodation> accommodations)
        {
            
            foreach (Accommodation accommodation in accommodations)
            {
                accommodation.Location = locations.Find(l => l.Id == accommodation.LocationId);
            }
        }

        public void SetTypeForAccommodation(List<Accommodation> accommodations)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.Type == TYPE.HOUSE)
                {
                    accommodation.TypeText = "House";
                }
                else if (accommodation.Type == TYPE.APARTMENT)
                {
                    accommodation.TypeText = "Apartment";
                }
                else
                {
                    accommodation.TypeText = "Cottage";
                }
            }
        }

        public List<Accommodation> Search(string name, string[] nameWords, string location, string type, string guestNumber, string daysForReservation, List<Location> Locations)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            SetTypeForAccommodation(_accommodations);
            SetLocationForAccommodation(Locations, _accommodations);
            List<Accommodation> filteredAccommodations = new List<Accommodation>();

            foreach (Accommodation accommodation in _accommodations)
            {
                if ((IsContainingNameWords(accommodation, nameWords) || string.IsNullOrEmpty(name)) &&
                    ((accommodation.Location.CityAndCountry.Replace(",", "").Replace(" ", "")).Contains(location) || string.IsNullOrEmpty(location)) &&
                    (HasMatchingAccommodationType(accommodation, type) || string.IsNullOrEmpty(type)) &&
                    (IsGuestNumberLessThanMaximum(accommodation, guestNumber) || string.IsNullOrEmpty(guestNumber)) &&
                    (IsReservationGreaterThanMinimum(accommodation, daysForReservation) || string.IsNullOrEmpty(daysForReservation)))
                {
                    if (!filteredAccommodations.Contains(accommodation))
                        filteredAccommodations.Add(accommodation);
                }  
            }
            return filteredAccommodations;
        }

        public bool IsContainingNameWords(Accommodation accommodation, string[] nameWords)
        {
            bool containsAllWords = true;

            foreach (string word in nameWords)
            {
                if (!accommodation.Name.ToLower().Contains(word))
                {
                    containsAllWords = false;
                    break;
                }
            }
            return containsAllWords;
        }

        public bool HasMatchingAccommodationType(Accommodation accommodation, string type)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(type))
            {
                result = accommodation.Type.ToString().ToLower().Contains(type.ToLower());
            }
            return result;
        }

        public bool IsGuestNumberLessThanMaximum(Accommodation accommodation, string guestNumber)
        {
            bool isLess = false;
            if (int.TryParse(guestNumber, out int parsedGuestNumber) && parsedGuestNumber <= accommodation.MaxGuestNumber)
            {
                isLess = true;
            }
            return isLess;
        }

        public bool IsReservationGreaterThanMinimum(Accommodation accommodation, string daysForReservation)
        {
            bool isGreater = false;
            if (int.TryParse(daysForReservation, out int parsedDaysForReservation) && parsedDaysForReservation >= accommodation.MinReservationDays)
            {
                isGreater = true;
            }
            return isGreater;
        }

    }
}
