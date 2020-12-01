using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace Todo.Services
{
    public interface IToDoService
    {
        // Async to support future API implementations

        Task CreateList(ToDoList list);

        Task DeleteList(Guid listId);

        Task<List<ToDoList>> GetAllLists();

        Task<ToDoList> GetList(Guid listId);

        Task RemoveItem(Guid listId, Guid itemId);

        Task AddItem(Guid listId, ToDoItem item);
    }
}
