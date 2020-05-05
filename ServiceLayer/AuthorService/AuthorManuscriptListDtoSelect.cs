using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace ServiceLayer.AuthorService
{
    public static class AuthorManuscriptListDtoSelect
    {
        public static IQueryable<AuthorManuscriptListDto> MapAuthorManuscriptToDto(this IQueryable<AuthorManuscript> authorManuscripts)
        {
            return authorManuscripts.Select(authorManuscript =>
                new AuthorManuscriptListDto
                {
                    AuthorManuscriptId = authorManuscript.AuthorManuscriptId,
                    AuthorId = authorManuscript.AuthorId,
                    ManuscriptId = authorManuscript.ManuscriptId,
                    AuthorOrder = authorManuscript.AuthorOrder,
                });
        }
    }
}
