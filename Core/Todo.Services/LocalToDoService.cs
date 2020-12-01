using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public class LocalToDoService : IToDoService
    {
        private readonly List<ToDoList> _lists = new List<ToDoList>();

        public Task CreateList(ToDoList list)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            _lists.Add(list);

            return Task.FromResult(0);
        }

        public Task DeleteList(Guid listId)
        {
            _lists.RemoveAll(x => x.Id == listId);

            return Task.FromResult(0);
        }

        public Task UpdateListName(Guid listId, string name)
        {
            var list = _lists.SingleOrDefault(x => x.Id == listId);

            if (list is null)
                throw new Exception($"Could not find list {listId}");

            list.Name = name;

            return Task.FromResult(0);
        }

        public Task<List<ToDoList>> GetAllLists()
        {
            return Task.FromResult(_lists);
        }

        public Task<ToDoList> GetList(Guid listId)
        {
            var list = _lists.SingleOrDefault(x => x.Id == listId);

            if (list is null)
                throw new Exception($"Could not find list {listId}");

            return Task.FromResult(list);
        }

        public Task DeleteItem(Guid listId, Guid itemId)
        {
            var list = _lists.SingleOrDefault(x => x.Id == listId);

            if (list is null)
                throw new Exception($"Could not find list {listId}");

            var item = list.Items.SingleOrDefault(x => x.Id == itemId);

            if (list is null)
                throw new Exception($"Could not find item {itemId} in list {listId}");

            list.Items.Remove(item);
            
            return Task.FromResult(0);
        }

        public Task AddItem(Guid listId, ToDoItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var list = _lists.SingleOrDefault(x => x.Id == listId);

            if (list is null)
                throw new Exception($"Could not find list {listId}");

            list.Items.Add(item);

            return Task.FromResult(0);
        }

        public Task UpdateItemName(Guid listId, Guid itemId, string name)
        {
            var list = _lists.SingleOrDefault(x => x.Id == listId);

            if (list is null)
                throw new Exception($"Could not find list {listId}");

            var item = list.Items.SingleOrDefault(x => x.Id == itemId);

            if (list is null)
                throw new Exception($"Could not find item {itemId} in list {listId}");

            item.Name = name;

            return Task.FromResult(0);
        }
    }
}
