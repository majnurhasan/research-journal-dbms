using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ReviewerId { get; set; }
        public int ManuscriptId { get; set; }
        public DateTime DateManuscriptReceived { get; set; }
        public int AppropriatenessScore { get; set; }
        public int ClarityScore { get; set; }
        public int MethodologyScore { get; set; }
        public int ContributionScore { get; set; }
        public bool RecommendationStatus { get; set; }
        public DateTime DateReviewed { get; set; }
        public Manuscript Manuscript { get; set; }
        public Reviewer Reviewer { get; set; }
    }
}
