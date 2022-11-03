using Microsoft.EntityFrameworkCore;
using System;
using TodoTask.DataAccess;
using TodoTask.Service;

namespace TodoTask.Api.Extensions
{
    public static class ServiceConfiguration
    {
        public static void AddTodoTaskServices(this IServiceCollection services)
        {
            services.AddDbContext<TodoTaskDbContext>(x => x.UseInMemoryDatabase("TodoTaskDB"));
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            services.AddScoped<ITodoItemQueryService, TodoItemQueryService>();
        }
    }
}
