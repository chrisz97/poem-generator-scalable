using MediatR;
using PoemGenerator.Models;

namespace PoemGenerator.Monolith.Cqrs.Queries;

public record GetAllPoemsQuery : IRequest<IEnumerable<Poem>>;
