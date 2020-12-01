using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todo.Services;
using ToDo.Models;
using ToDo.Pages;
using Xamarin.Forms;
using ToDo.Extensions;

namespace ToDo.PageModels
{
    public class ListsPageModel : BasePageModel
    {
        private ObservableCollection<ToDoList> _lists;
        public ObservableCollection<ToDoList> Lists
        {
            get { return _lists; }
            set { _lists = value; OnPropertyChanged(); }
        }

        public ICommand CreateCommand { get; set; }

        private readonly IToDoService _toDoService;

        public ListsPageModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            CreateCommand = new Command(Create);
        }

        public override async Task OnAppearing()
        {
            var allLists = await _toDoService.GetAllLists();
            Lists = new ObservableCollection<ToDoList>(allLists);
        }

        public async void Create()
        {
            await Navigation.PushAsync<ItemsPage, ItemsPageModel>();
        }
    }
}
