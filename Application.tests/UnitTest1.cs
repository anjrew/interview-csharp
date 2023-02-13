using UrlShortenerService.Application.Common.Interfaces;
using UrlShortenerService.Application.Url.Commands;
using UrlShortenerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HashidsNet;
using Moq;

namespace Application.tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {

        Func<string, string> getExpected = idHash => $"http://localhost:5246/u/{idHash}";
        int id = 1;
        string mock_hash = "erf22";
        string expected = getExpected(mock_hash);

        // IApplicationDbContext context, IHashids hashids
        var context = new Mock<IApplicationDbContext>();
        var hashids = new Mock<IHashids>();

        var mockSet = new Mock<DbSet<Url>>();
        context.Setup(x =>  x.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(id));
        context.Setup(x =>  x.Urls).Returns(mockSet.Object);
        hashids.Setup(x =>  x.Encode()).Returns(mock_hash);

        CreateShortUrlCommandHandler handler  = new CreateShortUrlCommandHandler(context.Object, hashids.Object);

        var request = new CreateShortUrlCommand() {
            Url = "http://www.google.com"
        };

        var result =  await handler.Handle(request, CancellationToken.None);

        Assert.Equal(expected, result);

    }
}