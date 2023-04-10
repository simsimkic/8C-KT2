using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for SecondGuestView.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private readonly GuideRepository _guideRepository;
        public Tour SelectedTour { get; set; }
        public Guide Guide { get; set; }    
        public List<Guest> Guests { get; set; }
        public Window1(Guide guide)
        {
            InitializeComponent();
            this.Guide = guide;
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddTourView_CLick(object sender, RoutedEventArgs e)
        {
            AddTour addTour = new AddTour(Guide);
            addTour.Show();
        }
        private void TourOverview_Click(object sender, RoutedEventArgs e)
        {
            TourOverview overView = new TourOverview(SelectedTour);
            overView.Show();
            Close();
        }
       private void ThisWeekTours_Click(object sender, RoutedEventArgs e)
        {
           
            MyTours myTours = new MyTours(SelectedTour,Guide);
            myTours.Show();
            Close();
        }

        private void PastTours_Click(object sender, RoutedEventArgs e)
        {

            PastTours pastTours = new PastTours(SelectedTour, Guide);
            pastTours.Show();
            Close();
        }

        private void MostVisited_Click(object sender, RoutedEventArgs e)
        {
            MostVisited mostVisited = new MostVisited(Guide);
            mostVisited.Show();
            Close();
        }

    }
}