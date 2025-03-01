﻿using Microsoft.EntityFrameworkCore;
using PokesMan.Models;

namespace PokesMan.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }

   
}
