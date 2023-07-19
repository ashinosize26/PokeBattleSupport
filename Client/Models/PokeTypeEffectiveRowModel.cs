namespace PokeBattleSupport.Client.Models
{
    public class PokeTypeEffectiveRowModel
    {
        public string TypeName { get; set; } = string.Empty;

        public PokeTypeEffectiveMarkModel[] Marks { get; set; } = new PokeTypeEffectiveMarkModel[6];

        public int SuperEffectiveCount { get; set; }

        public int WeakAgainstCount { get; set; }
    }
}
