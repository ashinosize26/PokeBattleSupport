using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;
using GrpcPoke;

namespace PokeBattleSupport.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokeBaseStatsController : ControllerBase
    {
        private readonly ILogger<PokeBaseStatsController> _logger;

        public PokeBaseStatsController(ILogger<PokeBaseStatsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<PokeBaseStatsModel> GetAsync([FromQuery] string name, [FromQuery] long generation)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7191");
            var client = new PokeSrv.PokeSrvClient(channel);
            PokeBaseStats baseStats = await client.GetPokeBaseStatsAsync(new PokeBaseStatsKey() { Name = name, Generation = generation });

            return new PokeBaseStatsModel() {
                Name = baseStats.Name,
                No = baseStats.No, 
                H = baseStats.H, 
                A = baseStats.A, 
                B = baseStats.B, 
                C = baseStats.C,
                D = baseStats.D,
                S = baseStats.S,
                Total = baseStats.Total
            };
        }
    }
}
