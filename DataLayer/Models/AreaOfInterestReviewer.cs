using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class AreaOfInterestReviewer
    {
        public int AreaOfInterestReviewerId { get; set; }
        public int ReviewerId { get; set; }
        public int AreaOfInterestId { get; set; }
        public AreaOfInterest AreaOfInterest { get; set; }
        public Reviewer Reviewer { get; set; }

    }
}
