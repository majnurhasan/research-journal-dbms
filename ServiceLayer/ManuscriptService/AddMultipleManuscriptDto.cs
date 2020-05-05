using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ManuscriptService
{
    public class AddMultipleManuscriptDto
    {
        public string ManuscriptTitle { get; set; }
        public IList<int> AuthorsId { get; set; }

        public AddMultipleManuscriptDto()
        {
            AuthorsId = new List<int>();
        }
    }
}
