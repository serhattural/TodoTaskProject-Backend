using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTask.Model.Dtos;
using TodoTask.Model.Entities;
using TodoTask.DataAccess;

namespace TodoTask.Service
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoTaskDbContext dbContext;

        public TodoItemService(TodoTaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TodoItemDto> CreateTodoItem(TodoItemCreateDto dto)
        {
            TodoItem entity = new TodoItem();
            entity.Title = dto.Title;
            entity.DueDate = dto.DueDate;
            entity.IsComplated = dto.IsComplated;

            await dbContext.TodoItems.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return new TodoItemDto
            {
                Id = entity.Id,
                Title = entity.Title,
                DueDate = entity.DueDate,
                IsComplated = entity.IsComplated
            };
        }

        public async Task EditTodoItem(TodoItemEditDto dto)
        {
            TodoItem entity = await dbContext.TodoItems.SingleOrDefaultAsync(x => x.Id == dto.Id);
            
            // There can be better ways to handle this. For simplicity to this task return.
            if (entity is null)
                return; 

            entity.Title = dto.Title;
            entity.DueDate = dto.DueDate;
            entity.IsComplated = dto.IsComplated;

            await dbContext.SaveChangesAsync();
        }
    }
}
