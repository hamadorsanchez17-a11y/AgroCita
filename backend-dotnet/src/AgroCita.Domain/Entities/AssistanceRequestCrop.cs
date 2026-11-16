using AgroCita.Domain.Common;

namespace AgroCita.Domain.Entities;

public class AssistanceRequestCrop : ITenantEntity
{
    public int SolicitudId { get; set; }

    public int CultivoId { get; set; }

    public int EmpresaId { get; set; }

    public AssistanceRequest? Solicitud { get; set; }

    public Crop? Cultivo { get; set; }
}
