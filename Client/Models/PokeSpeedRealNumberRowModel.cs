namespace PokeBattleSupport.Client.Models
{
    public class PokeSpeedRealNumberRowModel
    {
        public long No { get; set; }

        public string Name { get; set; } = string.Empty;

        public int FastestSpeed { get; set; }

        public int FastSpeed { get; set; }

        public int DefaultSpeed { get; set; }

        public int SlowSpeed { get; set; }

        public int SlowestSpeed { get; set; }

        public Common.Common.SpeedRank SpeedRank { get; set; } = Common.Common.SpeedRank.Unknown;
    }
}
