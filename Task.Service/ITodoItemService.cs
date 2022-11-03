using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTask.Model.Dtos;

namespace TodoTask.Service
{
    public interface ITodoItemService
    {
        Task<TodoItemDto> CreateTodoItem(TodoItemCreateDto dto);
        Task EditTodoItem(TodoItemEditDto dto);
    }
}
