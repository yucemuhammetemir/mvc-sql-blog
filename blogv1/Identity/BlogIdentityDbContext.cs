﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blogv1.Identity
{
    public class BlogIdentityDbContext : IdentityDbContext<BlogIdentityUser,BlogIdentityRole,string>
    {
        public BlogIdentityDbContext(DbContextOptions<BlogIdentityDbContext> options):base(options) { 
        //base appsettings .js
        }
    }
}
