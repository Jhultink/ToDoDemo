using Autofac;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetNamePopupPage : PopupPage
    {
        private readonly TaskCompletionSource<string> _tcs;
        public Task<string> Task => _tcs.Task;

        private string _header;
        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged(); }
        }

        private string _body;
        public string Body
        {
            get { return _body; }
            set { _body = value; OnPropertyChanged(); }
        }

        private string _confirmButtonText = "Okay";
        public string ConfirmButtonText
        {
            get { return _confirmButtonText; }
            set { _confirmButtonText = value; OnPropertyChanged(); }
        }

        private string _cancelButtonText = "Cancel";
        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set { _cancelButtonText = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public SetNamePopupPage()
        {
            InitializeComponent();
            _tcs = new TaskCompletionSource<string>();
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            _tcs.TrySetResult(Name);

            var nav = IoC.Container.Resolve<IPopupNavigation>();
            await nav.PopAsync();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            _tcs.TrySetCanceled();

            var nav = IoC.Container.Resolve<IPopupNavigation>();
            await nav.PopAsync();
        }
    }
}