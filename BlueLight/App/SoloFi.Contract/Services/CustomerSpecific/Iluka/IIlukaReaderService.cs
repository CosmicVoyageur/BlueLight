using System;
using System.Threading.Tasks;
using SoloFi.Entity.CustomerSpecific.Iluka;
using Tsl.AsciiProtocol.Pcl;
using BarcodeEventArgs = SoloFi.Contract.EventArgs.BarcodeEventArgs;

namespace SoloFi.Contract.Services.CustomerSpecific.Iluka
{
    public interface IIlukaReaderService
    {
        void Initialise();
        bool WriteToTransponderUserMemory(IlukaUhfTag select, IlukaUhfTag update);
        event EventHandler<TransponderDataEventArgs> TransponderReadEvent;
        event EventHandler<BarcodeEventArgs> BarcodeReadEvent;
        Task<bool> VerifyReaderLicence();
    }
}