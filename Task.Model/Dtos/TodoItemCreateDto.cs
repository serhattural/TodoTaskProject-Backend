using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTask.Model.Dtos
{
    public class TodoItemCreateDto
    {
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsComplated { get; set; }
    }
}
