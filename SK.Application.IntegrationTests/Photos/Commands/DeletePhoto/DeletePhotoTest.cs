using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Photos.Commands.DeletePhoto;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Photos.Commands.DeletePhoto
{
    using static Testing;
    public class DeletePhotoTest : TestBase
    {
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

            var deletePhotoCommand = new DeletePhotoCommand("wrongId");

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deletePhotoCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldBeRestExceptionWhenMainId()
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

            var deletePhotoCommand = new DeletePhotoCommand(photo1.Id);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deletePhotoCommand)).Should().ThrowAsync<RestException>();
        }
    }
}
