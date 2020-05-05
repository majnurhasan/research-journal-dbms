using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using DataLayer.Models;

namespace ServiceLayer.AuthorService
{
    public class ListFrontAuthorService
    {
        private EfCoreContext _context;

        public ListFrontAuthorService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<Author> GetFrontAuthors()
        {
            return _context.Authors;
        }
    }
}
