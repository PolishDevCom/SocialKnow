using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using SK.Application.Profiles.Queries.DetailsProfile;
using SK.Application.Common.Exceptions;

namespace SK.Application.IntegrationTests.Profiles.Queries
{
    using static Testing;
    public class DetailsProfileTests : TestBase
    {
        [Test]
        public async Task ShouldReturnUserProfile()
        {
            //arrange
            string testedUsername = "jane@testhost";
            await RunAsUserAsync(testedUsername, "Pa$$w0rd!");
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var query = new DetailsProfileQuery(testedUsername);

            //act
            var result = await SendAsync(query);

            //assert
            result.Username.Should().Be(testedUsername);
        }

        [Test]
        public async Task ShouldReturnNotFoundExceptionWhenWrongUsername()
        {
            //arrange
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var query = new DetailsProfileQuery("notExistingUsername");

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
    }
}
