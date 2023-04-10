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
    public class GuideRepository
    {
        private const string FilePath = "../../../Resources/Data/guides.csv";

        private readonly Serializer<Guide> _serializer;

        private List<Guide> _guides;

        public GuideRepository()
        {
            _serializer = new Serializer<Guide>();
            _guides = _serializer.FromCSV(FilePath);
        }

        public Guide GetByUsername(string username)
        {
            _guides = _serializer.FromCSV(FilePath);
            return _guides.FirstOrDefault(u => u.Username == username);
        }

    }
}
