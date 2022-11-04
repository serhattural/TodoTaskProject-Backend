using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TodoTask.Model.Entities;

namespace TodoTask.DataAccess.EF.Configuration
{
    public class TodoItemEntityTypeConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(b => b.Title)
                   .HasMaxLength(250)
                   .IsRequired();

            //builder.Property(x => x.IsComplated)
            //       .HasDefaultValue(false);
        }
    }
}
