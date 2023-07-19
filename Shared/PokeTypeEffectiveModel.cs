using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattleSupport.Shared
{
    public class PokeTypeEffectiveModel
    {
        public string FirstType { get; set; } = string.Empty;

        public string SecondType { get; set; } = string.Empty;

        public double NormalEffectiveValue { get; set; }

        public double FireEffectiveValue { get; set; }

        public double WaterEffectiveValue { get; set; }

        public double ElectricEffectiveValue { get; set; }

        public double GrassEffectiveValue { get; set; }

        public double IceEffectiveValue { get; set; }

        public double FightingEffectiveValue { get; set; }

        public double PoisonEffectiveValue { get; set; }

        public double GroundEffectiveValue { get; set; }

        public double FlyingEffectiveValue { get; set; }

        public double PsychicEffectiveValue { get; set; }

        public double BugEffectiveValue { get; set; }

        public double RockEffectiveValue { get; set; }

        public double GhostEffectiveValue { get; set; }

        public double DragonEffectiveValue { get; set; }

        public double DarkEffectiveValue { get; set; }

        public double SteelEffectiveValue { get; set; }

        public double FairyEffectiveValue { get; set; }

    }
}
