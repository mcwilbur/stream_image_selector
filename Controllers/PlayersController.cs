using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TCGStreamHelper.Models;
using TCGStreamHelper.Services;

namespace TCGStreamHelper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {     
        private readonly ILogger<PlayersController> _logger;
        private readonly LiveDataService _liveDataService;

        public PlayersController(ILogger<PlayersController> logger, LiveDataService liveDataService)
        {
            _logger = logger;
            _liveDataService = liveDataService;
        }

        [HttpGet]
        public ActivePlayers Get()
        {
            ActivePlayers players = _liveDataService.GetPlayers();
            return players;          
        }

        [HttpPost]
        public ActionResult Post(ActivePlayers players)
        {
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerLeft_name.txt", players.playerLeft.name); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerLeft_deck.txt", players.playerLeft.deck); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerLeft_score.txt", players.playerLeft.score); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerLeft_lpoints.txt", players.playerLeft.lifePoints); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerRight_name.txt", players.playerRight.name); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerRight_deck.txt", players.playerRight.deck); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerRight_score.txt", players.playerRight.score); 
            System.IO.File.WriteAllText($"players{Path.DirectorySeparatorChar}playerRight_lpoints.txt", players.playerRight.lifePoints); 

            _liveDataService.SetPlayers(players);

            return Ok();
        }
    }
}
