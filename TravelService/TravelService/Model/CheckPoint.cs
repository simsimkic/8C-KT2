using System;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class CheckPoint : ISerializable
    {
        public int CheckPointId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int TourId { get; set; }

        public CheckPoint() { }

        public CheckPoint(int checkPointId, string name, bool active, int tourId)
        {
            CheckPointId = checkPointId;
            Name = name;
            Active = active;
            TourId = tourId;
        }

        public override string ToString()
        {
            return CheckPointId + ";" + Name;

        }

        public string[] ToCSV()
        {
            string[] csvValues = { CheckPointId.ToString(), Name, TourId.ToString(), Active.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            CheckPointId = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);
            Active = Boolean.Parse(values[3]);
        }
    }
}