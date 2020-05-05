using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AuthorService
{
    public class AuthorManuscriptListDto
    {
        public int AuthorManuscriptId { get; set; }
        public int AuthorId { get; set; }
        public int ManuscriptId { get; set; }
        public int AuthorOrder { get; set; }
    }
}
