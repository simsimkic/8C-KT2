using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;
using TravelService.Model;

namespace TravelService.Repository
{
    public class OwnerRatingRepository
    {

        private const string FilePath = "../../../Resources/Data/ownerRatings.csv";

        private readonly Serializer<OwnerRating> _serializer;

        public Guest1Repository _guestRepository;

        private List<OwnerRating> _ownerRatings;

        public OwnerRatingRepository()
        {
            _serializer = new Serializer<OwnerRating>();
            _ownerRatings = _serializer.FromCSV(FilePath);
            _guestRepository = new Guest1Repository();
        }

        public List<OwnerRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerRating Save(OwnerRating ownerRating)
        {
            ownerRating.Id = NextId();
            _ownerRatings = _serializer.FromCSV(FilePath);
            _ownerRatings.Add(ownerRating);
            _serializer.ToCSV(FilePath, _ownerRatings);
            return ownerRating;
        }

        public int NextId()
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            if (_ownerRatings.Count < 1)
            {
                return 1;
            }
            return _ownerRatings.Max(c => c.Id) + 1;
        }

        public int GetNumberOfRatings(int ownerId)
        {
            int ratingCount = 0;
            foreach(OwnerRating rating in  _ownerRatings)
            {
                if(rating.Id == ownerId)
                {
                    ratingCount++;
                }
            }
            return ratingCount;
        }
        public double GetAverageRating(int ownerId)
        {
            int ratingCount = 0;
            double sumRatings = 0;
  
            foreach (OwnerRating rating in _ownerRatings)
            {
                double averageRating = 0;
                if (rating.Id == ownerId)
                {
                    ratingCount++;
                    averageRating = (rating.Cleanliness + rating.Comfort + rating.Correctness + rating.Content + rating.Location) / 5;
                    sumRatings += averageRating;
                }
            }
            return (double)sumRatings / ratingCount;
        }
        public void Delete(OwnerRating ownerRating)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            OwnerRating founded = _ownerRatings.Find(o => o.Id == ownerRating.Id);
            _ownerRatings.Remove(founded);
            _serializer.ToCSV(FilePath, _ownerRatings);
        }

        public List<Guest1> FindGuestsByAccommodation(Accommodation selectedAccommodation)
        {
            List<Guest1> guests = new List<Guest1>();

            foreach(OwnerRating ownerRating in _ownerRatings)
            {
                if(ownerRating.AccommodationId == selectedAccommodation.Id)
                {
                    Guest1 guest = _guestRepository.FindById(ownerRating.GuestId);
                    guests.Add(guest);
                }
            }

            return guests;
        }

        public OwnerRating FindById(int id)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            foreach (OwnerRating ownerRating in _ownerRatings)
            {
                if (ownerRating.Id == id)
                {
                    return ownerRating;
                }
            }
            return null;
        }

        public OwnerRating FindByGuestOwnerIds(int guestId, int ownerId, int accommodationId)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            foreach (OwnerRating ownerRating in _ownerRatings)
            {
                if (ownerRating.GuestId == guestId && ownerRating.OwnerId==ownerId && ownerRating.AccommodationId==accommodationId)
                {
                    return ownerRating;
                }
            }
            return null;
        }

        public OwnerRating Update(OwnerRating ownerRating)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            OwnerRating current = _ownerRatings.Find(o => o.Id == ownerRating.Id);
            int index = _ownerRatings.IndexOf(current);
            _ownerRatings.Remove(current);
            _ownerRatings.Insert(index, ownerRating);
            _serializer.ToCSV(FilePath, _ownerRatings);
            return ownerRating;
        }
    }
}
