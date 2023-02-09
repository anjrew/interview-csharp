using FluentValidation;
using HashidsNet;
using MediatR;
using UrlShortenerService.Application.Common.Interfaces;

namespace UrlShortenerService.Application.Url.Commands;

public record CreateShortUrlCommand : IRequest<string>
{
    public string Url { get; init; } = default!;
}

public class CreateShortUrlCommandValidator : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator()
    {
        _ = RuleFor(v => v.Url)
          .NotEmpty()
          .WithMessage("Url is required.");
    }
}

public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public CreateShortUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var originalUrl = request.Url;

        // Url is already an existing Class
        var shortUrl = new UrlShortenerService.Domain.Entities.Url()
        {
            OriginalUrl = originalUrl        
        };

        _context.Urls.Add(shortUrl);

        var id = await _context.SaveChangesAsync(cancellationToken);

        var idHash = _hashids.Encode(id);

        // Return the shortened hash of the URL ID
        return $"http://localhost:5246/u/{idHash}";
    }
}
