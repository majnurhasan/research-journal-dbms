using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AuthorService
{
    public class AddAuthorDto
    {
        public string Name { get; set; }
        public string MailingAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Affiliation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IList<int> ManuscriptsId { get; set; }

        public AddAuthorDto()
        {
            ManuscriptsId = new List<int>();
        }
    }
}
