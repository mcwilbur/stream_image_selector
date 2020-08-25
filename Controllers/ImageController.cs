using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreVueStarter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreVueStarter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        

        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles("wwwroot/images"))
            {
                files.Add(Path.GetFileName(file));
            } 
            return files;          
        }

        [HttpPost]
        public ActionResult Post(Image image)
        {
            _logger.LogInformation(image.filename);
            Directory.EnumerateFiles("activeImage/").ToList().ForEach(f => System.IO.File.Delete(f));
            string sourcePath = $"wwwroot/images/{image.filename}";
            string destinationPath = $"activeImage/current{Path.GetExtension(sourcePath)}";
            //set file active
            System.IO.File.Copy(sourcePath, destinationPath);
            return Ok();
        }
    }

    public class Image
    {
        public string filename {get; set;}
    }
}
