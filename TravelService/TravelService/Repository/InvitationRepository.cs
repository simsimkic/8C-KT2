using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class InvitationRepository
    {
        private const string FilePath = "../../../Resources/Data/invitation.csv";

        private readonly Serializer<Invitation> _serializer;

        private List<Invitation> _invitation;

        public InvitationRepository()
        {
            _serializer = new Serializer<Invitation>();
            _invitation = _serializer.FromCSV(FilePath);
        }

        public List<Invitation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Invitation Save(Invitation invitation)
        {
            invitation.Id = NextId();
            _invitation = _serializer.FromCSV(FilePath);
            _invitation.Add(invitation);
            _serializer.ToCSV(FilePath, _invitation);
            return invitation;
        }

        public int NextId()
        {
            _invitation = _serializer.FromCSV(FilePath);
            if (_invitation.Count < 1)
            {
                return 1;
            }
            return _invitation.Max(c => c.Id) + 1;
        }

        public void Delete(Invitation invitation)
        {
            _invitation = _serializer.FromCSV(FilePath);
            Invitation founded = _invitation.Find(c => c.Id == invitation.Id);
            _invitation.Remove(founded);
            _serializer.ToCSV(FilePath, _invitation);
        }

        public Invitation Update(Invitation invitation)
        {
            _invitation = _serializer.FromCSV(FilePath);
            Invitation current = _invitation.Find(c => c.Id == invitation.Id);
            int index = _invitation.IndexOf(current);
            _invitation.Remove(current);
            _invitation.Insert(index, invitation);
            _serializer.ToCSV(FilePath, _invitation);
            return invitation;
        }

        public void confirmInvitation(List<Invitation> Invitations, Guest2 guest2)
        {
            foreach (Invitation invitation in Invitations)
            {
                if (invitation.GuestId == guest2.Id)
                {
                    invitation.GuestAttendence = true;
                    Update(invitation);
                }

            }
        }

    }

}

