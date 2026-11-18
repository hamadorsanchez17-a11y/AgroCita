
namespace AgroCita.Domain.Entities;

public class ReschedulingVisit
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int VisitaId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaAnterior { get; set; }
    public DateTime FechaNueva { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaReprogramacion { get; set; }

    public Company? Empresa { get; set; }
    public Visit? Visita { get; set; }
    public User? Usuario { get; set; }
}
