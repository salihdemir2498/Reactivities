using Application.Activities;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();

            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUserName, o => o.MapFrom(s => s.Attendees
                    .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            //Bu satýr, Activity sýnýfýndaki Attendees koleksiyonundan, IsHost özelliði true olan katýlýmcýnýn AppUser özelliðinin UserName deðerini, ActivityDto sýnýfýndaki HostUserName özelliðine eþlemek için bir kural belirtir.

           // s.Attendees: Activity sýnýfýnýn içindeki Attendees koleksiyonunu temsil eder.Bu koleksiyon, etkinlik katýlýmcýlarýný içerir.

          //FirstOrDefault(x => x.IsHost): Attendees koleksiyonunda, IsHost özelliði true olan ilk katýlýmcýyý seçer. FirstOrDefault metodu, koleksiyon içinde belirtilen koþulu saðlayan ilk öðeyi veya koleksiyon boþsa varsayýlan deðeri döndürür.

         //.AppUser: Seçilen katýlýmcýnýn, AppUser özelliðini temsil eder.Bu özellik, ActivityAttendee sýnýfýnda tanýmlý olan kullanýcý nesnesini içerir.

         //.AppUser.UserName: Seçilen katýlýmcýnýn AppUser özelliðindeki kullanýcý nesnesinin UserName özelliðini temsil eder. Bu özellik, etkinliðin sahibi(host) kullanýcýnýn kullanýcý adýný içerir.

        //.ForMember(d => d.HostUserName, o => o.MapFrom(...): Bu ifade, Activity sýnýfýndaki Attendees koleksiyonundan elde edilen verileri, ActivityDto sýnýfýndaki HostUserName özelliðine eþlemek için kullanýlan kuralý belirtir.

        //Sonuç olarak, HostUserName özelliði, etkinliðin sahibi(host) kullanýcýnýn kullanýcý adýný içeren ActivityDto nesnesine atanýr.Bu sayede, veritabanýndan çekilen etkinlik verileri, ActivityDto nesnelerine dönüþtürülürken sahibi olan kullanýcýnýn kullanýcý adý da ActivityDto nesnesine eklenir.



            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))//Bu satýr, ActivityAttendee sýnýfýndaki AppUser özelliðinin DisplayName özelliðini, Profiles.Profile sýnýfýndaki DisplayName özelliðine eþlemek için bir kural belirtir. 

                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))//Bu satýr, ActivityAttendee sýnýfýndaki AppUser özelliðinin UserName özelliðini, Profiles.Profile sýnýfýndaki Username özelliðine eþlemek için bir kural belirtir. 

                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));//Bu satýr, ActivityAttendee sýnýfýndaki AppUser özelliðinin Bio özelliðini, Profiles.Profile sýnýfýndaki Bio özelliðine eþlemek için bir kural belirtir.


            //Bu eþleme, ActivityAttendee sýnýfýndaki özellikleri, Profiles.Profile sýnýfýndaki özelliklere eþler. Bu eþlemeyi yaparken, ActivityAttendee sýnýfý ile iliþkili AppUser sýnýfýndan özellikleri alarak Profiles.Profile sýnýfýný doldurur. Bu sayede, ActivityAttendee ile Profiles.Profile arasýnda dönüþümler yapýlýr ve etkinlik katýlýmcýlarýna iliþkin kullanýcý profili bilgileri elde edilir.
        }
    }
}