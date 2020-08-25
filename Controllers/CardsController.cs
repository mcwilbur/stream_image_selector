using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TCGStreamHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {       
        private readonly ILogger<CardsController> _logger;

        public CardsController(ILogger<CardsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles("wwwroot/cards"))
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
            string sourcePath = $"wwwroot/cards/{image.filename}";
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
