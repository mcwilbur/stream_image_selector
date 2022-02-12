namespace TCGStreamHelper.Models 
{
    public class PlayerVM
    {
        public int index {get; set; }
        public string name {get; set; }
        public string deck {get; set; }
        public string score {get; set; }
        public string lifePoints {get; set; }
    }

    public class ActivePlayers
    {
        public PlayerVM playerLeft {get; set; }
        public PlayerVM playerRight {get; set; }
    }
}