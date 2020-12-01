using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todo.Services;
using ToDo.Models;

namespace ToDo.Tests
{
    public class LocalToDoServiceTests
    {
        private IToDoService _toDoService;

        private ToDoList _mockList_1;
        private ToDoList _mockList_2;

        private ToDoItem _mockItem_1;
        private ToDoItem _mockItem_2;

        [SetUp]
        public void Setup()
        {
            _toDoService = new LocalToDoService();

            _mockList_1 = new ToDoList
            {
                Id = Guid.NewGuid(),
                Name = "Mock List 1",
            };

            _mockList_2 = new ToDoList
            {
                Id = Guid.NewGuid(),
                Name = "Mock List 1",
            };

            _mockItem_1 = new ToDoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Mock Item 1",
                Completed = false
            };

            _mockItem_2 = new ToDoItem()
            {
                Id = Guid.NewGuid(),
                Name = "Mock Item 2",
                Completed = false
            };
        }

        [Test]
        public async Task Test_CreateList()
        {
            await _toDoService.CreateList(_mockList_1);

            var lists = await _toDoService.GetAllLists();

            Assert.That(lists.Count(x => x.Id == _mockList_1.Id), Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GetList()
        {
            await _toDoService.CreateList(_mockList_1);

            var list = await _toDoService.GetList(_mockList_1.Id);

            Assert.AreEqual(_mockList_1, list);
        }

        [Test]
        public async Task Test_RemoveList()
        {
            await _toDoService.CreateList(_mockList_1);
            await _toDoService.DeleteList(_mockList_1.Id);

            var lists = await _toDoService.GetAllLists();

            Assert.That(lists.Count(x => x.Id == _mockList_1.Id), Is.EqualTo(0));
        }

        [Test]
        public async Task Test_GetAllLists()
        {
            await _toDoService.CreateList(_mockList_1);
            await _toDoService.CreateList(_mockList_2);

            var lists = await _toDoService.GetAllLists();

            Assert.That(lists.Count(), Is.EqualTo(2));
            CollectionAssert.AllItemsAreUnique(lists);
            CollectionAssert.AllItemsAreNotNull(lists);
        }

        [Test]
        public async Task Test_AddItems()
        {
            await _toDoService.CreateList(_mockList_1);
            await _toDoService.AddItem(_mockList_1.Id, _mockItem_1);
            await _toDoService.AddItem(_mockList_1.Id, _mockItem_2);

            var list = await _toDoService.GetList(_mockList_1.Id);

            CollectionAssert.Contains(list.Items, _mockItem_1);
            CollectionAssert.Contains(list.Items, _mockItem_2);
        }

        [Test]
        public async Task Test_RemoveItems()
        {
            await _toDoService.CreateList(_mockList_1);
            await _toDoService.AddItem(_mockList_1.Id, _mockItem_1);
            await _toDoService.AddItem(_mockList_1.Id, _mockItem_2);

            await _toDoService.RemoveItem(_mockList_1.Id, _mockItem_1.Id);

            var list = await _toDoService.GetList(_mockList_1.Id);

            CollectionAssert.DoesNotContain(list.Items, _mockItem_1);
            CollectionAssert.Contains(list.Items, _mockItem_2);
        }
    }
}