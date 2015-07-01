using System.Reflection;
using Autofac;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Droid.Platform;
using SoloFi.Model.Config;
using SoloFi.Model.Services;
using XamlingCore.Droid.Glue;
using XamlingCore.Platform.Shared.Glue;
using XamlingCore.Portable.Contract.Downloaders;

namespace SoloFi.Droid.Glue
{
    class ProjectGlue : DroidGlue
    {
        public override void Init()
        {
            base.Init();
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(AppEntryXApplication)); // registering views and viewmodels


            Builder.RegisterType<TransferConfigService>().As<IHttpTransferConfigService>();


            #region Platform Specific Services

            Builder.RegisterType<BluetoothRfidReaderDiscoveryService>().As<IBluetoothRfidReaderDiscoveryService>();
            Builder.RegisterType<NotifyService>().As<INotifyService>();
            Builder.RegisterType<FileExportService>().As<IFileExportService>();
            Builder.RegisterType<FileShareService>().As<IFileShareService>();

            #endregion


            var b = Builder.RegisterAssemblyTypes(typeof(LocalEntityService<>).GetTypeInfo().Assembly)
                .Where((t => t.Name.EndsWith("Repo") || t.Name.EndsWith("Service")));
            b.SingleInstance();
            b.AsImplementedInterfaces();

            Container = Builder.Build();

            _init();
        }

        async void _init()
        {
            var workflow = Container.Resolve<IWorkflowConfigService>();
            await workflow.SetupWorkflows();
        }
    }
}