using System;
using System.Collections.Generic;

namespace ToDo.Models
{
    public class ToDoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ToDoItem> Items { get; set; } = new List<ToDoItem>();
    }
}
