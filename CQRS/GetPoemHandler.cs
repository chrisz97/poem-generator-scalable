using MediatR;
using PoemGenerator.Models;
using PoemGenerator.Monolith.Cqrs.Queries;
using PoemGenerator.Monolith.Services;

namespace PoemGenerator.Monolith.Cqrs.Handlers;

public class GetPoemByIdHandler : IRequestHandler<GetPoemByIdQuery, Poem>
{
    private readonly IPoemService _service;

    public GetPoemByIdHandler(IPoemService service)
    {
        _service = service;
    }

    public async Task<Poem> Handle(GetPoemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetPoem(request.Id);
    }
}
