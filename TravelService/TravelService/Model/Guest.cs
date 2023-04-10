using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Repository;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guest : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int CheckPointId { get; set; }
        public int TourId { get; set; }
        public bool Attendence { get; set; }
        public int Age { get; set; }
        public List<GuestVoucher> VoucherList { get; set; }


        public Guest() 
        {
            VoucherList = new List<GuestVoucher>();
        }


        public Guest(string username, int checkPointId, int tourId, bool attendence,int age)
        {
            Username = username;
            CheckPointId = checkPointId;
            TourId = tourId;
            Attendence = attendence; 
            Age = age;  
        }

       
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, CheckPointId.ToString(),TourId.ToString(), Attendence.ToString(),Age.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Username = values[1];
            CheckPointId = int.Parse(values[2]);
            TourId = int.Parse(values[3]);
            Attendence = Boolean.Parse(values[4]);
            Age = int.Parse(values[5]);
        }
    }
}