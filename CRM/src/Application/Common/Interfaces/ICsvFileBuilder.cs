using CRM.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace CRM.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
