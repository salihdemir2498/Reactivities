using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> //Result<Unit> tipi ise geri dönüþ deðeri olmayan bir iþlemin sonucunu temsil eder.                                               Ýþlem baþarýlýmý naþarýsýz mý onu döndürmek için
        {
           public Activity Activity;
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0; //veritabanýndaki deðiþiklikleri kaydetmeyi dener ve geri dönen deðer 0'dan büyükse (yani en az bir satýr etkilendiyse) result deðiþkenine true deðerini atar.

                if (!result) return Result<Unit>.Failure("Failed to create activity"); 

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}