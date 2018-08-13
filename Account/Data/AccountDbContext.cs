﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Aiursoft.Account.Models;

namespace Aiursoft.Account.Data
{
    public class AccountDbContext : IdentityDbContext<AccountUser>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {

        }

        public DbSet<OAuthPack> OAuthPack { get; set; }
        public DbSet<AppGrant> LocalAppGrant { get; set; }
        public DbSet<UserEmail> UserEmails { get; set; }
    }
}
