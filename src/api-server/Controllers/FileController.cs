using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TwentyFiveHours.API.Controllers
{
    [Route("api/files")]
    public class FileController : Controller
    {
        [HttpGet("{filename}")]
        public IActionResult GetFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
                return NotFound();

            var fileContent = System.IO.File.ReadAllBytes(filename);

            string mime;
            if (filename.EndsWith(".wav"))
                mime = "audio/x-wav";
            else if (filename.EndsWith(".txt"))
                mime = "text/plain";
            else
                return NotFound();

            return File(fileContent, mime);
        }
    }
}
