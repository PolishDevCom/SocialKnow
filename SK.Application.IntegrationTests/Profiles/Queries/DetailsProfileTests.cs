using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Profiles.Queries.DetailsProfile;
using System.Threading.Tasks;

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
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }
    }
}