using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions;

///  solo lectura de pedidos.
public interface IOrderReader
{
    Task<IReadOnlyList<Order>> GetLastAsync(int count, CancellationToken cancellationToken);
}
