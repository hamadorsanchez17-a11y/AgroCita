
namespace AgroCita.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? EmpresaId { get; }
    bool EsSuperAdmin { get; }
}
