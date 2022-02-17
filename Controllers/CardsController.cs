using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TCGStreamHelper.Models;
using TCGStreamHelper.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TCGStreamHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {       
        private readonly ILogger<CardsController> _logger;
        private readonly LiveDataService _liveDataService;

        public CardsController(ILogger<CardsController> logger, LiveDataService liveDataService)
        {
            _logger = logger;
            _liveDataService = liveDataService;
        }

        [HttpGet]
        public IEnumerable<ImageSetVM> Get()
        {
            List<ImageSetVM> imageSets = new List<ImageSetVM>();

            //base directory
            ImageSetVM imageSet = new ImageSetVM()
            {
                name = "Root",
                images = new List<string>()
            };

            foreach (string file in Directory.GetFiles($"wwwroot{Path.DirectorySeparatorChar}cards"))
            {
               imageSet.images.Add($"cards{Path.DirectorySeparatorChar}{Path.GetFileName(file)}");
            } 

            imageSets.Add(imageSet);               

            //subdirectories
            foreach(string directory in Directory.GetDirectories($"wwwroot{Path.DirectorySeparatorChar}cards"))
            {
                string directoryName = Path.GetFileName(directory);
                imageSet = new ImageSetVM()
                {
                    name = directoryName,
                    images = new List<string>()
                };
                
                foreach (string file in Directory.GetFiles(directory))
                {
                    imageSet.images.Add($"cards{Path.DirectorySeparatorChar}{directoryName}{Path.DirectorySeparatorChar}{Path.GetFileName(file)}");
                } 

                imageSets.Add(imageSet);
            }
            
            return imageSets;          
        }

        [HttpGet]
        [Route("active")]
        public List<ImageVM> GetActive()
        {
            return _liveDataService.GetImages();
        }

        [HttpPost]
        public ActionResult Post(ImageVM image)
        {
            _liveDataService.SetImage(image);
            _logger.LogInformation(image.filename);
            Directory.EnumerateFiles($"activeImage{Path.DirectorySeparatorChar}").ToList().Where(f => f.Contains($"current{image.index}")).ToList().ForEach(f => System.IO.File.Delete(f));
            string sourcePath = $"wwwroot{Path.DirectorySeparatorChar}{image.filename}";
            string destinationPath = $"activeImage{Path.DirectorySeparatorChar}current{image.index}{Path.GetExtension(sourcePath)}";
            //set file active
            System.Threading.Thread.Sleep(200);
            System.IO.File.Copy(sourcePath, destinationPath);
            return Ok();
        }

        [HttpPost]
        [Route("openImageFolder")]
        public ActionResult OpenImageFolder()
        {
            try {
                new Process{
                    StartInfo = new ProcessStartInfo(Path.GetFullPath($"wwwroot{Path.DirectorySeparatorChar}cards"))
                    {
                        UseShellExecute = true
                    }}.Start();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Unable to open local folder \n{e}");
                return Problem();
            }
        }


    }
}
