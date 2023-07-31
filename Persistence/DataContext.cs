using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));//ActivityAttendee" varl��� i�in birincil anahtar yap�land�rmas�n� belirtir. "AppUserId" ve "ActivityId" �zelliklerinin birle�imiyle birincil anahtar olu�turulur.

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);//ActivityAttendee" varl��� i�in "AppUser" ile ili�kiyi belirtir. Bir "AppUser"�n birden fazla etkinli�i olabilir.

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId); //esi, "ActivityAttendee" varl��� i�in "Activity" ile ili�kiyi belirtir. Bir etkinli�in birden fazla kat�l�mc�s� olabilir.
        }
    }
}