using System.Reflection;
using UrlShortenerService.Application.Url;
using UrlShortenerService.Application.Common.Interfaces;
using HashidsNet;
using Moq;
namespace Application.tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // IApplicationDbContext context, IHashids hashids
        var context = new Mock<IApplicationDbContext>();
        var hashids = new Mock<IHashids>();
        context.Setup(x => x.Hashids).Returns(hashids.Object);
        CreateShortUrlCommandHandler handler  = new CreateShortUrlCommandHandler(context.Object, hashids.Object);
        var result =  await handler.Handle("https://www.google.com", CancellationToken.None);
        Assert.Equal(result, "https://www.google.com");

    }
}