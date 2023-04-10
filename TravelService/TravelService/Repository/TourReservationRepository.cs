using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using TravelService.Model;
using TravelService.Serializer;
using TravelService.View;

namespace TravelService.Repository
{
    public class TourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservation.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _reservation;
        public Tour SelectedTour { get; set; }

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _reservation = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourReservation Save(TourReservation reservations)
        {
            reservations.Id = NextId();
            _reservation = _serializer.FromCSV(FilePath);
            _reservation.Add(reservations);
            _serializer.ToCSV(FilePath, _reservation);
            return reservations;
        }

        public int NextId()
        {
            _reservation = _serializer.FromCSV(FilePath);
            if (_reservation.Count < 1)
            {
                return 1;
            }
            return _reservation.Max(c => c.Id) + 1;
        }

        public void Delete(TourReservation reservations)
        {
            _reservation = _serializer.FromCSV(FilePath);
            TourReservation founded = _reservation.Find(c => c.Id == reservations.Id);
            _reservation.Remove(founded);
            _serializer.ToCSV(FilePath, _reservation);
        }

        public TourReservation Update(TourReservation reservations)
        {
            _reservation = _serializer.FromCSV(FilePath);
            TourReservation current = _reservation.Find(c => c.Id == reservations.Id);
            int index = _reservation.IndexOf(current);
            _reservation.Remove(current);
            _reservation.Insert(index, reservations);
            _serializer.ToCSV(FilePath, _reservation);
            return reservations;
        }

        public void FindActiveTourList(Tour tour, List<Tour> ActiveTours)
        {
            if (IsInPorgress(tour))
            {
                AddActiveTours(tour, ActiveTours);
            }
        }
        public void AddActiveTours(Tour tour, List<Tour> ActiveTours)
        {
            ActiveTours.Add(tour);
        }
        public bool IsInPorgress(Tour tour)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (tour.TourStart.Date == currentDate)
            {
                return true;
            }
            return false;
        }

        public List<Tour> showAllActiveTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours)
        {

            foreach (Tour tour in Tours)
            {
                List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
                tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                tour.CheckPoints.Clear();
                ListCheckPoints.Clear();

                int currentId = tour.Id;
                foreach (CheckPoint c in CheckPoints)
                {
                    int currentCheckPointTourId = c.TourId;
                    if ((currentCheckPointTourId == currentId))
                    {
                        ListCheckPoints.Add(c);

                    }
                }

                tour.CheckPoints.AddRange(ListCheckPoints);
                FindActiveTourList(tour, ActiveTours);
            }
            return ActiveTours;
        }

        public bool TryReserving(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {
            int number = int.Parse(numberOfGuests);
            if (number <= 0)
            {
                MessageBox.Show("Inserted number of people is not valid.");
                return false;
            }
            else
            {
                if (ReservationSuccess(selectedTour, numberOfGuests, TourReservations, ReservationsByTour, OtherTours, tourReservationView, guest2))
                {
                    MessageBox.Show("You have successfully booked a tour!");
                    return true;
                }
            }
            return false;
        }

        public bool ReservationSuccess(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {

            TourReservation lastReservation = FindLastCurrentReservation(selectedTour.Id, TourReservations, ReservationsByTour, guest2);

            if (TourReservations.Count() == 0 || lastReservation == null)
            {
                if (int.Parse(numberOfGuests) <= selectedTour.MaxGuestNumber)
                {
                    SaveValidReservation(selectedTour, numberOfGuests, TourReservations, guest2);
                    return true;
                }
                else
                {
                    MessageBox.Show("There is " + selectedTour.MaxGuestNumber + " more available spots!");
                    return false;
                }
            }
            else
            {
                if (SuccessOfTheLastReservation(selectedTour, numberOfGuests, TourReservations, ReservationsByTour, OtherTours, tourReservationView, guest2))
                    return true;
                else
                    return false;
            }
        }

        public bool SuccessOfTheLastReservation(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, List<Tour> OtherTours, TourReservationView tourReservationView, Guest2 guest2)
        {
            TourReservation lastReservation = FindLastCurrentReservation(selectedTour.Id, TourReservations, ReservationsByTour, guest2);
            string lastGuestNumber = lastReservation.GuestNumber.ToString();
            foreach (TourReservation tourReservation in TourReservations)
            {
                if (tourReservation.Id == lastReservation.Id)
                {
                    if (tourReservation.TourId == selectedTour.Id)
                    {
                        if (int.Parse(numberOfGuests) <= tourReservation.GuestNumber)
                        {
                            SaveSameReservation(selectedTour, tourReservation, numberOfGuests, TourReservations, guest2);
                            return true;
                        }
                        else
                        {
                            if (tourReservation.GuestNumber == 0)
                            {
                                FullyBookedTour(selectedTour, OtherTours, tourReservationView);
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("There is " + tourReservation.GuestNumber + " more available spots!");
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private TourReservation FindLastCurrentReservation(int tourId, List<TourReservation> TourReservations, List<TourReservation> ReservationsByTour, Guest2 guest2)
        {
            foreach (TourReservation tourReservation in TourReservations)
                if (tourReservation.TourId == tourId)
                    ReservationsByTour.Add(tourReservation);
            if (ReservationsByTour.Count() == 0)
            {
                return null;
            }
            else
            {
                TourReservation lastReservation = ReservationsByTour.Last();
                return lastReservation;
            }
        }

        public bool FullyBookedTour(Tour selectedTour, List<Tour> OtherTours, TourReservationView tourReservationView)
        {
            MessageBox.Show("Selected tour is fully booked. Try other tours on this location! ");
            tourReservationView.FindOtherTours(selectedTour);
            return true;
        }

        private void SaveSameReservation(Tour selectedTour, TourReservation reservation, string numberOfGuests, List<TourReservation> TourReservations, Guest2 guest2)
        {
            if (guest2.Id == reservation.GuestId)
                TourReservations.Remove(reservation);
            int newGuestNumber = reservation.GuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(NextId(), selectedTour.Id, newGuestNumber, guest2.Id);
            TourReservations.Add(tourReservation);
            Save(tourReservation);
            _serializer.ToCSV(FilePath, TourReservations);
        }

        private void SaveValidReservation(Tour selectedTour, string numberOfGuests, List<TourReservation> TourReservations, Guest2 guest2)
        {
            int newGuestNumber = selectedTour.MaxGuestNumber - int.Parse(numberOfGuests);
            TourReservation tourReservation = new TourReservation(NextId(), selectedTour.Id, newGuestNumber, guest2.Id);
            TourReservations.Add(tourReservation);
            Save(tourReservation);
            _serializer.ToCSV(FilePath, TourReservations);
        }

        public List<TourReservation> getGuestsReservations(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours, List<TourReservation> TourReservations, Guest2 guest2)
        {
            List<TourReservation> guestReservations = new List<TourReservation>();
            foreach (TourReservation tourReservation in TourReservations)
            {
                Tour currentTour = null;
                if (tourReservation.GuestId == guest2.Id)
                {
                    guestReservations.Add(tourReservation);
                }
            }
            return guestReservations;
        }

        public List<Tour> showGuestsTours(List<Tour> Tours, List<Location> Locations, List<Language> Languages, List<CheckPoint> CheckPoints, List<Tour> ActiveTours, List<TourReservation> TourReservations, Guest2 guest2)
        {
            List<TourReservation> guestReservations = getGuestsReservations(Tours, Locations, Languages, CheckPoints, ActiveTours, TourReservations, guest2);
            foreach (Tour tour in Tours)
            {
                TourReservation currentReservation = guestReservations.Find(tourReservation => tourReservation.TourId == tour.Id);
                if (currentReservation != null)
                {
                    List<CheckPoint> ListCheckPoints = new List<CheckPoint>();
                    tour.Location = Locations.Find(loc => loc.Id == tour.LocationId);
                    tour.Language = Languages.Find(lan => lan.Id == tour.LanguageId);

                    tour.CheckPoints.Clear();
                    ListCheckPoints.Clear();

                    int currentId = tour.Id;
                    foreach (CheckPoint c in CheckPoints)
                    {
                        int currentCheckPointTourId = c.TourId;
                        if ((currentCheckPointTourId == currentId))
                        {
                            ListCheckPoints.Add(c);

                        }
                    }

                    tour.CheckPoints.AddRange(ListCheckPoints);
                    FindActiveTourList(tour, ActiveTours);
                }
            }
            return ActiveTours;
        }
    }
}
