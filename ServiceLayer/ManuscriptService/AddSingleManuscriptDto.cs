using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ManuscriptService
{
    public class AddSingleManuscriptDto
    {
        public int EditorId { get; set; }
        public string ManuscriptTitle { get; set; }
        public DateTime DateReceived { get; set; }
        public int ManuscriptStatus { get; set; }
    }
}
