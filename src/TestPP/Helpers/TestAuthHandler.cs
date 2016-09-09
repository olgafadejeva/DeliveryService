using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceTests.Helpers
{
    public class TestAuthHandler : IAuthenticationHandler
    {
        public void Authenticate(AuthenticateContext context)
        {
            context.NotAuthenticated();
        }

        public Task AuthenticateAsync(AuthenticateContext context)
        {
            context.NotAuthenticated();
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(ChallengeContext context)
        {
            throw new NotImplementedException();
        }

        public void GetDescriptions(DescribeSchemesContext context)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(SignInContext context)
        {
            context.Accept();
            return Task.FromResult(0);
        }

        public Task SignOutAsync(SignOutContext context)
        {
            throw new NotImplementedException();
        }
    }
}
