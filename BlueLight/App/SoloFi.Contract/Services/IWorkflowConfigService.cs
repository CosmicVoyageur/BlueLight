using System;
using System.Threading.Tasks;

namespace SoloFi.Contract.Services
{
    public interface IWorkflowConfigService
    {
        Task SetupWorkflows();
        event EventHandler ConfigComplete;
        bool IsConfigComplete { get; }
    }
}