using API.Application.Services.Authentication;
using Microsoft.Extensions.Options;
using FluentAssertions;

namespace Tests.Pollinator
{
    public class JwtTests
    {
        [Fact]
        public void GenerateToken_ReturnsValidJwt()
        {
            //var sut = new JwtService(Options.Create(new JwtSettings { ... }));
            //var token = sut.GenerateToken(user);
            //token.Should().NotBeNullOrEmpty();
        }
    }
}
