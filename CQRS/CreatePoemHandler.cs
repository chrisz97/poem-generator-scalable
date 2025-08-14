using MediatR;
using PoemGenerator.Models;
using PoemGenerator.Monolith.Cqrs.Commands;
using PoemGenerator.Monolith.Services;

namespace PoemGenerator.Monolith.Cqrs.Handlers;

public class CreatePoemHandler : IRequestHandler<CreatePoemCommand, Poem>
{
    private readonly IPoemService _service;

    public CreatePoemHandler(IPoemService service)
    {
        _service = service;
    }

    public async Task<Poem> Handle(CreatePoemCommand request, CancellationToken cancellationToken)
    {
        return await _service.CreatePoem(new CreatePoemRequest { Length = request.Length });
    }
}
