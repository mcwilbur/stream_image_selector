using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TCGStreamHelper.Services;

namespace TCGStreamHelper.Controllers
{
    [Route("live")]
    [Controller]
    public class LivedataController : Controller
    {       
        private readonly ILogger<CardsController> _logger;
        private readonly LiveDataService _liveDataService;

        public LivedataController(ILogger<CardsController> logger, LiveDataService liveDataService)
        {
            _logger = logger;
            _liveDataService = liveDataService;
        }

        [HttpGet]
        [Route("image/{index}")]
        public IActionResult GetLiveImage(int index, int width, int height)
        {            
            ViewData.Add("index", index);            
            ViewData.Add("width", width);            
            ViewData.Add("height", height);            
            return View($"Pages{Path.DirectorySeparatorChar}LiveImage.cshtml");
        }
    }
}
