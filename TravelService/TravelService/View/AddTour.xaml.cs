using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
//using System.Windows.Shapes;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for AddTour.xaml
    /// </summary>
    public partial class AddTour : Window, INotifyPropertyChanged
    {



        private readonly TourRepository _repositoryTour;
        private readonly LocationRepository _repositoryLocation;
        private readonly LanguageRepository _repositoryLanguage;
        private readonly CheckPointRepository _repositoryCheckPoint;

        public int TourId;
        public Guide Guide;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _location;

        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _tourLanguage;

        public string TourLanguage
        {
            get => _tourLanguage;
            set
            {
                if (value != _tourLanguage)
                {
                    _tourLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxguestnumber;

        public int MaxGuestNumber
        {
            get => _maxguestnumber;
            set
            {
                if (value != _maxguestnumber)
                {
                    _maxguestnumber = value;
                    OnPropertyChanged();
                }
            }
        }




        private DateTime _tourStart;

        public DateTime TourStart
        {
            get => _tourStart;
            set
            {
                if (value != _tourStart)
                {
                    _tourStart = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _duration;

        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _pictures;

        public string Pictures
        {
            get => _pictures;
            set
            {
                if (value != _pictures)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value != _done)
                {
                    _done = value;
                    OnPropertyChanged();
                }
            }
        }




        public AddTour(Guide guide)
        {
            InitializeComponent();
            DataContext = this;
            this.Guide = guide;
            _repositoryTour = new TourRepository();
            _repositoryLanguage = new LanguageRepository();
            _repositoryLocation = new LocationRepository();
            _repositoryCheckPoint = new CheckPointRepository();

            TourId = _repositoryTour.NextId();

        }



        private void AddTour_Click(object sender, RoutedEventArgs e)
        {

            InitializeComponent();
            DataContext = this;

            string[] words = _location.Split(',');

            string city = words[0];
            string country = words[1];

            Location location = new Location(country, city);
            Location savedLocation = _repositoryLocation.Save(location);


            Language language = new Language(TourLanguage);
            Language savedLanguage = _repositoryLanguage.Save(language);


            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }



            Tour tour = new Tour(Guide.Id,TourName, savedLocation,savedLocation.Id, Description, savedLanguage, savedLanguage.Id, MaxGuestNumber,  TourStart, Duration, formattedPictures, Done);




            List<CheckPoint> checkPoints = _repositoryCheckPoint.GetAll();
            _repositoryTour.Check(checkPoints, tour, TourId);

        }


        private void CheckPoint_Click(object sender, RoutedEventArgs e)
        {
            EnterCheckPoint enterCheckPoint = new EnterCheckPoint(TourId);
            enterCheckPoint.Show();
        }

        private void findPictures_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png;*.jfif)|*.jpg;*.jpeg;*.png;*.jfif";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                string destinationFolder = @"../../../Resources/Images/";

                if(!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach(string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                    string destinationFilePath = System.IO.Path.Combine(destinationFolder, Path.GetFileName(file));
                    File.Copy(file, destinationFilePath);
                   
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}