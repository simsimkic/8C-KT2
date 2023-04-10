using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guest1 : User, ISerializable
    {
        public List<AccommodationReservation> AccommodationReservations { get; set; }

        public Guest1(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }

        public Guest1() 
        {
            AccommodationReservations = new List<AccommodationReservation>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = values[3];
        }
    }
}
