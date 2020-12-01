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
    public partial class ConfirmationPopupPage : PopupPage
    {
        private readonly TaskCompletionSource<bool> _tcs;

        public Task<bool> Task => _tcs.Task;

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

        public ConfirmationPopupPage()
        {
            InitializeComponent();
            _tcs = new TaskCompletionSource<bool>();
        }

        private async void Confirm_Clicked(object sender, EventArgs e)
        {
            _tcs.TrySetResult(true);

            var nav = IoC.Container.Resolve<IPopupNavigation>();
            await nav.PopAsync();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            _tcs.TrySetResult(false);

            var nav = IoC.Container.Resolve<IPopupNavigation>();
            await nav.PopAsync();
        }
    }
}