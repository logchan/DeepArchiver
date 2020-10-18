using Microsoft.EntityFrameworkCore;

namespace DeepArchiver.Data {
    public sealed class RemoteDb : DbContext {
        public DbSet<RemoteFile> Files { get; set; }

        public RemoteDb(DbContextOptions<RemoteDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<RemoteFile>().HasIndex(f => f.FullName);
        }
    }
}
