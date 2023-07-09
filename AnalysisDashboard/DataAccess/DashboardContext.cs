using AnalysisDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalysisDashboard.DataAccess
{
    public class DashboardContext : DbContext
    {
        public DbSet<RegistrationInfo> RegistrationInfos { get; set; }
        public DashboardContext(DbContextOptions options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationInfo>().ToTable("RegistrationInfos");
            modelBuilder.Entity<RegistrationInfo>().HasKey(r => r.Id);
        }
    }
}
