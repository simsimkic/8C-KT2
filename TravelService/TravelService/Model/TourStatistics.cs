using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService.Model
{
    public class TourStatistics
    {
        public int TourId { get; set; }
        public int Under18Count { get; set; }
        public int Between18And50Count { get; set; }
        public int Over50Count { get; set; }
        public double WithVoucherPercentage { get; set; }
        public double WithoutVoucherPercentage { get; set; }


        public TourStatistics() { }

        public TourStatistics(int tourId, int under18Count, int between18And50Count, int over50Count, double withVoucherPercentage, double withoutVoucherPercentage)
        {
            TourId = tourId;
            Under18Count = under18Count;
            Between18And50Count = between18And50Count;
            Over50Count = over50Count;
            WithVoucherPercentage = withVoucherPercentage;
            WithoutVoucherPercentage = withoutVoucherPercentage;

        }
    }

    
}
