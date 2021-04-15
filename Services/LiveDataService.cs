using System.Collections.Generic;
using TCGStreamHelper.Models;
using System;

namespace TCGStreamHelper.Services 
{
    public class LiveDataService
    {
        private Players _players;
        private List<ImageVM> _images;
        private DateTime _lastPlayerChange;
        private DateTime _lastImageChange;

        public LiveDataService()
        {
            _images = new List<ImageVM>();
            _players = new Players(){playerLeft = new PlayerVM(), playerRight = new PlayerVM()};
            _players.playerLeft.name = System.IO.File.ReadAllText("players/playerLeft_name.txt"); 
            _players.playerLeft.deck = System.IO.File.ReadAllText("players/playerLeft_deck.txt"); 
            _players.playerLeft.score = System.IO.File.ReadAllText("players/playerLeft_score.txt"); 
            _players.playerLeft.lifePoints = System.IO.File.ReadAllText("players/playerLeft_lpoints.txt"); 
            _players.playerRight.name = System.IO.File.ReadAllText("players/playerRight_name.txt"); 
            _players.playerRight.deck = System.IO.File.ReadAllText("players/playerRight_deck.txt"); 
            _players.playerRight.score = System.IO.File.ReadAllText("players/playerRight_score.txt"); 
            _players.playerRight.lifePoints = System.IO.File.ReadAllText("players/playerRight_lpoints.txt"); 
        }

        public void SetPlayers(Players players)
        {
            _players = players;
            _lastPlayerChange = DateTime.Now;
            System.IO.File.WriteAllText("players/playerLeft_name.txt", _players.playerLeft.name); 
            System.IO.File.WriteAllText("players/playerLeft_deck.txt", _players.playerLeft.deck); 
            System.IO.File.WriteAllText("players/playerLeft_score.txt", _players.playerLeft.score); 
            System.IO.File.WriteAllText("players/playerLeft_lpoints.txt", _players.playerLeft.lifePoints); 
            System.IO.File.WriteAllText("players/playerRight_name.txt", _players.playerRight.name); 
            System.IO.File.WriteAllText("players/playerRight_deck.txt", _players.playerRight.deck); 
            System.IO.File.WriteAllText("players/playerRight_score.txt", _players.playerRight.score); 
            System.IO.File.WriteAllText("players/playerRight_lpoints.txt", _players.playerRight.lifePoints); 
        }

        public Players GetPlayers()
        {
            return _players;
        }

        public DateTime GetLastPlayerChange()
        {
            return _lastPlayerChange;
        }

        public void SetImage(ImageVM image)
        {
            _images.Remove(_images.Find(i => i.index == image.index));
            _images.Add(image);             
        }

        public List<ImageVM> GetImages()
        {
            return _images;
        }

        public DateTime GetLastImageChange()
        {
            return _lastImageChange;
        }

    }
}