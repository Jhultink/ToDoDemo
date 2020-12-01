using Autofac;
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
        }
    }
}
