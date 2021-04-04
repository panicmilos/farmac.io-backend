using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Farmacio_Tests.UnitTests.AuthenticationServiceTests
{
    public class AuthorizeTests
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Mock<IRepository<Account>> _accountRepository;

        public AuthorizeTests()
        {
            var tokenProvider = new Mock<ITokenProvider>();

            _accountRepository = new Mock<IRepository<Account>>();
            _accountRepository.Setup(repository => repository.Read()).Returns(new List<Account>()
            {
                new Account()
                {
                    Username = "admin",
                    Password = "kZY9HjWTKiPej5KwekXjjjuyvWXopFLFDSsEPx/WF2A=",
                    Salt = "MAThRdAuQ+FEICcu5u+w9pd8uYlw33qvulcwuNIqu6Q=",
                    IsVerified = true
                },
                new Account()
                {
                    Username = "panic",
                    Password = "mmd9YliTynAcpmdqP1eCCTTpNQ7HDZS6MDXadF501lg=",
                    Salt = "r+AN+b7OhwJHrDlN5vNWH/mJnKKpARur/8olKuovUZg=",
                    IsVerified = false
                }
            });
            _authenticationService = new AuthenticationService(_accountRepository.Object, tokenProvider.Object);
        }

        [Fact]
        public void Authorize_ThrowsCredentialsDontMatchAnyAccount_BecauseUsernameIsNotFound()
        {
            var username = "luka";
            var password = "luk1c@123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<CredentialsDontMatchAnyAccountException>();
            _accountRepository.Verify(repository => repository.Read(), Times.Once);
        }

        [Fact]
        public void Authorize_ThrowsCredentialsDontMatchAnyAccount_BecausePasswordIsWrong()
        {
            var username = "panic";
            var password = "panic123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<CredentialsDontMatchAnyAccountException>();
            _accountRepository.Verify(repository => repository.Read(), Times.Once);
        }

        [Fact]
        public void Authorize_ThrowsNotVerifiedAccountException_BecauseAccountIsNotVerified()
        {
            var username = "panic";
            var password = "p@anic123";

            Action authorizationAction = () => _authenticationService.Authenticate(username, password);

            authorizationAction.Should().Throw<NotVerifiedAccountException>();
            _accountRepository.Verify(repository => repository.Read(), Times.Once);
        }

        [Fact]
        public void Authorize_ReturnsAuthenticatedUserDTO()
        {
            var username = "admin";
            var password = "@admin123";

            var authenticatedUser = _authenticationService.Authenticate(username, password);

            authenticatedUser.Should().NotBeNull();
            _accountRepository.Verify(repository => repository.Read(), Times.Once);
        }
    }
}