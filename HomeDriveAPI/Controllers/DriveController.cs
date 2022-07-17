using HomeDriveAPI.FileManagment;
using Microsoft.AspNetCore.Mvc;

namespace HomeDriveAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriveController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public ActionResult SayHello()
        {
            return Ok("hello from controller");
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult GetAllFiles()
        {
            return Ok(Filemanager.ListFiles());
        }

        [HttpPost]
        [Route("[action]/{pathOfDirectory}")]
        [RequestSizeLimit(3000000000)]
        public ActionResult UploadFiles([FromForm] List<IFormFile> files, [FromRoute] string pathOfDirectory)
        {
            Filemanager.SaveFilesAsync(files, pathOfDirectory);
            return Ok();
        }

        [HttpDelete]
        [Route("[action]")]
        public ActionResult DeleteFiles(List<string> pathsOfFilesToDelete)
        {
            Filemanager.DeleteFilesAsync(pathsOfFilesToDelete);
            return Ok();
        }

        [HttpGet]
        [Route("[action]/{pathOfFileToDownload}")]
        public ActionResult DownloadFile([FromRoute] string pathOfFileToDownload)
        {
            var filedata = Filemanager.ReturnFile(pathOfFileToDownload);
            var contentType = "multipart/form-data";
            return File(filedata, contentType);
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult CreateDirectory(string pathOfNewDirectory)
        {
            var succes = Filemanager.CreateDirectory(pathOfNewDirectory);
            if (succes)
                return Ok();
            else
                return BadRequest("This path is either not valid or the directory already exists");
        }

        [HttpDelete]
        [Route("[action]")]
        public ActionResult DeleteDirectory(string nameOfDirectory)
        {
            Filemanager.DeleteDirectory(nameOfDirectory);
            return Ok();
        }
    }
}
