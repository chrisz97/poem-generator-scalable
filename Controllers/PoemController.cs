using Microsoft.AspNetCore.Mvc;
using PoemGenerator.Models;
using Microsoft.Extensions.Caching.Memory;
using MediatR;
using PoemGenerator.Monolith.Cqrs.Commands;
using PoemGenerator.Monolith.Cqrs.Queries;

namespace PoemGenerator.Monolith.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoemController(IMediator mediator, IMemoryCache memoryCache) : ControllerBase
{
    [HttpPost]
    public async Task<Poem> CreatePoemAsync([FromBody] CreatePoemRequest request)
    {
        var poem = await mediator.Send(new CreatePoemCommand(request.Length));
        memoryCache.Set(poem.Id, poem, TimeSpan.FromMinutes(5));
        memoryCache.Remove("allPoems");
        return poem;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Poem>> GetPoem(int id)
    {
        if (memoryCache.TryGetValue(id, out Poem? poem))
        {
            return Ok(poem);
        }

        poem = await mediator.Send(new GetPoemByIdQuery(id));
        memoryCache.Set(poem.Id, poem, TimeSpan.FromMinutes(5));
        return Ok(poem);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Poem>>> GetAllPoems()
    {
        if (memoryCache.TryGetValue("allPoems", out IEnumerable<Poem>? poems))
        {
            return Ok(poems);
        }

        poems = await mediator.Send(new GetAllPoemsQuery());
        memoryCache.Set("allPoems", poems, TimeSpan.FromMinutes(5));
        return Ok(poems);
    }
}