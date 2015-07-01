using System;
using System.Threading.Tasks;

namespace BlueLight.Contract.Services
{
    public interface IWorkflowConfigService
    {
        Task SetupWorkflows();
        event EventHandler ConfigComplete;
        bool IsConfigComplete { get; }
    }
}