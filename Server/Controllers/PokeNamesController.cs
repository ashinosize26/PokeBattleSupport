using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;
using Grpc.Net.Client;
using GrpcPoke;

namespace PokeBattleSupport.Server.Controllers
{
    // [controller]‚ÍƒNƒ‰ƒX–¼‚©‚çContrller‚ğæ‚èœ‚¢‚½‚à‚Ì‚É’uŠ·‚³‚ê‚é
    [ApiController]
    [Route("api/[controller]")]
    public class PokeNamesController : ControllerBase
    {
        private readonly ILogger<PokeNamesController> _logger;

        public PokeNamesController(ILogger<PokeNamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7191");
            var client = new PokeSrv.PokeSrvClient(channel);
            PokeNames pokeNames = await client.GetPokeNamesAsync(new Empty());

            return pokeNames.Items.Select(x => x.Name);
        }
    }
}