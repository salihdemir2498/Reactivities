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

            //Bu sat�r, Activity s�n�f�ndaki Attendees koleksiyonundan, IsHost �zelli�i true olan kat�l�mc�n�n AppUser �zelli�inin UserName de�erini, ActivityDto s�n�f�ndaki HostUserName �zelli�ine e�lemek i�in bir kural belirtir.

           // s.Attendees: Activity s�n�f�n�n i�indeki Attendees koleksiyonunu temsil eder.Bu koleksiyon, etkinlik kat�l�mc�lar�n� i�erir.

          //FirstOrDefault(x => x.IsHost): Attendees koleksiyonunda, IsHost �zelli�i true olan ilk kat�l�mc�y� se�er. FirstOrDefault metodu, koleksiyon i�inde belirtilen ko�ulu sa�layan ilk ��eyi veya koleksiyon bo�sa varsay�lan de�eri d�nd�r�r.

         //.AppUser: Se�ilen kat�l�mc�n�n, AppUser �zelli�ini temsil eder.Bu �zellik, ActivityAttendee s�n�f�nda tan�ml� olan kullan�c� nesnesini i�erir.

         //.AppUser.UserName: Se�ilen kat�l�mc�n�n AppUser �zelli�indeki kullan�c� nesnesinin UserName �zelli�ini temsil eder. Bu �zellik, etkinli�in sahibi(host) kullan�c�n�n kullan�c� ad�n� i�erir.

        //.ForMember(d => d.HostUserName, o => o.MapFrom(...): Bu ifade, Activity s�n�f�ndaki Attendees koleksiyonundan elde edilen verileri, ActivityDto s�n�f�ndaki HostUserName �zelli�ine e�lemek i�in kullan�lan kural� belirtir.

        //Sonu� olarak, HostUserName �zelli�i, etkinli�in sahibi(host) kullan�c�n�n kullan�c� ad�n� i�eren ActivityDto nesnesine atan�r.Bu sayede, veritaban�ndan �ekilen etkinlik verileri, ActivityDto nesnelerine d�n��t�r�l�rken sahibi olan kullan�c�n�n kullan�c� ad� da ActivityDto nesnesine eklenir.



            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))//Bu sat�r, ActivityAttendee s�n�f�ndaki AppUser �zelli�inin DisplayName �zelli�ini, Profiles.Profile s�n�f�ndaki DisplayName �zelli�ine e�lemek i�in bir kural belirtir. 

                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))//Bu sat�r, ActivityAttendee s�n�f�ndaki AppUser �zelli�inin UserName �zelli�ini, Profiles.Profile s�n�f�ndaki Username �zelli�ine e�lemek i�in bir kural belirtir. 

                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));//Bu sat�r, ActivityAttendee s�n�f�ndaki AppUser �zelli�inin Bio �zelli�ini, Profiles.Profile s�n�f�ndaki Bio �zelli�ine e�lemek i�in bir kural belirtir.


            //Bu e�leme, ActivityAttendee s�n�f�ndaki �zellikleri, Profiles.Profile s�n�f�ndaki �zelliklere e�ler. Bu e�lemeyi yaparken, ActivityAttendee s�n�f� ile ili�kili AppUser s�n�f�ndan �zellikleri alarak Profiles.Profile s�n�f�n� doldurur. Bu sayede, ActivityAttendee ile Profiles.Profile aras�nda d�n���mler yap�l�r ve etkinlik kat�l�mc�lar�na ili�kin kullan�c� profili bilgileri elde edilir.
        }
    }
}