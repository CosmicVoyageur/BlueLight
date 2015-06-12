using System;
using System.Threading.Tasks;
using SoloFi.Contract.Services;
using XamlingCore.Portable.Workflow.Flow;

namespace SoloFi.Model.Config
{
    public class WorkflowConfigService : IWorkflowConfigService
    {
        private readonly XWorkflowHub _xWorkflowHub;
        private readonly ICommonWorkflowService _commonWorkflowService;

        public WorkflowConfigService(XWorkflowHub xWorkflowHub, 
            ICommonWorkflowService commonWorkflowService)
        {
            _xWorkflowHub = xWorkflowHub;
            _commonWorkflowService = commonWorkflowService;
        }


        public event EventHandler ConfigComplete;

        public bool IsConfigComplete { get; private set; }

        public async Task SetupWorkflows()
        {

            //await _xWorkflowHub.AddFlow(WorkflowNames.PushNewItemToServerFlow, "Push New Item")
            //    .AddStage(WorkflowStageNames.PushNewItem, "Creating new Item on Server", "Server rejected new item",
            //        _serverWorkflowService.PushNewItemToServer, false, true, 1)
            //    .Complete();

            OnConfigComplete();
        }

        private void OnConfigComplete()
        {
            IsConfigComplete = true;
            if (ConfigComplete != null)
            {
                ConfigComplete(this, null);
            }
        }
    }






}
