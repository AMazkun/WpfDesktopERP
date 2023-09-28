using System.Data.Entity;

namespace WpfApp5.DBContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Logs)
                .WithOptional(e => e.theUser)
                .HasForeignKey(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Settings)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
