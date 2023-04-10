using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guide : User, ISerializable
    {
        
        public List<Tour> Tours { get; set; }
        public Guide()
        {
            Tours = new List<Tour>();
        }

        public Guide(string username, string password, string userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
            Tours = new List<Tour>();
             
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType};
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
