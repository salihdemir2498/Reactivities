using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier); // Eğer kullanıcı kimliği yoksa (null), görev tamamlanır ve işlev Task.CompletedTask döndürerek sonlandırılır.

            if (userId == null) return Task.CompletedTask;

            var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString()); //Etkinlik kimliğini almak için HttpContextAccessor aracılığıyla HTTP isteği üzerinden etkinlik kimliği (id) alınır ve Guid.Parse yöntemiyle Guid türüne dönüştürülür.

            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId).Result; 


            if(attendee == null) return Task.CompletedTask; //Veritabanından (_dbContext.ActivityAttendees) etkinlik katılımcısını (attendee) kullanıcı kimliği ve etkinlik kimliğiyle arayarak bulur. Eğer katılımcı yoksa (null), görev tamamlanır ve işlev Task.CompletedTask döndürerek sonlandırılır.

            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
            //Eğer katılımcı varsa, katılımcının etkinlik sahibi (IsHost) olup olmadığı kontrol edilir. Eğer etkinlik sahibi ise, gereksinimi başarıyla tamamlamak için context.Succeed(requirement) çağrılır.

            //İşlem tamamlandığında, işlev Task.CompletedTask döndürerek sonlandırılır.
        }
    }
}
