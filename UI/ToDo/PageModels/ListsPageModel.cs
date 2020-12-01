using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Services;
using ToDo.Models;
using ToDo.Pages;
using Xamarin.Forms;
using ToDo.Extensions;
using Rg.Plugins.Popup.Contracts;
using ToDo.Pages.Popups;
using System.Diagnostics;

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
        private readonly IPopupNavigation _popupNavigation;

        public ListsPageModel(IToDoService toDoService, IPopupNavigation popupNavigation)
        {
            _toDoService = toDoService;
            _popupNavigation = popupNavigation;

            CreateCommand = new Command(Create);
        }

        public override async Task OnAppearing()
        {
            var allLists = await _toDoService.GetAllLists();
            Lists = new ObservableCollection<ToDoList>(allLists);
        }

        public async void Create()
        {
            try
            {
                var setNamePopupPage = new SetNamePopupPage()
                {
                    Header = "List Name",
                    Body = "Set a list name"
                };

                await _popupNavigation.PushAsync(setNamePopupPage);

                string name = await setNamePopupPage.Task;

                ToDoList newList = new ToDoList
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };

                await _toDoService.CreateList(newList);

                Lists.Add(newList);

                OnPropertyChanged(nameof(Lists)); // To show/hide list
            }
            catch(OperationCanceledException)
            {
                Debug.WriteLine("User cancelled setting name");
            }
        }
    }
}
