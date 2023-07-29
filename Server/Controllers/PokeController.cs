using Grpc.Net.Client;
using GrpcPoke;
using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;

namespace PokeBattleSupport.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokeController : ControllerBase
    {
        private readonly ILogger<PokeController> _logger;

        public PokeController(ILogger<PokeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 世代のデフォルト
        /// </summary>
        private const long defaultGeneration = 9;

        /// <summary>
        /// gRPCサーバーのURL
        /// </summary>
        private const string gRPCServerUrl = "https://localhost:7191";

        /// <summary>
        /// ポケモンの名前リストを取得
        /// </summary>
        /// <returns></returns>
        [HttpGet("Names")]
        public async Task<IEnumerable<string>> GetNamesAsync()
        {
            // gRPCクライアントの生成
            using var channel = GrpcChannel.ForAddress(gRPCServerUrl);
            PokeSrv.PokeSrvClient client = new (channel);

            // ポケモンの名前リストを取得
            PokeNames pokeNames = await client.GetPokeNamesAsync(new Empty());
            return pokeNames.Items.Select(x => x.Name);
        }

        /// <summary>
        /// ポケモンの基本情報を取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<PokeSimpleInfoModel> GetSimpleInfoAsync(string name)
        {
            // gRPCクライアントの生成
            using var channel = GrpcChannel.ForAddress(gRPCServerUrl);
            PokeSrv.PokeSrvClient client = new(channel);

            // 基本情報の取得
            PokeSimpleInfo poke = await client.GetPokeSimpleInfoAsync(new PokeName() { Name = name });
            return new PokeSimpleInfoModel() { Name = poke.Name, No = poke.No, FirstType = poke.FirstType, SecondType = poke.SecondType };
        }

        /// <summary>
        /// ポケモンの種族値を取得
        /// </summary>
        /// <param name="name"></param>
        /// <param name="generation"></param>
        /// <returns></returns>
        [HttpGet("{name}/BaseStats")]
        public async Task<PokeBaseStatsModel> GetBaseStatsAsync(string name, [FromQuery] long generation = defaultGeneration)
        {
            // gRPCクライアントの生成
            using var channel = GrpcChannel.ForAddress(gRPCServerUrl);
            PokeSrv.PokeSrvClient client = new(channel);

            // 種族値を取得
            PokeBaseStats baseStats = await client.GetPokeBaseStatsAsync(new PokeBaseStatsKey() { Name = name, Generation = generation });
            return new PokeBaseStatsModel() {
                H = baseStats.H, 
                A = baseStats.A, 
                B = baseStats.B, 
                C = baseStats.C,
                D = baseStats.D,
                S = baseStats.S,
                Total = baseStats.Total
            };
        }

        /// <summary>
        /// ポケモンのタイプ耐性を取得
        /// </summary>
        /// <param name="firstType"></param>
        /// <param name="secondType"></param>
        /// <returns></returns>
        [HttpGet("{name}/TypeEffective")]
        public async Task<PokeTypeEffectiveModel> GetEffectiveAsync(string name)
        {
            // gRPCクライアントの生成
            using var channel = GrpcChannel.ForAddress(gRPCServerUrl);
            PokeSrv.PokeSrvClient client = new(channel);

            // タイプ耐性を取得
            PokeTypeEffective effectiveValues = await client.GetPokeTypeEffectiveAsync(new PokeName() {Name = name });
            return new PokeTypeEffectiveModel()
            {
                NormalEffectiveValue = effectiveValues.NormalValue,
                FireEffectiveValue = effectiveValues.FireValue,
                WaterEffectiveValue = effectiveValues.WaterValue,
                ElectricEffectiveValue = effectiveValues.ElecticValue,
                GrassEffectiveValue = effectiveValues.GrassValue,
                IceEffectiveValue = effectiveValues.IceValue,
                FightingEffectiveValue = effectiveValues.FightingValue,
                PoisonEffectiveValue = effectiveValues.PoisonValue,
                GroundEffectiveValue = effectiveValues.GroundValue,
                FlyingEffectiveValue = effectiveValues.FlyingValue,
                PsychicEffectiveValue = effectiveValues.PsychicValue,
                BugEffectiveValue = effectiveValues.BugValue,
                RockEffectiveValue = effectiveValues.RockValue,
                GhostEffectiveValue = effectiveValues.GhostValue,
                DragonEffectiveValue = effectiveValues.DragonValue,
                DarkEffectiveValue = effectiveValues.DarkValue,
                SteelEffectiveValue = effectiveValues.SteelValue,
                FairyEffectiveValue = effectiveValues.FairyValue,
            };
        }

        /// <summary>
        /// タイプ相性表を取得
        /// </summary>
        /// <returns></returns>
        [HttpGet("TypeEffectiveTable")]
        public async Task<IEnumerable<TypeEffectiveTableModel>> GetEffectiveTableAsync()
        {
            // gRPCクライアントの生成
            using var channel = GrpcChannel.ForAddress(gRPCServerUrl);
            PokeSrv.PokeSrvClient client = new(channel);

            // タイプ相性表を取得
            TypeEffectiveTable typeEffectiveTable = await client.GetTypeEffectiveTableAsync(new Empty());
            return typeEffectiveTable.Items.Select(x => new TypeEffectiveTableModel()
                {
                    AttackType = x.AttackType,
                    NormalEffectiveValue = x.TypeEffective.NormalValue,
                    FireEffectiveValue = x.TypeEffective.FireValue,
                    WaterEffectiveValue = x.TypeEffective.WaterValue,
                    ElectricEffectiveValue = x.TypeEffective.ElecticValue,
                    GrassEffectiveValue = x.TypeEffective.GrassValue,
                    IceEffectiveValue = x.TypeEffective.IceValue,
                    FightingEffectiveValue = x.TypeEffective.FightingValue,
                    PoisonEffectiveValue = x.TypeEffective.PoisonValue,
                    GroundEffectiveValue = x.TypeEffective.GroundValue,
                    FlyingEffectiveValue = x.TypeEffective.FlyingValue,
                    PsychicEffectiveValue = x.TypeEffective.PsychicValue,
                    BugEffectiveValue = x.TypeEffective.BugValue,
                    RockEffectiveValue = x.TypeEffective.RockValue,
                    GhostEffectiveValue = x.TypeEffective.GhostValue,
                    DragonEffectiveValue = x.TypeEffective.DragonValue,
                    DarkEffectiveValue = x.TypeEffective.DarkValue,
                    SteelEffectiveValue = x.TypeEffective.SteelValue,
                    FairyEffectiveValue = x.TypeEffective.FairyValue,
                });
        }
    }
}
