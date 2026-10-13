using Application.Abstractions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppLogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, IAppLogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Regla de negocio incumplida", ex.Message);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // El cliente cancelo la peticion y no es un error del servidor.
        }
        catch (Exception ex)
        {
            _logger.Error("Unhandled exception", ex);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Error interno", "Ocurrio un error inesperado.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        var problem = new ProblemDetails { Status = status, Title = title, Detail = detail };
        return context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
    }
}
