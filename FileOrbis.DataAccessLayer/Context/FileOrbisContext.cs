using FileOrbis.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Context
{
    public class FileOrbisContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=FILEORBIS;database=FileOrbisDb;integrated security=true; TrustServerCertificate=True;");

        }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<FileInfos> FileInfo { get; set; }
        public DbSet<FolderInfo> FolderInfo { get; set; }
    }
}
