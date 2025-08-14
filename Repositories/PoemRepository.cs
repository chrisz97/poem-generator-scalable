using Microsoft.EntityFrameworkCore;
using PoemGenerator.Monolith.Data;
using PoemGenerator.Monolith.Data.Entities;

namespace PoemGenerator.Monolith.Repositories;

public class PoemRepository : IPoemRepository
{
    private readonly PoemDbContext _context;
    private readonly ILogger<PoemRepository> _logger;

    public PoemRepository(PoemDbContext context, ILogger<PoemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AuthorEntity?> GetAuthorByNameAsync(string name)
    {
        var author = await _context.Set<AuthorEntity>().FirstOrDefaultAsync(a => a.Name == name);
        if (author == null)
        {
            _logger.LogWarning("Author with name {Name} not found in database", name);
        }

        return author;
    }

    public async Task<PoemEntity> CreateAsync(PoemEntity poem)
    {
        _context.Poems.Add(poem);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created poem with ID {Id} in database", poem.Id);
        return poem;
    }

    public async Task<PoemEntity?> GetByIdAsync(int id)
    {
        var poem = await _context.Poems
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (poem != null)
        {
            _logger.LogInformation("Retrieved poem with ID {Id} from database", id);
        }
        else
        {
            _logger.LogWarning("Poem with ID {Id} not found in database", id);
        }
        return poem;
    }

    public async Task<IEnumerable<PoemEntity>> GetAllAsync()
    {
        var poems = await _context.Poems
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        _logger.LogInformation("Retrieved {Count} poems from database", poems.Count);
        return poems;
    }
}