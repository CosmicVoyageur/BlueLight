using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using BlueCastello.Android.Platform;
using BlueCastello.Android.Platform.Tcp;
using BlueCastello.Contract.Platform;
using BlueCastello.Contract.Platform.Tcp;
using BlueCastello.Model;
using XamlingCore.Droid.Glue;
using XamlingCore.Platform.Shared.Glue;

namespace BlueCastello.Android.Glue
{
    class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(App)); // registering views and viewmodels

            Builder.RegisterType<NotifyService>().As<INotifyService>();
            Builder.RegisterType<TcpSocketService>().As<ITcpSocketService>().SingleInstance();

            var b = Builder.RegisterAssemblyTypes(typeof(TempClass).GetTypeInfo().Assembly)
                .Where((t => t.Name.EndsWith("Repo") || t.Name.EndsWith("Service")));
            b.SingleInstance();
            b.AsImplementedInterfaces();

            Container = Builder.Build();
        }
    }
}