using static PokeBattleSupport.Client.Common.Common;
namespace PokeBattleSupport.Client.Models
{
    public class PokeSpeedRealNumberRowModel
    {
        public long No { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool ChoiceScarf { get; set; } = false;

        public int FastestSpeed { get; set; }

        public int FastSpeed { get; set; }

        public int DefaultSpeed { get; set; }

        public int SlowSpeed { get; set; }

        public int SlowestSpeed { get; set; }

        public SpeedRank SpeedRank { get; set; } = SpeedRank.Unknown;
    }
}
