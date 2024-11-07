using LibraryManagement.Core.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infra.Context
{
    public class LibraryManagementContext : DbContext
    {
        public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Membro> Membros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Livro>().HasKey(l => l.Id);
            builder.Entity<Membro>().HasKey(m => m.Id);
            builder.Entity<Emprestimo>().HasKey(e => e.Id);

            builder.Entity<Livro>()
                .Property(l => l.Disponivel)
                .HasDefaultValue(true);

            builder.Entity<Emprestimo>()
                .HasOne(l => l.Livro)
                .WithMany(b => b.Emprestimos)
                .HasForeignKey(l => l.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Emprestimo>()
                .HasOne(l => l.Membro)
                .WithMany(m => m.Emprestimos)
                .HasForeignKey(l => l.MembroId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Membro>()
                .HasIndex(m => m.Email)
                .IsUnique();
        }
    }
}
