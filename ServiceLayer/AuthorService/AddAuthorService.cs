using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using DataLayer.Models;

namespace ServiceLayer.AuthorService
{
    public class AddAuthorService
    {
        private EfCoreContext _context;

        public AddAuthorService(EfCoreContext context)
        {
            _context = context;
        }

        public Author AddAuthor(AddAuthorDto dto)
        {
            var newAuthor = new Author
            {
                Name = dto.Name,
                MailingAddress = dto.MailingAddress,
                EmailAddress = dto.EmailAddress,
                Affiliation = dto.Affiliation,
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Authors.Add(newAuthor);

            newAuthor.AuthorManuscripts = new List<AuthorManuscript>();
            foreach (var i in dto.ManuscriptsId)
            {
                var authorManuscript = new AuthorManuscript
                {
                    AuthorId = newAuthor.AuthorId,
                    ManuscriptId = i,
                    AuthorOrder = 1,
                };
                newAuthor.AuthorManuscripts.Add(authorManuscript);
            }

            _context.SaveChanges();
            return newAuthor;
        }
    }
}
