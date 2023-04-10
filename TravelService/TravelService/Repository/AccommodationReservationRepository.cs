using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TravelService.Model;
using TravelService.Serializer;

namespace TravelService.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            _accommodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public AccommodationReservation FindById(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.Id == id)
                {
                    return reservation;
                }
            }
            return null;
        }

        public ObservableCollection<AccommodationReservation> FindUnratedReservations(AccommodationRepository _accommodationRepository, int OwnerId)
        {
            ObservableCollection<AccommodationReservation> UnratedReservations = new ObservableCollection<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                Accommodation reservedAccommodation = _accommodationRepository.FindById(reservation.AccommodationId);
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservedAccommodation.OwnerId == OwnerId)
                {
                    UnratedReservations.Add(reservation);
                }
            }

            return UnratedReservations;
        }
        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }


        public List<Tuple<DateTime, DateTime>> FindAvailableDates(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying)
        {
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
                else
                {
                    availableDates.Clear();
                }

                if (availableDates.Count == daysOfStaying)
                {
                    availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                    availableDates.RemoveAt(0);
                }
            }
            return availableDatesPair;
        }

        public List<Tuple<DateTime, DateTime>> FindAvailableDatesOutsideRange(Accommodation selectedAccommodation, DateTime startDate, DateTime endDate, int daysOfStaying)
        {
            DateTime recommendedStartDate = startDate;
            DateTime recommendedEndDate = endDate;
            List<DateTime> reservedDates = FindReservedDates(selectedAccommodation);
            List<DateTime> availableDates = new List<DateTime>();
            List<Tuple<DateTime, DateTime>> availableDatesPair = new List<Tuple<DateTime, DateTime>>();

            while (!(availableDatesPair.Count >= 5))
                 {
                     recommendedStartDate = recommendedStartDate.Equals(DateTime.Today) ? recommendedStartDate : recommendedStartDate.AddDays(-1);
                     recommendedEndDate = recommendedEndDate.AddDays(1);

                     availableDates.Clear();
                     for (DateTime date = recommendedStartDate; date <= recommendedEndDate; date = date.AddDays(1))
                     {
                         if (!reservedDates.Contains(date))
                         {
                             availableDates.Add(date);
                         }
                         else
                         {
                             availableDates.Clear();
                         }

                         if (availableDates.Count == daysOfStaying)
                         {
                             if (!availableDatesPair.Contains(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date)))
                                 availableDatesPair.Add(Tuple.Create(availableDates[0].Date, availableDates[availableDates.Count - 1].Date));
                             availableDates.RemoveAt(0);
                         }
                     }
                 }
            return availableDatesPair;
        }


        public List<DateTime> FindReservedDates(Accommodation selectedAccommodation)
        {
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (selectedAccommodation.Id == reservation.AccommodationId)
                {
                    DateTime checkIn = reservation.CheckInDate;
                    DateTime checkOut = reservation.CheckOutDate;

                    for (DateTime currentDate = checkIn; currentDate <= checkOut; currentDate = currentDate.AddDays(1))
                    {
                        reservedDates.Add(currentDate);
                    }
                }
            }
            return reservedDates;
        }


        public List<AccommodationReservation> FindUnratedOwners(int guestId)
        {
            List<AccommodationReservation> UnratedOwners = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                TimeSpan dayDifference = DateTime.Today - reservation.CheckOutDate;
                if (!reservation.IsOwnerRated && dayDifference.Days < 5 && dayDifference.Days > 0 && reservation.GuestId == guestId)
                {
                    UnratedOwners.Add(reservation);
                }
            }

            return UnratedOwners;
        }

        public void SetAccommodationForUnratedOwners(List<Accommodation> accomodations)
        {
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                reservation.Accommodation = accomodations.Find(a => a.Id == reservation.AccommodationId);
            }
        }

        public void SetLocationForUnratedOwners(List<Location> locations)
        {
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                reservation.Location = locations.Find(l => l.Id == reservation.LocationId);
            }
        }

        public void SetNameForUnratedOwners(List<Owner> owners)
        {
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                reservation.Owner = owners.Find(o => o.Id == reservation.OwnerId);
            }
        }
    }

}
