using Contest.Models.Score;
using Contest.Models.Survey;
using Contest.Web.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Contest.Data
{
    public class KolpiDbContext : IdentityDbContext
    {
        public KolpiDbContext(DbContextOptions<KolpiDbContext> options)
            : base(options)
        {
        }

        public DbSet<JudgeScore> JudgeScores { get; set; }
        public DbSet<ParticipantVote> ParticipantVotes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<SurveyThread> SurveyThreads { get; set; }
        public DbSet<Settings> Settings { get; set; }

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

            //Relate team to participant
            builder.Entity<Team>()
                .HasMany(t => t.Participants)
                .WithOne(p => p.Team)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SurveyThread>()
                .HasMany(f => f.Feedbacks)
                .WithOne(s => s.SurveyThread)
                .HasForeignKey(f => f.SurveyThreadId);

            builder.Entity<SurveyThread>()
                .HasOne(a => a.KolpiUser);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.SuperAdmin },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.Admin },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.Participant }
            );
        }
    }
}
