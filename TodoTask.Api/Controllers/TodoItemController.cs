using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoTask.Model.Dtos;
using TodoTask.Model.Entities;
using TodoTask.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ILogger<TodoItemController> logger;
        private readonly ITodoItemService todoItemService;
        private readonly ITodoItemQueryService todoItemQueryService;
        private readonly IDateTimeProvider dateTimeProvider;

        public TodoItemController(ILogger<TodoItemController> logger, 
                                ITodoItemService todoItemService,
                                ITodoItemQueryService todoItemQueryService,
                                IDateTimeProvider dateTimeProvider)
        {
            this.logger = logger;
            this.todoItemService = todoItemService;
            this.todoItemQueryService = todoItemQueryService;
            this.dateTimeProvider = dateTimeProvider;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateTodoItem(TodoItemCreateDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Title))
            {
                return BadRequest("Title is empty");
            }
            //if (createDto.DueDate.HasValue && createDto.DueDate.Value.Date.Date < dateTimeProvider.Now.Date)
            //{
            //    return BadRequest("Due date should be bigger than current date");
            //}

            var newDto = await todoItemService.CreateTodoItem(createDto);
           
            return newDto;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoItem(int id, TodoItemEditDto editDto)
        {
            if (id != editDto.Id) 
            {
                return BadRequest("Parameter id and TodoItem id does not match");
            }
            if (string.IsNullOrWhiteSpace(editDto.Title))
            {
                return BadRequest("Title is empty");
            }

            var dto = await todoItemQueryService.GetById(id);
            if (dto is null)
            {
                return NotFound();
            }

            await todoItemService.EditTodoItem(editDto);

            return NoContent();
        }

        [HttpGet("pending")]
        public async Task<List<TodoItemDto>> GetPendingTodoItems()
        {
            var list = await todoItemQueryService.GetPendingTodoItems();
            
            return list;
        }

        [HttpGet("overdue")]
        public async Task<List<TodoItemDto>> GetOverdueTodoItems()
        {
            var list = await todoItemQueryService.GetOverdueTodoItems();

            return list;
        }
    }
}