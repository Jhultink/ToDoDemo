using Autofac;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Todo.Services;

namespace ToDo.Helpers
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LocalToDoService>()
                .As<IToDoService>()
                .SingleInstance();

            builder.RegisterInstance(PopupNavigation.Instance)
                .As<IPopupNavigation>()
                .SingleInstance();
        }
    }
}
