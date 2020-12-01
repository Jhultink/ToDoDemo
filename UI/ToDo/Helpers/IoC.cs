using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToDo.PageModels;
using Xamarin.Forms;

namespace ToDo.Helpers
{
    public class IoC
    {
        public static IContainer Container { get; private set; }

        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            string pagesNamespace = "ToDo.Pages";
            string pageModelsNamespace = "ToDo.PageModels";

            List<Type> pageTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.Namespace == pagesNamespace || t.Namespace == pageModelsNamespace)
                .ToList();

            foreach (Type t in pageTypes)
            {
                builder.RegisterType(t);
            }

            builder.RegisterModule<ServiceModule>();

            return Container = builder.Build();
        }

        public static Page ResolvePage<P, M>(Action<M> initialize = null)
             where P : Page
             where M : BasePageModel
        {
            var page = Container.Resolve<P>();
            var pageModel = Container.Resolve<M>();
            pageModel.Navigation = page.Navigation;
            initialize?.Invoke(pageModel);
            page.BindingContext = pageModel;
            page.Appearing += async (sender, e) => await pageModel.OnAppearing();
            return page;
        }
    }
}
