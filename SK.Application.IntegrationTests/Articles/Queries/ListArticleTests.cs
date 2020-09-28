using FluentAssertions;
using NUnit.Framework;
using SK.Application.Articles.Queries.ListArticle;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Queries
{

    using static Testing;

    public class ListArticleTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllArticlesAsAList()
        {
            //arrange
            await AddAsync(new Article
            {
                Id = Guid.NewGuid(),
                Title = "First Article",
                Abstract = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis eros quam, scelerisque eu odio a, maximus gravida sapien. Proin velit sapien, placerat nec lorem sit amet, tempor egestas velit. Integer ullamcorper erat dolor, quis sagittis enim pharetra eget. Vestibulum magna ex, semper in efficitur vitae, accumsan eget eros.",
                Image = null,
                Content = "In hac habitasse platea dictumst. Nulla efficitur justo eu nisi commodo blandit. Integer tempus sit amet urna a lobortis. Praesent lacus ipsum, accumsan vitae lectus eget, sodales dapibus diam. Suspendisse hendrerit rutrum aliquet. Curabitur vel aliquet eros. Sed in lorem faucibus, pharetra odio vel, iaculis nunc.",
                CreatedBy = "Content Admin",
                Created = DateTime.UtcNow
            });
            await AddAsync(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Second Article",
                Abstract = "Nulla ullamcorper justo ex, vel ultricies augue tincidunt porta. Phasellus eros lorem, vehicula tincidunt tempor consectetur, eleifend molestie urna.",
                Image = null,
                Content = "Duis tempus dolor nec ante ullamcorper consequat. Pellentesque sagittis mauris condimentum sollicitudin aliquet. Fusce tortor lorem, dignissim ac scelerisque at, blandit viverra leo. Phasellus et lorem sit amet quam sollicitudin ultrices sit amet blandit nisi. Sed ultrices malesuada purus, eu blandit nibh sodales id. Praesent ac turpis eget arcu luctus ullamcorper.",
                CreatedBy = "Content Admin",
                Created = DateTime.UtcNow.AddDays(-7)
            });
            await AddAsync(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Third Article",
                Abstract = "Donec sagittis tincidunt tempus. Vestibulum a enim est. Donec ut odio sollicitudin lacus blandit porttitor sed interdum quam. Curabitur eget auctor erat. Sed at enim imperdiet, efficitur lectus id, pellentesque dui.",
                Image = null,
                Content = "Aenean odio dolor, mollis non diam a, dictum iaculis enim. Praesent ultrices pharetra dolor, ac bibendum felis scelerisque quis. Donec congue mi ligula, ac feugiat nunc porttitor at. Aenean ornare accumsan auctor. Vestibulum dictum orci vel aliquet sodales. Pellentesque pulvinar at eros nec euismod.",
                CreatedBy = "Content Admin",
                Created = DateTime.UtcNow.AddDays(-3)
            });

            var query = new ListArticleQuery();

            //act
            var result = await SendAsync(query);

            //assert
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("First Article");
            result[1].Title.Should().Be("Third Article");
            result[2].Title.Should().Be("Second Article");
        }
    }
}
