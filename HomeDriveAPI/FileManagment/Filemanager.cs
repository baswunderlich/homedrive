
namespace HomeDriveAPI.FileManagment
{
    public class Filemanager
    {
        private static readonly object _lockForSaving = new object();
        private static readonly object _lockForDeleting = new object();
        private static readonly string _drivepath = "./drive";
        public static bool SaveFilesAsync(List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    lock (_lockForSaving) {
                        var fStream = File.Create($"{_drivepath}/{file.FileName}");
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
            byte[] bytes = {};
            try
            {
                lock (_lockForDeleting)lock(_lockForSaving)
                {
                    if (File.Exists($"{_drivepath}/{file}"))
                    {
                        byte[] buffer = null;
                        using (FileStream fs = new FileStream($"{_drivepath}/{file}", FileMode.Open, FileAccess.Read))
                        {
                            buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, (int)fs.Length);
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
            var dirInfo = new DirectoryInfo(_drivepath);
            return dirInfo.GetFiles().Select(f => f.Name).ToList();
        }
    }
}
