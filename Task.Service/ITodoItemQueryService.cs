using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTask.Model.Dtos;

namespace TodoTask.Service
{
    public interface ITodoItemQueryService
    {
        Task<TodoItemDto> GetById(int id);
        Task<List<TodoItemDto>> GetPendingTodoItems();
        Task<List<TodoItemDto>> GetOverdueTodoItems();
    }
}
