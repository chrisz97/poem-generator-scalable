using MediatR;
using PoemGenerator.Models;
using PoemGenerator.Monolith.Cqrs.Queries;
using PoemGenerator.Monolith.Services;

namespace PoemGenerator.Monolith.Cqrs.Handlers;

public class GetAllPoemsHandler : IRequestHandler<GetAllPoemsQuery, IEnumerable<Poem>>
{
    private readonly IPoemService _service;

    public GetAllPoemsHandler(IPoemService service)
    {
        _service = service;
    }

    public async Task<IEnumerable<Poem>> Handle(GetAllPoemsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllPoems();
    }
}
