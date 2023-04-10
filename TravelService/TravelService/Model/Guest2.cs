using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Guest2 : User, ISerializable
    {
        public int Age { get; set; }    

        public Guest2(string username, string password, string userType, int age)
        {
            Username = username;
            Password = password;
            UserType = userType;
            Age = age;
        }

        public Guest2() { }


        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType, Age.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = values[3];
            Age = Convert.ToInt32(values[4]);   
        }
    }
}
