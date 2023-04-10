using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class Invitation: ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public bool GuestAttendence { get; set; }
        




        public Invitation()
        {

        }


        public Invitation(int guestId, bool guestAttendence)
        {
            GuestId = guestId;
            GuestAttendence = guestAttendence;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                GuestAttendence.ToString(),
               
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            GuestAttendence=Boolean.Parse(values[2]);   
           

        }
    }
}
