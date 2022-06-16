using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.User.Queries.CheckUserNameForExists
{
    public class CheckUserNameForExistsQueryHandler : IRequestHandler<CheckUserNameForExistsQuery, bool>
    {
        private readonly IApplicationIdentityDbContext _identityContext;

        public CheckUserNameForExistsQueryHandler(IApplicationIdentityDbContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<bool> Handle(CheckUserNameForExistsQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityContext.Users
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.UserName));

            return result is not null;
        }
    }
}
