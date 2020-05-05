using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace ServiceLayer.AuthorService
{
    public static class AuthorListDtoSelect
    {
        public static IQueryable<AuthorListDto> MapAuthorToDto(this IQueryable<Author> authors)
        {
            return authors.Select(author =>
                new AuthorListDto
                {
                    AuthorId = author.AuthorId,
                    Name = author.Name,
                    MailingAddress = author.MailingAddress,
                    EmailAddress = author.EmailAddress,
                    Affiliation = author.Affiliation,
                    Username = author.Username,
                    Password = author.Password,
                });
        }
    }
}
