namespace PokeBattleSupport.Client.Models
{
    public class PokeTypeEffectiveViewModel
    {
        public string TypeName { get; set; } = string.Empty;

        public TypeEffectiveMarkModel[] Marks { get; set; } = new TypeEffectiveMarkModel[6];

        public int SuperEffectiveCount { get; set; }

        public int WeakAgainstCount { get; set; }
    }
}
