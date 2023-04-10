using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    public partial class MostVisited : Window
    {
        private readonly TourRepository _tourRepository;
        public List<Tour> MostVisitedTours { get; set; }
        public List<Tour> MostVisitedToursInYear { get; set; }
        public static List<Location> Locations { get; set; }
        public readonly LocationRepository _locationRepository;

        public MostVisited(Guide guide)
        {
            InitializeComponent();
            DataContext = this;
            _locationRepository = new LocationRepository();
            _tourRepository = new TourRepository();
            List<Guest> guests = new List<Guest>();
            Locations = new List<Location>(_locationRepository.GetAll());

            List<Tour> guideTours = _tourRepository.FindGuidesTours(guide.Id);
            MostVisitedTours = new List<Tour> { _tourRepository.GetMostVisitedTour(guideTours, guests, Locations) };
            MostVisitedToursInYear = new List<Tour> { _tourRepository.GetMostVisitedTour(guideTours, guests,Locations, DateTime.Now.Year) };



        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
