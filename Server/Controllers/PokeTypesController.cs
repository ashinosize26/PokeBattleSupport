using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using PokeBattleSupport.Shared;
using GrpcPoke;

namespace PokeBattleSupport.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokeTypesController : ControllerBase
    {
        private readonly ILogger<PokeTypesController> _logger;

        public PokeTypesController(ILogger<PokeTypesController> logger)
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
    }
}
