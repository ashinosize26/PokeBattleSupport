using static PokeBattleSupport.Client.Common.Common;
namespace PokeBattleSupport.Client.Models
{
    public class PokeInfoModel
    {
        public string Name { get; set; } = string.Empty;

        public long No { get; set; }

        public string FirstType { get; set; } = string.Empty;

        public string SecondType { get; set; } = string.Empty;

        public long H { get; set; }

        public long A { get; set; }

        public long B { get; set; }

        public long C { get; set; }

        public long D { get; set; }

        public long S { get; set; }

        public long Total { get; set; }

        public SpeedRank SpeedRank { get; set; } = SpeedRank.Unknown;

        public BattleItem BattleItem { get; set; } = BattleItem.None;
    }
}
