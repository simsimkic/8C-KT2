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
    /// Interaction logic for EnterCheckPoint.xaml
    /// </summary>
    public partial class EnterCheckPoint : Window
    {
        public int TourId;
        private readonly CheckPointRepository _repositoryCheckPoint;
        private readonly TourRepository _repositoryTour;
        public EnterCheckPoint(int Id)
        {
            InitializeComponent();
            DataContext = this;
            _repositoryCheckPoint = new CheckPointRepository();
            _repositoryTour = new TourRepository();
            TourId = Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _checkPoint;
        public string CheckPoint
        {
            get => _checkPoint;
            set
            {
                if (value != _checkPoint)
                {
                    _checkPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CheckPoint checkPoint = new CheckPoint();
            checkPoint.Name = CheckPoint;
            checkPoint.TourId = TourId;
            CheckPoint savedCheckPoint = _repositoryCheckPoint.Save(checkPoint);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
