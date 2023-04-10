using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class CheckPointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpoint.csv";

        private readonly Serializer<CheckPoint> _serializer;

        private List<CheckPoint> _checkpoints;

        public CheckPointRepository()
        {
            _serializer = new Serializer<CheckPoint>();
            _checkpoints = _serializer.FromCSV(FilePath);
        }

        public List<CheckPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public CheckPoint Save(CheckPoint checkpoint)
        {
            checkpoint.CheckPointId = NextId();
            _checkpoints = _serializer.FromCSV(FilePath);
            _checkpoints.Add(checkpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
            return checkpoint;
        }

        public int NextId()
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            if (_checkpoints.Count < 1)
            {
                return 1;
            }
            return _checkpoints.Max(c => c.CheckPointId) + 1;
        }

        public void Delete(CheckPoint checkpoint)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            CheckPoint founded = _checkpoints.Find(c => c.CheckPointId == checkpoint.CheckPointId);
            _checkpoints.Remove(founded);
            _serializer.ToCSV(FilePath, _checkpoints);
        }

        public CheckPoint Update(CheckPoint checkpoint)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            CheckPoint current = _checkpoints.Find(c => c.CheckPointId == checkpoint.CheckPointId);
            int index = _checkpoints.IndexOf(current);
            _checkpoints.Remove(current);
            _checkpoints.Insert(index, checkpoint);       
            _serializer.ToCSV(FilePath, _checkpoints);
            return checkpoint;
        }


        public void FirstCheckPointActive(List<CheckPoint> FilteredCheckPoint)
        {
            if (FilteredCheckPoint.Count > 0)
            {
                FilteredCheckPoint[0].Active = true;
            }
        }

        public CheckPoint GetById(int id)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            foreach (CheckPoint checkpoints in _checkpoints)
            {
                if (checkpoints.CheckPointId == id)
                {
                    return checkpoints;
                }
            }
            return null;
        }
    }
}
