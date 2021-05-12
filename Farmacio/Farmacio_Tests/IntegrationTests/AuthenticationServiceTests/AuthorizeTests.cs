using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Farmacio_Tests.IntegrationTests.AuthenticationServiceTests
{
    public class AuthorizeTests : FarmacioTestBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository<Account> _accountRepository;

        public AuthorizeTests()
        {
            var tokenProvider = new Mock<ITokenProvider>();

            _accountRepository = new Repository<Account>(context);
            _authenticationService = new AuthenticationService(_accountRepository, tokenProvider.Object);
        }

        [Fact]
        public void Authorize_ThrowsCredentialsDontMatchAnyAccount_BecauseUsernameIsNotFound()
        {
            var username = "luka";
            var password = "luk1c@123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<CredentialsDontMatchAnyAccountException>();
        }

        [Fact]
        public void Authorize_ThrowsCredentialsDontMatchAnyAccount_BecausePasswordIsWrong()
        {
            var username = "panic";
            var password = "panic123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<CredentialsDontMatchAnyAccountException>();
        }

        [Fact]
        public void Authorize_ThrowsNotVerifiedAccountException_BecauseAccountIsNotVerified()
        {
            var username = "janko";
            var password = "janko123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<NotVerifiedAccountException>();
        }

        [Fact]
        public void Authorize_ReturnsAuthenticatedUserDTO()
        {
            var username = "sysadmin";
            var password = "admin123";

            var authenticatedUser = _authenticationService.Authenticate(username, password);

            authenticatedUser.Should().NotBeNull();
        }
    }
}