using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AreaOfInterestService
{
    public class AreaOfInterestReviewerSelectionViewDto
    {
        public int AreaOfInterestId { get; set; }
        public string ISCode { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }
}
