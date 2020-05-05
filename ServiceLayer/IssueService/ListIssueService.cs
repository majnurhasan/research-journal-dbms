using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.IssueService
{
    public class ListIssueService
    {
        private readonly EfCoreContext _context;

        public ListIssueService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<IssueListDto> GetIssueList()
        {
            var issues = _context.Issues
                .AsNoTracking()
                .MapIssueToDto();
            return issues;
        }
    }
}
