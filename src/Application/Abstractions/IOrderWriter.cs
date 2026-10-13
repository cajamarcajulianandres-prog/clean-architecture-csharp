using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions;

/// escritura de pedidos.
public interface IOrderWriter
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
}
