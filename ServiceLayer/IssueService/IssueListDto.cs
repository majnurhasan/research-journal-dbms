using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.IssueService
{
    public class IssueListDto
    {
        public int IssueId { get; set; }
        public int PublicationPeriod { get; set; }
        public DateTime PublicationYear { get; set; }
        public int VolumeNumber { get; set; }
        public int IssueNumber { get; set; }
        public DateTime PrintDate { get; set; }
    }
}
