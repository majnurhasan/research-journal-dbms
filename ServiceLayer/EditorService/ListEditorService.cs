using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.AuthorService;

namespace ServiceLayer.EditorService
{
    public class ListEditorService
    {
        private readonly EfCoreContext _context;

        public ListEditorService(EfCoreContext context)
        {
            _context = context;
        }

        public IQueryable<EditorListDto> GetEditorList()
        {
            var editors = _context.Editors
                .AsNoTracking()
                .MapEditorToDto();
            return editors;
        }
    }
}
