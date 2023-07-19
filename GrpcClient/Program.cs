using Grpc.Net.Client;
using GrpcPoke;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7191");
var client = new PokeSrv.PokeSrvClient(channel);
PokeNames pokeNames = await client.GetPokeNamesAsync(new Empty());

foreach (var item in pokeNames.Items)
{
    Console.WriteLine(item.Name);
}
Console.ReadKey();