using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TCGStreamHelper.Services;
using TCGStreamHelper.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TCGStreamHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SSEController : ControllerBase
    {     
        private readonly ILogger<SSEController> _logger;
        private readonly LiveDataService _livedata;

        public SSEController(ILogger<SSEController> logger, LiveDataService livedata)
        {
            _logger = logger;
            _livedata = livedata;
        }

        [HttpGet]
        [Route("card/{index}")]
        public async Task Get(int index)
        {

            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");

            string previousName = "";

            for(var i = 0; true ; i = (i+1)%100)
            {
                
                string imageName = "empty.png";
                ImageVM image = _livedata.GetImages().Find(i => i.index == index);
                if (image != null) imageName = image.filename;
                if(!imageName.Equals(previousName))
                {
                    await response.WriteAsync($"id: ${i}\n");
                    await response.WriteAsync("event: message\n");
                    await response.WriteAsync($"data: {imageName}\n\n");
                    previousName = imageName;
                }

                await response.Body.FlushAsync();
                await Task.Delay(200);
            }            
        }

        [HttpPost]
        public string Post([FromBody] string param)
        {
            return null;
        }
    }
}