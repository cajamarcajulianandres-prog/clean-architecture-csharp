using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence;

/// <summary>Repositorio SQL Server: consultas parametrizadas y conexiones liberadas con using.</summary>
public sealed class SqlOrderRepository : IOrderWriter, IOrderReader
{
    private const string InsertSql =
        "INSERT INTO Orders (Id, Customer, Product, Qty, Price) VALUES (@Id, @Customer, @Product, @Qty, @Price)";

    private const string SelectLastSql =
        "SELECT TOP (@Count) Id, Customer, Product, Qty, Price FROM Orders ORDER BY CreatedAtUtc DESC";

    private readonly string _connectionString;

    public SqlOrderRepository(SqlServerOptions options)
    {
        _connectionString = options.ConnectionString;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(InsertSql, connection);

        command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = order.Id;
        command.Parameters.Add("@Customer", SqlDbType.NVarChar, 200).Value = order.CustomerName;
        command.Parameters.Add("@Product", SqlDbType.NVarChar, 200).Value = order.ProductName;
        command.Parameters.Add("@Qty", SqlDbType.Int).Value = order.Quantity;
        command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal)
        {
            Precision = 18,
            Scale = 2,
            Value = order.UnitPrice
        });

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetLastAsync(int count, CancellationToken cancellationToken)
    {
        var orders = new List<Order>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(SelectLastSql, connection);
        command.Parameters.Add("@Count", SqlDbType.Int).Value = count;

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            orders.Add(Order.Restore(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetDecimal(4)));
        }

        return orders;
    }
}
