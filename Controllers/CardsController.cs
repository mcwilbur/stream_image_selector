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
using System.Net.Http;
using System.Text.RegularExpressions;

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
               imageSet.images.Add($"/cards/{Path.GetFileName(file)}");
            } 

            imageSets.Add(imageSet);      

            //downloads directory as second set
            imageSet = new ImageSetVM()
            {
                name = "Downloads",
                images = new List<string>()
            };

            foreach (string file in Directory.GetFiles($"wwwroot{Path.DirectorySeparatorChar}cards{Path.DirectorySeparatorChar}downloads"))
            {
               imageSet.images.Add($"/cards/downloads/{Path.GetFileName(file)}");
            } 

            imageSets.Add(imageSet);           

            //subdirectories
            foreach(string directory in Directory.GetDirectories($"wwwroot{Path.DirectorySeparatorChar}cards"))
            {
                string directoryName = Path.GetFileName(directory);
                if (directoryName.Equals("downloads")) continue; //skip downloads folder
                imageSet = new ImageSetVM()
                {
                    name = directoryName,
                    images = new List<string>()
                };
                
                foreach (string file in Directory.GetFiles(directory))
                {
                    imageSet.images.Add($"/cards/{directoryName}/{Path.GetFileName(file)}");
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
        public async Task<ActionResult> Post(ImageVM image)
        {
            _liveDataService.SetImage(image);
            _logger.LogInformation(image.filename);
            string destinationPath = $"activeImage{Path.DirectorySeparatorChar}current{image.index}";
            string destinationFile;
            //delete previous active file
            Directory.EnumerateFiles($"activeImage{Path.DirectorySeparatorChar}").ToList().Where(f => f.Contains($"current{image.index}")).ToList().ForEach(f => System.IO.File.Delete(f));
            System.Threading.Thread.Sleep(200);
            //check if local file
            if(image.filename.StartsWith('/'))
            {
                string sourcePath = $"wwwroot{image.filename.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar)}";
                destinationFile = $"{destinationPath}{Path.GetExtension(sourcePath)}";
                //set file active                
                System.IO.File.Copy(sourcePath, destinationFile);
            }
            else
            {
                Uri uri = new Uri(image.filename);
                using var httpClient = new HttpClient();

                // Get the file extension
                var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
                var fileExtension = Path.GetExtension(uriWithoutQuery);

                // Create file path and ensure directory exists
                destinationFile = $"{destinationPath}{fileExtension}";

                // Download the image and write to the file
                var imageBytes = await httpClient.GetByteArrayAsync(uri);
                await System.IO.File.WriteAllBytesAsync(destinationFile, imageBytes);
            }            
            return Ok();
        }

        [HttpPost]
        [Route("download")]
        public async Task<ActionResult> Download(ImageVM image)
        {
            try{
                string destinationFolder = $"wwwroot{Path.DirectorySeparatorChar}cards{Path.DirectorySeparatorChar}downloads";
                string destinationFile;
                Uri uri = new Uri(image.filename);
                using var httpClient = new HttpClient();

                // Get the file extension
                var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
                var fileExtension = Path.GetExtension(uriWithoutQuery);
                
                //string imageName = Regex.Match(uriWithoutQuery, @"^.*\/(.*)\..*$").Groups[1].Value ;
                string imageName = uriWithoutQuery.Split('/').LastOrDefault();
                if (fileExtension == null || fileExtension == string.Empty) {
                    fileExtension = ".png";
                    imageName = $"{imageName}{fileExtension}";
                }

                // Create file path and ensure directory exists
                destinationFile = $"{destinationFolder}{Path.DirectorySeparatorChar}{imageName}";

                // Download the image and write to the file
                var imageBytes = await httpClient.GetByteArrayAsync(uri);
                await System.IO.File.WriteAllBytesAsync(destinationFile, imageBytes);
            }
            catch(Exception e)
            {
                return Problem(detail: e.ToString());
            }
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
