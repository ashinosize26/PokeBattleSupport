namespace PokeBattleSupport.Client.Models
{
    public class PokeSpeedRealNumberRowModel
    {
        public long No { get; set; }

        public string Name { get; set; } = string.Empty;

        public double FastestSpeed { get; set; }

        public double FastSpeed { get; set; }

        public double DefaultSpeed { get; set; }

        public double SlowSpeed { get; set; }

        public double SlowestSpeed { get; set; }
    }
}
