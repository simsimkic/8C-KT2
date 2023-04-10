using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TravelService.Model;
using TravelService.Repository;

namespace TravelService.View
{
    /// <summary>
    /// Interaction logic for OwnerRatingView.xaml
    /// </summary>
    public partial class OwnerRatingView : Window
    {
        private readonly OwnerRatingRepository _ownerRatingRepository;
        private readonly AccommodationReservationRepository _reservationRepository;
        private readonly OwnerRepository _ownerRepository;
        public AccommodationReservation SelectedUnratedOwner { get; set; }
        public Window Parent { get; set; }

        private int _correctness;
        public int Correctness
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _location;
        public int Location
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

        private int _comfort;
        public int Comfort
        {
            get => _comfort;
            set
            {
                if (value != _comfort)
                {
                    _comfort = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _content;
        public int Contents
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OwnerRatingView(AccommodationReservation selectedUnratedOwner)
        {
            InitializeComponent();
            DataContext = this;
            SelectedUnratedOwner = selectedUnratedOwner;
            _reservationRepository = new AccommodationReservationRepository();
            _ownerRatingRepository = new OwnerRatingRepository();
            _ownerRepository = new OwnerRepository();

            AccommodationName.Text = selectedUnratedOwner.AccommodationName;
            Owner owner = _ownerRepository.FindById(selectedUnratedOwner.OwnerId);
            OwnerName.Text = owner.Username;
        }

        private void AddOwnerRating_Click(object sender, RoutedEventArgs e)
        {
            List<string> formattedPictures = new List<string>();

            string[] delimitedPictures = Pictures.Split(new char[] { '|' });

            foreach (string picture in delimitedPictures)
            {
                formattedPictures.Add(picture);
            }

            OwnerRating ownerRating = new OwnerRating(SelectedUnratedOwner.Id, SelectedUnratedOwner.AccommodationId, SelectedUnratedOwner.GuestId, SelectedUnratedOwner.OwnerId, Correctness, Cleanliness, Location, Comfort, Contents, Comment, formattedPictures);
            _ownerRatingRepository.Save(ownerRating);

            AccommodationReservation ratedOwner = _reservationRepository.FindById(SelectedUnratedOwner.Id);
            ratedOwner.IsOwnerRated = true;
            _reservationRepository.Update(ratedOwner);

            RatingView ratingView = (RatingView)this.Parent;
            ratingView.UnratedOwners.Remove(SelectedUnratedOwner);

            Close();
        }

        private void addPictures_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image files (*.jpg;*.jpeg;*.png;*.jfif)|*.jpg;*.jpeg;*.png;*.jfif";
            dlg.Multiselect = true;

            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                string[] selectedFiles = dlg.FileNames;

                string destinationFolder = @"../../../Resources/Images/";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                foreach (string file in selectedFiles)
                {
                    Pictures += file;
                    Pictures += "|";
                    string destinationFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(file));
                    File.Copy(file, destinationFilePath);
                    MyListBox.Items.Add(file);
                }

                Pictures = Pictures.Substring(0, Pictures.Length - 1);

            }
        }


        private void CancelOwnerRating_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
