using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; } 
        public int OwnerId { get; set; }
        public int GuestId { get; set; }
        public int Cleanness { get; set; }
        public int RulesFollowing { get; set; }
        public int Communication { get; set; }
        public int NoiseLevel { get; set; }
        public int PropertyRespect { get; set; }
        public string Comment { get; set; }
        public int ReservationId { get; set; }

        public GuestRating() { }
        public GuestRating(int ownerId, int guestId, int cleanness, int rulesFollowing, int communication, int noiseLevel, int propertyRespect, string comment, int reservationId)
        {
            OwnerId = ownerId;
            GuestId = guestId;
            Cleanness = cleanness;
            RulesFollowing = rulesFollowing;
            Communication = communication;
            NoiseLevel = noiseLevel;
            PropertyRespect = propertyRespect;
            Comment = comment;
            ReservationId = reservationId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Cleanness = Convert.ToInt32(values[3]);
            RulesFollowing = Convert.ToInt32(values[4]);
            Communication = Convert.ToInt32(values[5]);
            NoiseLevel = Convert.ToInt32(values[6]);
            PropertyRespect = Convert.ToInt32(values[7]);
            Comment = values[8];
            ReservationId = Convert.ToInt32(values[9]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
           {
                Id.ToString(),
                OwnerId.ToString(),
                GuestId.ToString(),
                Cleanness.ToString(),
                RulesFollowing.ToString(),
                Communication.ToString(),
                NoiseLevel.ToString(),
                PropertyRespect.ToString(),
                Comment,
                ReservationId.ToString(),
            };
            return csvValues;
        }
    }
}
