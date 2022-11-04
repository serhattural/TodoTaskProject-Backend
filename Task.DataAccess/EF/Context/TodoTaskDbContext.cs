using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TodoTask.DataAccess.Entities;

namespace TodoTask.DataAccess.EF.Context
{
    public class TodoTaskDbContext : DbContext
    {
        public TodoTaskDbContext() 
        { 
        }

        public TodoTaskDbContext(DbContextOptions<TodoTaskDbContext> options) : base(options)
        {

        }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
    }
}
