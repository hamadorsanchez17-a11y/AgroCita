namespace AgroCita.Application.Features.Auth;

public record UsuarioActualDto(
    int Id,
    string Nombre,
    string Email,
    string Rol,
    int? EmpresaId,
    string? EmpresaNombre);

public record AuthResponseDto(
    string Token,
    int ExpiraEn,
    UsuarioActualDto Usuario);
