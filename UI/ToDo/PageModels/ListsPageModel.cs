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
        public Command<ToDoList> DeleteCommand { get; set; }

        private readonly IToDoService _toDoService;
        private readonly IPopupNavigation _popupNavigation;

        public ListsPageModel(IToDoService toDoService, IPopupNavigation popupNavigation)
        {
            _toDoService = toDoService;
            _popupNavigation = popupNavigation;

            CreateCommand = new Command(Create);
            DeleteCommand = new Command<ToDoList>(Delete);

        }

        public override async Task OnAppearing()
        {
            var allLists = await _toDoService.GetAllLists();
            Lists = new ObservableCollection<ToDoList>(allLists);
        }

        private async void Create()
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

                await Navigation.PushAsync<ItemsPage, ItemsPageModel>(x => x.Init(newList));
            }
            catch(OperationCanceledException)
            {
                Debug.WriteLine("User cancelled setting name");
            }
            catch (Exception ex)
            {
                Debug.Fail("Error creating list", ex.Message);
            }
        }

        private async void Delete(ToDoList list)
        {
            try
            {
                var confirmPopup = new ConfirmationPopupPage()
                {
                    Header = "Delete",
                    Body = "Are you sure you want to delete this list?",
                    ConfirmButtonText = "Yes"
                };

                await _popupNavigation.PushAsync(confirmPopup);

                if (!await confirmPopup.Task)
                    return;

                await _toDoService.DeleteList(list.Id);

                Lists.Remove(list);

                OnPropertyChanged(nameof(Lists)); // To show/hide list
            }
            catch (Exception ex)
            {
                Debug.Fail("Error deleting list", ex.Message);
            }
        }

    }
}
