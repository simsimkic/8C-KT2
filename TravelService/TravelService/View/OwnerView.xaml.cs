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
    /// Interaction logic for OwnerView.xaml
    /// </summary>
    public partial class OwnerView : Window
    {
        public Owner Owner { get; set; }

        private bool _isSuperOwner;
        public bool IsSuperOwner
        {
            get => _isSuperOwner;
            set
            {
                if (value != _isSuperOwner)
                {
                    _isSuperOwner = value;
                    OnPropertyChanged(nameof(IsSuperOwner));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerView(Owner owner)
        {
            this.Owner = owner;
            IsSuperOwner = owner.SuperOwner;
            InitializeComponent();
            DataContext = this;
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            AddAccommodation addAccommodation = new AddAccommodation(Owner);
            addAccommodation.Show();
        }

        private void GuestRating_Click(object sender, RoutedEventArgs e)
        {
            GuestRatingOverview ratingOverview = new GuestRatingOverview(Owner);
            ratingOverview.Show();
        }

        private void ReviewSelection_Click(object sender, RoutedEventArgs e)
        {
            ReviewsSelectionView reviewSelection = new ReviewsSelectionView(Owner);
            reviewSelection.Show();
        }
    }
}
