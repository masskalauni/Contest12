using Kolpi.Models.ScoreCard;
using Kolpi.Models.Survey;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kolpi.Data
{
    public class KolpiDbContext : IdentityDbContext
    {
        public KolpiDbContext(DbContextOptions<KolpiDbContext> options)
            : base(options)
        {
        }

        public DbSet<TeamScore> TeamScores { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SurveyThread> SurveyThreads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            builder.Entity<SurveyThread>()
                .HasMany(f => f.Feedbacks)
                .WithOne(s => s.SurveyThread)
                .HasForeignKey(f => f.SurveyThreadId);

            builder.Entity<SurveyThread>()
                .HasOne(a => a.KolpiUser);
        }
    }
}
