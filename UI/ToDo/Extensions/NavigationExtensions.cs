using System;
using System.Threading.Tasks;
using ToDo.Helpers;
using ToDo.PageModels;
using Xamarin.Forms;

namespace ToDo.Extensions
{
    public static class NavigationExtensions
    {
        public static Task PushAsync<P, M>(this INavigation navigation, Action<M> initialize = null, bool animated = true)
            where P : Page
            where M : BasePageModel
        {
            var page = IoC.ResolvePage<P, M>(initialize);
            return navigation.PushAsync(page, animated);
        }

        public static Task PushModalAsync<P, M>(this INavigation navigation, Action<M> initialize = null, bool animated = true)
            where P : Page
            where M : BasePageModel
        {
            var page = IoC.ResolvePage<P, M>(initialize);
            return navigation.PushModalAsync(page, animated);
        }

        public static Task PushNavigationModalAsync<P, M>(this INavigation navigation, Action<M> initialize = null, bool animated = true)
            where P : Page
            where M : BasePageModel
        {
            var page = IoC.ResolvePage<P, M>(initialize);
            return navigation.PushModalAsync(new NavigationPage(page), animated); ;
        }
    }
}
