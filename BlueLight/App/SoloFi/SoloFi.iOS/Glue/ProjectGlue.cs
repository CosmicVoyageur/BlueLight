using System.Reflection;
using Autofac;
using iQMobile2.Contract.Platform;
using iQMobile2.Contract.Services;
using iQMobile2.iOS.Platform;
using iQMobile2.Model.Config;
using iQMobile2.Model.Services;
using XamlingCore.iOS.Unified.Glue;
using XamlingCore.Platform.Shared.Glue;
using XamlingCore.Portable.Contract.Downloaders;

namespace iQMobile2.iOS.Glue
{
    class ProjectGlue : iOSGlue
    {
        public override void Init()
        {
            base.Init();
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(iQMobile2.PortableApp)); // registering views and viewmodels
            Builder.RegisterType<TransferConfigService>().As<IHttpTransferConfigService>();
            Builder.RegisterType<RandomThing>().As<IRandomInterface>();
            var b = Builder.RegisterAssemblyTypes(typeof(PersonService).GetTypeInfo().Assembly)
                .Where((t => t.Name.EndsWith("Repo") || t.Name.EndsWith("Service")));
            b.SingleInstance();
            b.AsImplementedInterfaces();

            Container = Builder.Build();
            _init();
        }

        async void _init()
        {
            var workflow = Container.Resolve<IWorkflowConfigService>();
            await workflow.SetAssetFlow();
        }
    }
}