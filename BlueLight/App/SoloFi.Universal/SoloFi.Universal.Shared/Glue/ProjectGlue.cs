using System.Reflection;
using Autofac;
using SoloFi.Contract.Platform;
using SoloFi.Contract.Services;
using SoloFi.Model.Config;
using SoloFi.Model.Services;
using SoloFi.Universal.Platform;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Windows8.Glue;
using XamlingCore.Windows8.Shared.Glue;

namespace SoloFi.Universal.Glue
{
    public class ProjectGlue : Windows8Glue
    {
        public override void Init()
        {
            base.Init();
            XCoreAutoRegistration.RegisterAssembly(Builder,typeof(AppEntryXApplication)); // registering views and viewmodels
            Builder.RegisterType<TransferConfigService>().As<IHttpTransferConfigService>();

            #region Platform Specific Services

            Builder.RegisterType<BluetoothRfidReaderDiscoveryService>().As<IBluetoothRfidReaderDiscoveryService>();
            Builder.RegisterType<NotifyService>().As<INotifyService>();
            Builder.RegisterType<FileExportService>().As<IFileExportService>();
            Builder.RegisterType<FileShareService>().As<IFileShareService>();

            #endregion


            Builder.RegisterType<BaseRfidReaderService>().As<IRfidReaderService>().SingleInstance();
            Builder.RegisterType<RandomThing>().As<IRandomInterface>();
            var b = Builder.RegisterAssemblyTypes(typeof (LocalEntityService<>).GetTypeInfo().Assembly)
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
