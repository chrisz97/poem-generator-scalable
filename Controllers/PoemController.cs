using Microsoft.AspNetCore.Mvc;
using PoemGenerator.Monolith.Services;
using PoemGenerator.Models;
using Microsoft.Extensions.Caching.Memory;

namespace PoemGenerator.Monolith.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoemController(IPoemService poemService, ILogger<PoemController> logger, IMemoryCache memoryCache) : ControllerBase
{
    private readonly IPoemService _poemService = poemService;
    private readonly ILogger<PoemController> _logger = logger;

    [HttpPost]
    public Poem CreatePoem([FromBody] CreatePoemRequest request)
    {
        _logger.LogInformation("Received a poem create request with length: {Length}", request.Length);

        var poem = _poemService.CreatePoem(request).Result;

        memoryCache.Set(poem.Id, poem, TimeSpan.FromMinutes(5));
        memoryCache.Remove("allPoems");

        return poem;
    }

    [HttpGet("{id}")]
    public ActionResult<Poem> GetPoem(int id)
    {
        if (memoryCache.TryGetValue(id, out var cachedPoem))
        {
            _logger.LogInformation("Returning cached poem with ID {Id}", id);
            return Ok(cachedPoem);
        }

        try
        {
            var poem = _poemService.GetPoem(id).Result;
            memoryCache.Set(id, poem, TimeSpan.FromMinutes(5));
            return Ok(poem);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Poem with ID {id} not found.");
        }
    }

    [HttpGet]
    public IEnumerable<Poem> GetAllPoems()
    {
        if (memoryCache.TryGetValue<IEnumerable<Poem>>("allPoems", out var cachedPoems) && cachedPoems != null)
        {
            _logger.LogInformation("Returning cached poems.");
            return cachedPoems;
        }

        var poems = _poemService.GetAllPoems().Result;

        memoryCache.Set("allPoems", poems, TimeSpan.FromMinutes(5));

        return poems;
    }
}