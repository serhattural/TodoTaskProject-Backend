using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTask.Model.Dtos;
using TodoTask.DataAccess;

namespace TodoTask.Service
{
    public class TodoItemQueryService : ITodoItemQueryService
    {
        private readonly TodoTaskDbContext dbContext;
        private readonly IDateTimeProvider dateTimeProvider;

        public TodoItemQueryService(TodoTaskDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            this.dbContext = dbContext;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<TodoItemDto> GetById(int id)
        {
            var dto = await dbContext.TodoItems.Where(x => x.Id == id)
                .Select( x => new TodoItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    IsComplated = x.IsComplated
                })
                .SingleOrDefaultAsync();

            return dto;
        }

        public async Task<List<TodoItemDto>> GetOverdueTodoItems()
        {
            var query = dbContext.TodoItems.Where(x => !x.IsComplated
                            && x.DueDate.HasValue
                            && x.DueDate.Value.Date < dateTimeProvider.Now.Date)
                            .Select(x => new TodoItemDto
                            {
                                Id = x.Id,
                                Title = x.Title,
                                DueDate = x.DueDate,
                                IsComplated = x.IsComplated
                            });

            var list = await query.ToListAsync();

            return list;
        }

        public async Task<List<TodoItemDto>> GetPendingTodoItems()
        {
            var query = dbContext.TodoItems.Where(x => !x.IsComplated
                            && (!x.DueDate.HasValue || x.DueDate.Value.Date >= dateTimeProvider.Now.Date))
                            .Select(x => new TodoItemDto
                            {
                                Id = x.Id,
                                Title = x.Title,
                                DueDate = x.DueDate,
                                IsComplated = x.IsComplated
                            });

            var list = await query.ToListAsync();

            return list;
        }
    }
}
