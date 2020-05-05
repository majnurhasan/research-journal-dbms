using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public int PublicationPeriod { get; set; }
        public DateTime PublicationYear { get; set; }
        public int VolumeNumber { get; set; }
        public int IssueNumber { get; set; }
        public DateTime PrintDate { get; set; }
        public ICollection<Manuscript> Manuscripts { get; set; }

    }
}
