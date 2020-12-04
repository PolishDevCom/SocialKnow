using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using SK.Application.Photos.Commands.AddPhoto;
using SK.Domain.Entities;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Photos.Commands.AddPhoto
{
    using static Testing;
    public class AddPhotoTest : TestBase
    {
        [Test]
        public async Task ShouldCreatePhoto()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");
            var command = new AddPhotoCommand() { File = file };

            //act
            var createdTestPhoto = await SendAsync(command);

            var createdTestPhotoFromDb = await FindByStringAsync<Photo>(createdTestPhoto.Id);

            //assert
            createdTestPhotoFromDb.Should().NotBeNull();
        }
    }
}
