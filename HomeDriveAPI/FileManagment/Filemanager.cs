
namespace HomeDriveAPI.FileManagment
{
    public class Filemanager
    {
        private static readonly object _lockForSaving = new object();
        private static readonly object _lockForDeleting = new object();
        private static readonly string _drivepath = "./drive";
        public static void CheckForDriveDirectory()
        {
            if (!Directory.Exists(_drivepath))
            {
                Directory.CreateDirectory(_drivepath);
            }
        }

        public static bool SaveFilesAsync(List<IFormFile> files, string pathOfDirectory)
        {
            CheckForDriveDirectory();
            try
            {
                foreach (var file in files)
                {
                    lock (_lockForSaving) {
                        var fStream = File.Create($"{_drivepath}/{pathOfDirectory}/{file.FileName}");
                        file.CopyTo(fStream);
                        fStream.Flush();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteFilesAsync(List<string> files)
        {
            CheckForDriveDirectory();
            try
            {
                foreach (var file in files)
                {
                    lock (_lockForDeleting)
                    {
                        if (File.Exists($"{_drivepath}/{file}"))
                            File.Delete($"{_drivepath}/{file}");
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] ReturnFile(string file)
        {
            CheckForDriveDirectory();
            byte[] bytes = {};
            try
            {
                lock (_lockForDeleting)lock(_lockForSaving)
                {
                    if (File.Exists($"{_drivepath}/{file}"))
                    {
                        using (FileStream fs = new FileStream($"{_drivepath}/{file}", FileMode.Open, FileAccess.Read))
                        {
                            bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, (int)fs.Length);
                        }
                    }
                    return bytes;
                }
            }
            catch (Exception)
            {
                return bytes;
            }
        }

        public static List<string> ListFiles()
        {
            CheckForDriveDirectory();
            var dirInfo = new DirectoryInfo(_drivepath);
            return dirInfo.GetFiles().Select(f => f.Name).ToList();
        }

        public static bool CreateDirectory(string pathOfNewDirectory)
        {
            CheckForDriveDirectory();
            lock (_lockForSaving)
            {
                if (Directory.Exists($"{_drivepath}/{pathOfNewDirectory}"))
                    return false;
                else
                    Directory.CreateDirectory($"{_drivepath}/{pathOfNewDirectory}");
            }
            return true;
        }

        public static void DeleteDirectory(string dirName)
        {
            CheckForDriveDirectory();
            lock (_lockForDeleting)
            {
                try
                {
                    Directory.Delete($"{_drivepath}/{dirName}", true);
                }
                catch (Exception) { }
            }
        }
    }
}
