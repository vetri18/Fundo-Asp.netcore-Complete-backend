using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.AddContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<CollaboratorEntity> CollaboratorTable { get; set; }
        public DbSet<LabelEntity> lableTable { get; set; }

    }
}


