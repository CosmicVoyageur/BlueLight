using System.Collections.Generic;
using System.Threading.Tasks;
using BlueLight.Contract.Platform;

namespace BlueLight.Droid.Platform
{
    public class FileExportService : IFileExportService
    {

        private const string DirectoryName = "Rian_Data";
        private static readonly string Root = Android.OS.Environment.ExternalStorageDirectory.Path;
        private static readonly string Path = System.IO.Path.Combine(Root, DirectoryName);

        public FileExportService()
        {
            
        }

        public async Task<bool> FileExists(string fileName)
        {
            var file = System.IO.Path.Combine(Path, fileName);
            return System.IO.File.Exists(file);
        }

        public async Task<bool> SaveToFile(string fileName, string data)
        {
            var file = System.IO.Path.Combine(Path, fileName);
            if(System.IO.File.Exists(file)) System.IO.File.Delete(file);

            System.IO.File.WriteAllText(file, data);

            return System.IO.File.Exists(file);
        }

        public async Task<string> ReadFile(string fileName)
        {
            var file = System.IO.Path.Combine(Path, fileName);
            var result = System.IO.File.ReadAllText(file);
            return result;
        }

        public async Task<IEnumerable<string>> GetAllFileNames()
        {
            return System.IO.Directory.EnumerateFiles(Path);
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                var file = System.IO.Path.Combine(Path, fileName);
                System.IO.File.Delete(file);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetPathForSharing(string fileName)
        {
            return System.IO.Path.Combine(Path, fileName);
        }
    }
}