using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.AuthorService
{
    public class ListAuthorService
    {
        private readonly EfCoreContext _context;

        public ListAuthorService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<AuthorListDto> GetAuthorList()
        {
            var authors = _context.Authors
                .AsNoTracking()
                .MapAuthorToDto();
            return authors;
        }
    }
}
