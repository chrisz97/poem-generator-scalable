using MediatR;
using PoemGenerator.Models;
using PoemGenerator.Monolith.Cqrs.Commands;
using PoemGenerator.Monolith.Notifications;
using PoemGenerator.Monolith.Services;

namespace PoemGenerator.Monolith.Cqrs.Handlers;

public class CreatePoemHandler : IRequestHandler<CreatePoemCommand, Poem>
{
    private readonly IPoemService _service;
    private readonly INotificationService _notifier;

    public CreatePoemHandler(IPoemService service, INotificationService notifier)
    {
        _service = service;
        _notifier = notifier;
    }

    public async Task<Poem> Handle(CreatePoemCommand request, CancellationToken cancellationToken)
    {
        var poem = await _service.CreatePoem(new CreatePoemRequest { Length = request.Length });
        await _notifier.NotifyPoemAddedAsync(poem.Id, poem.Title, poem.Author);
        return poem;
    }
}
