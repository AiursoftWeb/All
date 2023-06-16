using Aiursoft.SDK;
using Aiursoft.Warpgate.Data;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using static Aiursoft.WebTools.Extends;

namespace Aiursoft.Warpgate;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = App<Startup>(args);
        await app.UpdateDbAsync<WarpgateDbContext>();
        await app.RunAsync();
    }
}