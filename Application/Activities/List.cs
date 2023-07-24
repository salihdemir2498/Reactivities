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

                //buranýn eski hali ile yeni hali farký ne? unutma bak!!!!!!!!

                var activities = await _context.Activities
                   .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();

                //Bu ifade, AutoMapper kütüphanesinin ProjectTo yöntemini kullanarak Activity nesnelerini ActivityDto nesnelerine dönüþtürür. ProjectTo yöntemi, AutoMapper'ýn bir performans optimizasyonu özelliðidir. Veritabanýnda gereksiz sorgularýn ve fazla veri yükünün önlenmesi için kullanýlýr.

                //

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}