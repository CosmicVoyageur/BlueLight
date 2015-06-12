using System;
using System.Collections.Generic;
using Windows.Storage;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using SoloFi.Contract.Platform;

namespace SoloFi.Universal.Platform
{
    public class FileShareService : IFileShareService
    {
        public FileShareService()
        {
            
        }

        public async Task ShareFilesPreferEmail(List<string> paths, string subject)
        {
            _tempPaths = paths;
            _tempSubject = subject;
            var man = DataTransferManager.GetForCurrentView();
            man.DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
            
        }

        private List<string> _tempPaths;
        private string _tempSubject;
        public async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = _tempSubject;
            args.Request.Data.Properties.Description = "Data files have been shared";

            //Set the text content for the share target to use
            args.Request.Data.SetText(_tempPaths.Count + " files have been shared.");

            args.Request.Data.SetStorageItems(await GetFiles(_tempPaths));


        }

        private async Task<List<StorageFile>> GetFiles(List<string> paths)
        {
            var result = new List<StorageFile>();
            foreach (var path in paths)
            {
                var file = await StorageFile.GetFileFromPathAsync(path);
                result.Add(file);
            }
            return result;
        } 
    }
}
