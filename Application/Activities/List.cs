using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var activities = await _context.Activities
                //    .Include(a => a.Attendees)
                //    .ThenInclude(u => u.AppUser)
                //    .ToListAsync();

                //buran�n eski hali ile yeni hali fark� ne? unutma bak!!!!!!!!

                var activities = await _context.Activities
                   .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();

                //Bu ifade, AutoMapper k�t�phanesinin ProjectTo y�ntemini kullanarak Activity nesnelerini ActivityDto nesnelerine d�n��t�r�r. ProjectTo y�ntemi, AutoMapper'�n bir performans optimizasyonu �zelli�idir. Veritaban�nda gereksiz sorgular�n ve fazla veri y�k�n�n �nlenmesi i�in kullan�l�r.

                //

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}