using Rg.Plugins.Popup.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Pages;
using ToDo.Pages.Popups;
using ToDo.Services;
using Xamarin.Forms;

namespace ToDo.PageModels
{
    public class ItemsPageModel : BasePageModel
    {
        private ToDoList _list;

        public string Title => _list.Name;

        private ObservableCollection<ToDoItem> _items;
        public ObservableCollection<ToDoItem> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged(); }
        }


        public ICommand CreateCommand { get; set; }
        public ICommand EditListCommand { get; set; }
        public ICommand DeleteListCommand { get; set; }
        public Command<ToDoItem> EditItemCommand { get; set; }
        public Command<ToDoItem> DeleteItemCommand { get; set; }


        private readonly IToDoService _toDoService;
        private readonly IPopupNavigation _popupNavigation;

        public ItemsPageModel(IToDoService toDoService, IPopupNavigation popupNavigation)
        {
            _toDoService = toDoService;
            _popupNavigation = popupNavigation;

            CreateCommand = new Command(Create);
            EditListCommand = new Command(EditList);
            DeleteListCommand = new Command(DeleteList);
            EditItemCommand = new Command<ToDoItem>(EditItem);
            DeleteItemCommand = new Command<ToDoItem>(DeleteItem);
        }

        public void Init(ToDoList toDoList)
        {
            _list = toDoList;

            Items = new ObservableCollection<ToDoItem>(toDoList.Items);
        }

        private async void Create()
        {
            try
            {
                var setNamePopupPage = new SetNamePopupPage()
                {
                    Header = "Item Name",
                    Body = "Set item name"
                };

                await _popupNavigation.PushAsync(setNamePopupPage);

                string name = await setNamePopupPage.Task;

                ToDoItem newItem = new ToDoItem
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };

                await _toDoService.AddItem(_list.Id, newItem);

                Items.Add(newItem);

                OnPropertyChanged(nameof(Items)); // To show/hide list
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("User cancelled setting name");
            }
            catch(Exception ex)
            {
                Debug.Fail("Error creating item", ex.Message);
            }
        }

        private async void EditList()
        {
            try
            {
                var setNamePopupPage = new SetNamePopupPage()
                {
                    Header = "List Name",
                    Body = "Set a list name",
                    Name = _list.Name
                };

                await _popupNavigation.PushAsync(setNamePopupPage);

                string name = await setNamePopupPage.Task;

                await _toDoService.UpdateListName(_list.Id, name);

                _list.Name = name;
                OnPropertyChanged(nameof(Title));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("User cancelled setting name");
            }
            catch (Exception ex)
            {
                Debug.Fail("Error editing list", ex.Message);
            }
        }

        private async void DeleteList()
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

                await _toDoService.DeleteList(_list.Id);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.Fail("Error deleting list", ex.Message);
            }
        }

        private async void EditItem(ToDoItem item)
        {
            try
            {
                var setNamePopupPage = new SetNamePopupPage()
                {
                    Header = "Item Name",
                    Body = "Set an item name",
                    Name = item.Name
                };

                await _popupNavigation.PushAsync(setNamePopupPage);

                string name = await setNamePopupPage.Task;

                await _toDoService.UpdateItemName(_list.Id, item.Id, name);

                // Remove
                int index = Items.IndexOf(item);
                Items.Remove(item);

                // Update and re-add
                item.Name = name;
                Items.Insert(index, item);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("User cancelled setting name");
            }
            catch (Exception ex)
            {
                Debug.Fail("Error editing list", ex.Message);
            }
        }

        private async void DeleteItem(ToDoItem item)
        {
            var confirmPopup = new ConfirmationPopupPage()
            {
                Header = "Delete",
                Body = "Are you sure you want to delete this item?",
                ConfirmButtonText = "Yes"
            };

            await _popupNavigation.PushAsync(confirmPopup);

            if (!await confirmPopup.Task)
                return;

            try
            {
                await _toDoService.DeleteItem(_list.Id, item.Id);

                Items.Remove(item);

                OnPropertyChanged(nameof(Items)); // To show/hide list
            }
            catch (Exception ex)
            {
                Debug.Fail("Error deleting item", ex.Message);
            }
        }
    }
}
