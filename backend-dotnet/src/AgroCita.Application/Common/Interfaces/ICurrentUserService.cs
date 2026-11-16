namespace AgroCita.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UsuarioId { get; }

    string? Rol { get; }

    int? EmpresaId { get; }
}
