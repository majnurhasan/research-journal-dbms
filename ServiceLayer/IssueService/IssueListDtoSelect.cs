using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace ServiceLayer.IssueService
{
    public static class IssueListDtoSelect
    {
        public static IQueryable<IssueListDto> MapIssueToDto(this IQueryable<Issue> issues)
        {
            return issues.Select(issue =>
                new IssueListDto
                {
                    IssueId = issue.IssueId,
                    PublicationPeriod = issue.PublicationPeriod,
                    PublicationYear = issue.PublicationYear,
                    VolumeNumber = issue.VolumeNumber,
                    IssueNumber = issue.IssueNumber,
                    PrintDate = issue.PrintDate,
                });
        }
    }
}
