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
    public class PlayersController : ControllerBase
    {     
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Players Get()
        {
            Players players = new Players(){playerLeft = new Player(), playerRight = new Player()};
            players.playerLeft.name = System.IO.File.ReadAllText("players/playerLeft_name.txt"); 
            players.playerLeft.deck = System.IO.File.ReadAllText("players/playerLeft_deck.txt"); 
            players.playerLeft.score = System.IO.File.ReadAllText("players/playerLeft_score.txt"); 
            players.playerRight.name = System.IO.File.ReadAllText("players/playerRight_name.txt"); 
            players.playerRight.deck = System.IO.File.ReadAllText("players/playerRight_deck.txt"); 
            players.playerRight.score = System.IO.File.ReadAllText("players/playerRight_score.txt"); 
            return players;          
        }

        [HttpPost]
        public ActionResult Post(Players players)
        {
            System.IO.File.WriteAllText("players/playerLeft_name.txt", players.playerLeft.name); 
            System.IO.File.WriteAllText("players/playerLeft_deck.txt", players.playerLeft.deck); 
            System.IO.File.WriteAllText("players/playerLeft_score.txt", players.playerLeft.score); 
            System.IO.File.WriteAllText("players/playerRight_name.txt", players.playerRight.name); 
            System.IO.File.WriteAllText("players/playerRight_deck.txt", players.playerRight.deck); 
            System.IO.File.WriteAllText("players/playerRight_score.txt", players.playerRight.score); 

            return Ok();
        }
    }

    public class Player
    {
        public string name {get; set; }
        public string deck {get; set; }
        public string score {get; set; }
    }

    public class Players
    {
        public Player playerLeft {get; set; }
        public Player playerRight {get; set; }
    }
}
