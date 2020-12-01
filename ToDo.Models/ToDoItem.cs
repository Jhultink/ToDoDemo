using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}
