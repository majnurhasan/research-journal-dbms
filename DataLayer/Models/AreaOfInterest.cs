using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class AreaOfInterest
    {
        public int AreaOfInterestId { get; set; }
        public string ISCode { get; set; }
        public string Description { get; set; }
        public ICollection<AreaOfInterestReviewer> AreaOfInterestReviewers { get; set; }
    }
}
