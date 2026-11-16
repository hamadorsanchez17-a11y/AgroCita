using AgroCita.Domain.Constants;

namespace AgroCita.API.Controllers;

public static class Permisos
{
    public const string SuperAdmin = Roles.SuperAdmin;
    public const string Admin = Roles.AdminEmpresa;
    public const string Tecnico = Roles.Tecnico;
    public const string Productor = Roles.Productor;
    public const string AdminOTecnico = Admin + "," + Tecnico;
    public const string AdminOProductor = Admin + "," + Productor;
    public const string Empresa = Admin + "," + Tecnico + "," + Productor;
}
