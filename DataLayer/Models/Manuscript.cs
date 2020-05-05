using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Manuscript
    {
        public int ManuscriptId { get; set; }
        public int EditorId { get; set; }
        public int IssueId { get; set; }
        public string ManuscriptTitle { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DateAccepted { get; set; }
        public int ManuscriptStatus { get; set; }
        public int NumberOfPagesOccupied { get; set; }
        public int OrderInIssue { get; set; }
        public int BeginningPageNumber { get; set; }
        public ICollection<AuthorManuscript> AuthorManuscripts { get; set; }
        public Issue Issue { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Editor Editor { get; set; }
        
    }
}
