﻿using Aiursoft.WWW.Data;
using Aiursoft.DbTools;
using static Aiursoft.WebTools.Extends;

namespace Aiursoft.WWW;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = App<Startup>(args);
        await app.UpdateDbAsync<WWWDbContext>(UpdateMode.MigrateThenUse);
        await app.RunAsync();
    }
}