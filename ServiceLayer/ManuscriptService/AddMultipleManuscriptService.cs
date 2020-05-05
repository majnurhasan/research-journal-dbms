using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using DataLayer.Models;

namespace ServiceLayer.ManuscriptService
{
    public class AddMultipleManuscriptService
    {
        private EfCoreContext _context;

        public AddMultipleManuscriptService(EfCoreContext context)
        {
            _context = context;
        }

        public Manuscript AddMultipleManuscript(AddMultipleManuscriptDto dto)
        {
            Random rnd = new Random();
            var newMultipleManuscript = new Manuscript
            {
                EditorId = rnd.Next(1, 6),
                IssueId = 3,
                ManuscriptTitle = dto.ManuscriptTitle,
                DateReceived = DateTime.Now,
                DateAccepted = new DateTime(2099, 1, 1),
                ManuscriptStatus = 1,
                NumberOfPagesOccupied = 0,
                OrderInIssue = 0,
                BeginningPageNumber = 0
            };

            _context.Manuscripts.Add(newMultipleManuscript);

            newMultipleManuscript.AuthorManuscripts = new List<AuthorManuscript>();
            var authorOrder = 1;
            foreach (var i in dto.AuthorsId)
            {
                var authorManuscript = new AuthorManuscript
                {
                    ManuscriptId = newMultipleManuscript.ManuscriptId,
                    AuthorId = i,
                    AuthorOrder = authorOrder,
                };
                authorOrder++;
                newMultipleManuscript.AuthorManuscripts.Add(authorManuscript);
            }

            _context.SaveChanges();
            return newMultipleManuscript;
        }
    }
}
