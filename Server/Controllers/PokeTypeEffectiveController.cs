using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;
using GrpcPoke;

namespace PokeBattleSupport.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokeTypeEffectiveController : ControllerBase
    {
        private readonly ILogger<PokeTypeEffectiveController> _logger;

        public PokeTypeEffectiveController(ILogger<PokeTypeEffectiveController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<PokeTypeEffectiveModel> GetAsync([FromQuery] string? firstType = null, [FromQuery] string? secondType= null)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7191");
            var client = new PokeSrv.PokeSrvClient(channel);
            PokeTypeEffective effectiveValues = await client.GetPokeTypeEffectiveAsync(new PokeTypes() {FirstType = firstType?? string.Empty, SecondType = secondType?? string.Empty});

            return new PokeTypeEffectiveModel()
            {
                FirstType = firstType?? string.Empty,
                SecondType = secondType?? string.Empty,
                NormalEffectiveValue   = effectiveValues.NormalValue,
                FireEffectiveValue     = effectiveValues.FireValue,
                WaterEffectiveValue    = effectiveValues.WaterValue,
                ElectricEffectiveValue = effectiveValues.ElecticValue,
                GrassEffectiveValue    = effectiveValues.GrassValue,
                IceEffectiveValue      = effectiveValues.IceValue,
                FightingEffectiveValue = effectiveValues.FightingValue,
                PoisonEffectiveValue   = effectiveValues.PoisonValue,
                GroundEffectiveValue   = effectiveValues.GroundValue,
                FlyingEffectiveValue   = effectiveValues.FlyingValue,
                PsychicEffectiveValue  = effectiveValues.PsychicValue,
                BugEffectiveValue      = effectiveValues.BugValue,
                RockEffectiveValue     = effectiveValues.RockValue,
                GhostEffectiveValue    = effectiveValues.GhostValue,
                DragonEffectiveValue   = effectiveValues.DragonValue,
                DarkEffectiveValue     = effectiveValues.DarkValue,
                SteelEffectiveValue    = effectiveValues.SteelValue,
                FairyEffectiveValue    = effectiveValues.FairyValue,
            };
        }
    }
}

