namespace PokeBattleSupport.Client.Models
{
    public class TypeEffectiveRowModel
    {
        public string AttackTypeName { get; set; } = string.Empty;

        public TypeEffectiveMarkModel[] Marks { get; set; } = new TypeEffectiveMarkModel[18];
    }
}
