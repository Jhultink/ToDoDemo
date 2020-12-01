using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IToDoService
    {
        // Async to support future API implementations

        Task CreateList(ToDoList list);

        Task DeleteList(Guid listId);

        Task UpdateListName(Guid listId, string name);

        Task<List<ToDoList>> GetAllLists();

        Task<ToDoList> GetList(Guid listId);

        Task DeleteItem(Guid listId, Guid itemId);

        Task AddItem(Guid listId, ToDoItem item);

        Task UpdateItemName(Guid listId, Guid itemId, string name);

    }
}
