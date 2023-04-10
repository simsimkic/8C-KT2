using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Owner : User, ISerializable
    {
        public List<Accommodation> Accommodations { get; set; }
        public bool SuperOwner { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
        public Owner()
        {
            Accommodations = new List<Accommodation>();
        }

        public Owner(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
            Accommodations = new List<Accommodation>();
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
