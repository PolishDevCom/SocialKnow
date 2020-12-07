using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Photos.Commands.SetMainPhoto;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Photos.Commands.SetMainPhoto
{
    using static Testing;
    public class SetMainPhotoTest : TestBase
    {
        [Test]
        public async Task ShouldSetPhotoToMainPhoto()
        {
            //arrange
            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var photo1 = new Faker<Photo>()
                .RuleFor(p => p.Id, f => f.Lorem.Text())
                .RuleFor(p => p.IsMain, f => true)
                .RuleFor(p => p.Url, f => f.Lorem.Text()).Generate();
            var photo2 = new Faker<Photo>()
                .RuleFor(p => p.Id, f => f.Lorem.Text())
                .RuleFor(p => p.IsMain, f => false)
                .RuleFor(p => p.Url, f => f.Lorem.Text()).Generate();

            await AddPhotoToUserAsync(photo1, creatorUsername);
            await AddPhotoToUserAsync(photo2, creatorUsername);

            var setMainPhotoCommand = new SetMainPhotoCommand(photo2.Id);

            //act
            await SendAsync(setMainPhotoCommand);

            //assert
            var photo1FromDb = await FindByStringAsync<Photo>(photo1.Id);
            var photo2FromDb = await FindByStringAsync<Photo>(photo2.Id);
            photo1FromDb.IsMain.Should().BeFalse();
            photo2FromDb.IsMain.Should().BeTrue();
        }

        [Test]
        public async Task ShouldBeNotFoundExceptionWhenWrongPhototId()
        {
            //arrange
            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var photo1 = new Faker<Photo>()
                .RuleFor(p => p.Id, f => f.Lorem.Text())
                .RuleFor(p => p.IsMain, f => true)
                .RuleFor(p => p.Url, f => f.Lorem.Text()).Generate();
            var photo2 = new Faker<Photo>()
                .RuleFor(p => p.Id, f => f.Lorem.Text())
                .RuleFor(p => p.IsMain, f => false)
                .RuleFor(p => p.Url, f => f.Lorem.Text()).Generate();

            await AddPhotoToUserAsync(photo1, creatorUsername);
            await AddPhotoToUserAsync(photo2, creatorUsername);

            var setMainPhotoCommand = new SetMainPhotoCommand("wrongId");

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(setMainPhotoCommand)).Should().Throw<NotFoundException>();

        }
    }
}
