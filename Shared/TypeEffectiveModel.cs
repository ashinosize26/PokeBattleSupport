using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattleSupport.Shared
{
    public class TypeEffectiveModel
    {
        public string AttackType { get; set; } = string.Empty;

        public double NormalEffectiveValue { get; set; } = 1;

        public double FireEffectiveValue { get; set; } = 1;

        public double WaterEffectiveValue { get; set; } = 1;

        public double ElectricEffectiveValue { get; set; } = 1;

        public double GrassEffectiveValue { get; set; } = 1;

        public double IceEffectiveValue { get; set; } = 1;

        public double FightingEffectiveValue { get; set; } = 1;

        public double PoisonEffectiveValue { get; set; } = 1;

        public double GroundEffectiveValue { get; set; } = 1;

        public double FlyingEffectiveValue { get; set; } = 1;

        public double PsychicEffectiveValue { get; set; } = 1;

        public double BugEffectiveValue { get; set; } = 1;

        public double RockEffectiveValue { get; set; } = 1;

        public double GhostEffectiveValue { get; set; } = 1;

        public double DragonEffectiveValue { get; set; } = 1;

        public double DarkEffectiveValue { get; set; } = 1;

        public double SteelEffectiveValue { get; set; } = 1;

        public double FairyEffectiveValue { get; set; } = 1;

    }
}
