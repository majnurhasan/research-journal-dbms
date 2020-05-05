using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.AuthorService
{
    public class ListAuthorManuscriptService
    {
        private readonly EfCoreContext _context;

        public ListAuthorManuscriptService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<AuthorManuscriptListDto> GetAuthorManuscriptList()
        {
            var authorManuscripts = _context.AuthorManuscripts
                .AsNoTracking()
                .MapAuthorManuscriptToDto();
            return authorManuscripts;
        }
    }
}
