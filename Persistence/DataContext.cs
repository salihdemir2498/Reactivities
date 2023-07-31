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

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));//ActivityAttendee" varlýðý için birincil anahtar yapýlandýrmasýný belirtir. "AppUserId" ve "ActivityId" özelliklerinin birleþimiyle birincil anahtar oluþturulur.

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);//ActivityAttendee" varlýðý için "AppUser" ile iliþkiyi belirtir. Bir "AppUser"ýn birden fazla etkinliði olabilir.

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId); //esi, "ActivityAttendee" varlýðý için "Activity" ile iliþkiyi belirtir. Bir etkinliðin birden fazla katýlýmcýsý olabilir.
        }
    }
}