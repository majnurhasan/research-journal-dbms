using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.AuthorService;

namespace ServiceLayer.AreaOfInterestService
{
    public class ListAreaOfInterestService
    {
        private EfCoreContext _context;

        public ListAreaOfInterestService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<AreaOfInterest> GetAreaOfInterests()
        {
            return _context.AreaOfInterests;
        }
    }
}
