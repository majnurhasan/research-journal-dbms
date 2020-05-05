using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class AuthorManuscript
    {
        public int AuthorManuscriptId { get; set; }
        public int AuthorId { get; set; }
        public int ManuscriptId { get; set; }
        public int AuthorOrder { get; set; }
        public Author Author { get; set; }
        public Manuscript Manuscript { get; set; }

    }
}
