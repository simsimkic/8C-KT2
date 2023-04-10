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
    public class OwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRatingRepository _ownerRatingRepository;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
            _ownerRatingRepository = new OwnerRatingRepository();
        }

        public Owner GetByUsername(string username)
        {
            Owner owner = new Owner();
            _owners = _serializer.FromCSV(FilePath);
            owner = _owners.FirstOrDefault(u => u.Username == username);
            owner.NumberOfRatings = _ownerRatingRepository.GetNumberOfRatings(owner.Id);
            owner.AverageRating = _ownerRatingRepository.GetAverageRating(owner.Id);

            if(owner.NumberOfRatings > 50 && owner.AverageRating > 4.5)
            {
                owner.SuperOwner = true;
            }
            else
            {
                owner.SuperOwner = false;
            }

            return owner;
        }

        public Owner FindById(int id)
        {
            _owners = _serializer.FromCSV(FilePath);
            foreach (Owner owner in _owners)
            {
                if (owner.Id == id)
                {
                    return owner;
                }
            }
            return null;
        }

        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        
    }
}
