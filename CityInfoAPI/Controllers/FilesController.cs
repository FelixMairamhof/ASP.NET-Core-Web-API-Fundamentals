using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfoAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "Database ER diagram (crow's foot) - Page 1.svg";

            if(!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, "text/plain", Path.GetFileName(pathToFile));
        }
    }
}
