using EFTopics.DAL.Configuration;
using EFTopics.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.DAL.Data
{
    public class TopicsContext : DbContext
    {
        public DbSet<Topics> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TopicsConfiguration());
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WhatToRead2;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=True;");
            }
        }
        public TopicsContext(DbContextOptions<TopicsContext> options)
            : base(options)
        {
        }
    }
}
