using Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Configuration
{
    public class ContextBase : IdentityDbContext<ApplicationUser>
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) {}
        public DbSet<Message> Message { get; set; } // são os objetos mapeados para o BD
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // Caso no Program não tenha a config de conexão, será feita aqui
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ObterStringConexao());
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder) //Mapeia a chave primária da tabela do Identity de usuário
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder);
        }

        public string ObterStringConexao() //String de conexão local do meu banco
        {
           return "Data Source=DESKTOP-L8175GI\\SQLEXPRESS;Initial Catalog=master;Integrated Security=False;User ID=sa;Password=123456;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        }
    }
}
