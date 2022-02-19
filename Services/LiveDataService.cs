using System.Collections.Generic;
using TCGStreamHelper.Models;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace TCGStreamHelper.Services 
{
    public class LiveDataService
    {
        private ActivePlayers _players;
        private List<ImageVM> _images;
        private DateTime _lastPlayerChange;
        private DateTime _lastImageChange;

        public LiveDataService()
        {
            _images = new List<ImageVM>();
            _players = new ActivePlayers(){playerLeft = new PlayerVM(), playerRight = new PlayerVM()};
            _players.playerLeft.name = "";
            _players.playerLeft.deck = "";            
            _players.playerLeft.score = "";
            _players.playerLeft.lifePoints = "";
            _players.playerRight.name = "";
            _players.playerRight.deck = "";
            _players.playerRight.score = "";
            _players.playerRight.lifePoints = "";
        }

        public void InitService()
        {
            CreateLocalFiles();
            _players.playerLeft.name = System.IO.File.ReadAllText("players/playerLeft_name.txt"); 
            _players.playerLeft.deck = System.IO.File.ReadAllText("players/playerLeft_deck.txt"); 
            _players.playerLeft.score = System.IO.File.ReadAllText("players/playerLeft_score.txt"); 
            _players.playerLeft.lifePoints = System.IO.File.ReadAllText("players/playerLeft_lpoints.txt"); 
            _players.playerRight.name = System.IO.File.ReadAllText("players/playerRight_name.txt"); 
            _players.playerRight.deck = System.IO.File.ReadAllText("players/playerRight_deck.txt"); 
            _players.playerRight.score = System.IO.File.ReadAllText("players/playerRight_score.txt"); 
            _players.playerRight.lifePoints = System.IO.File.ReadAllText("players/playerRight_lpoints.txt"); 
        }

        public void SetPlayers(ActivePlayers players)
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

        public ActivePlayers GetPlayers()
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

        private void CreateLocalFiles()
        {
            if (!Directory.Exists("activeImage")) Directory.CreateDirectory("activeImage");
            if (!Directory.Exists("wwwroot")) Directory.CreateDirectory("wwwroot");
            if (!Directory.Exists("wwwroot/cards")) Directory.CreateDirectory("wwwroot/cards");
            if (!Directory.Exists("wwwroot/cards/downloads")) Directory.CreateDirectory("wwwroot/cards/downloads");
            if (!Directory.Exists("players")) Directory.CreateDirectory("players");

            if(!File.Exists("players/playerLeft_name.txt")) File.Create("players/playerLeft_name.txt").Close(); 
            if(!File.Exists("players/playerLeft_deck.txt")) File.Create("players/playerLeft_deck.txt").Close();
            if(!File.Exists("players/playerLeft_score.txt")) File.Create("players/playerLeft_score.txt").Close();
            if(!File.Exists("players/playerLeft_lpoints.txt")) File.Create("players/playerLeft_lpoints.txt").Close();
            if(!File.Exists("players/playerRight_name.txt")) File.Create("players/playerRight_name.txt").Close();
            if(!File.Exists("players/playerRight_deck.txt")) File.Create("players/playerRight_deck.txt").Close();
            if(!File.Exists("players/playerRight_score.txt")) File.Create("players/playerRight_score.txt").Close();
            if(!File.Exists("players/playerRight_lpoints.txt")) File.Create("players/playerRight_lpoints.txt").Close();
        }

    }
}