using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoloFi.Contract.Platform;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Search;
using Xamarin.Forms;

namespace SoloFi.Universal.Platform
{
    public class FileExportService : IFileExportService
    {
        private const string Folder = "Ramp_Data";
        private readonly StorageFolder _local = ApplicationData.Current.LocalFolder;

        public async Task<bool> FileExists(string fileName)
        {
            var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
            var file = await dataFolder.GetItemAsync(fileName);
            return file != null;
        }

        public async Task<bool> SaveToFile(string fileName, string data)
        {
            // Get the local folder.
            var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
            
            // Create a new file.
            var file = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            
            // Write the data from the textbox.
            try
            {
                using (var s = await file.OpenStreamForWriteAsync())
                {
                    using (var writer = new StreamWriter(s))
                    {
                        writer.Write(data);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ReadFile(string fileName)
        {
            var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);

            var file = await dataFolder.GetFileAsync(fileName);
            if (file == null) return null;
            using (var streamReader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public async Task<IEnumerable<string>> GetAllFileNames()
        {
            var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
            var files = await dataFolder.GetFilesAsync();

            foreach (var file in files)
            {
                Debug.WriteLine(file.Path);
            }

            return files.Select(file => file.Name).ToList();
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
                var file = await dataFolder.GetFileAsync(fileName);
                await file.DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetPathForSharing(string fileName)
        {
            var dataFolder = await _local.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
            var file = await dataFolder.GetFileAsync(fileName);
            return file.Path;
        }
    }
}
