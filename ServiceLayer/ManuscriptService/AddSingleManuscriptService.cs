using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using DataLayer.Models;

namespace ServiceLayer.ManuscriptService
{
    public class AddSingleManuscriptService
    {
        private EfCoreContext _context;

        public AddSingleManuscriptService(EfCoreContext context)
        {
            _context = context;
        }

        public Manuscript AddSingleManuscript(AddSingleManuscriptDto dto)
        {
            Random rnd = new Random();
            var newManuscript = new Manuscript
            {
                EditorId = dto.EditorId,
                IssueId = 3,
                ManuscriptTitle = dto.ManuscriptTitle,
                DateReceived = dto.DateReceived,
                DateAccepted = new DateTime(2099,1,1),
                ManuscriptStatus = dto.ManuscriptStatus,
                NumberOfPagesOccupied = 0,
                OrderInIssue = 0,
                BeginningPageNumber = 0
            };

            _context.Manuscripts.Add(newManuscript);
            _context.SaveChanges();
            return newManuscript;
        }
    }
}
