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
        public class Command : IRequest<Result<Unit>> //Result<Unit> tipi ise geri d�n�� de�eri olmayan bir i�lemin sonucunu temsil eder.                                               ��lem ba�ar�l�m� na�ar�s�z m� onu d�nd�rmek i�in
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

                var result = await _context.SaveChangesAsync() > 0; //veritaban�ndaki de�i�iklikleri kaydetmeyi dener ve geri d�nen de�er 0'dan b�y�kse (yani en az bir sat�r etkilendiyse) result de�i�kenine true de�erini atar.

                if (!result) return Result<Unit>.Failure("Failed to create activity"); 

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}