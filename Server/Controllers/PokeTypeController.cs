using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;
using GrpcPoke;

namespace PokeBattleSupport.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokeTypeController : ControllerBase
    {
        private readonly ILogger<PokeTypeController> _logger;

        public PokeTypeController(ILogger<PokeTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<PokeTypeModel> GetAsync([FromQuery] string name)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7191");
            var client = new PokeSrv.PokeSrvClient(channel);
            PokeTypes poke = await client.GetPokeTypesAsync(new PokeName() { Name = name });

            return new PokeTypeModel() { Name = poke.Name, No = poke.No, FirstType = poke.FirstType, SecondType = poke.SecondType };
        }

        [HttpGet("Effective")]
        public async Task<PokeTypeEffectiveModel> GetEffectiveAsync([FromQuery] string? firstType = null, [FromQuery] string? secondType = null)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7191");
            var client = new PokeSrv.PokeSrvClient(channel);
            PokeTypeEffective effectiveValues = await client.GetPokeTypeEffectiveAsync(new PokeTypes() { FirstType = firstType ?? string.Empty, SecondType = secondType ?? string.Empty });

            return new PokeTypeEffectiveModel()
            {
                FirstType = firstType ?? string.Empty,
                SecondType = secondType ?? string.Empty,
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
    }
}
