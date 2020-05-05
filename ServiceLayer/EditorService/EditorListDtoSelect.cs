using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace ServiceLayer.EditorService
{
    public static class EditorListDtoSelect
    {
        public static IQueryable<EditorListDto> MapEditorToDto(this IQueryable<Editor> editors)
        {
            return editors.Select(editor =>
                new EditorListDto
                {
                    EditorId = editor.EditorId,
                    Name = editor.Name,
                    Username = editor.Username,
                    Password = editor.Password,
                });
        }
    }
}
