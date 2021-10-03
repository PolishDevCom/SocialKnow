using Bogus;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.Profiles.Commands.EditProfile;
using SK.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Profiles.Commands
{
    using static Testing;

    public class EditProfileTests
    {
        [Test]
        public async Task ShouldUpdateProfile()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            //act
            var command = new Faker<EditProfileCommand>("en")
                .RuleFor(e => e.Age, f => 20)
                .RuleFor(e => e.City, f => "Cracov")
                .RuleFor(e => e.Nickname, f => "Scotty")
                .RuleFor(e => e.ShortBio, f => "I like to play bass")
                .RuleFor(e => e.UserGender, f => Gender.Male).Generate();
            await SendAsync(command);

            var actualUser = await FindAppUserByUsername(loggedUser);

            //assert
            actualUser.Should().NotBeNull();
            actualUser.Age.Should().Be(command.Age);
            actualUser.City.Should().Be(command.City);
            actualUser.Nickname.Should().Be(command.Nickname);
            actualUser.ShortBio.Should().Be(command.ShortBio);
            actualUser.UserGender.Should().Be(command.UserGender);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //act
            var command = new Faker<EditProfileCommand>("en")
                .RuleFor(e => e.Age, f => 20)
                .RuleFor(e => e.City, f => "Cracov")
                .RuleFor(e => e.Nickname, f => "Stefano")
                .RuleFor(e => e.ShortBio, f => "I like to play bass")
                .RuleFor(e => e.UserGender, f => Gender.Male).Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringEditingProfileTestCases
        {
            get
            {
                yield return new TestCaseData(20, null)
                    .SetName("ProfileNicknameMissingTest");
                yield return new TestCaseData(null, "Jack")
                    .SetName("ProfileAgeMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringEditingProfileTestCases))]
        public async Task ShouldThrowValidationExceptionDuringProfileEditing(int testAge, string testNickname)
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var command = new EditProfileCommand()
            {
                Age = testAge,
                Nickname = testNickname
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}