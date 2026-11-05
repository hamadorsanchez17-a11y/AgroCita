namespace AgroCita.Domain.Entities;

public class AssistanceRequestCrop
{
    public int SolicitudId { get; set; }

    public int CultivoId { get; set; }

    public AssistanceRequest? Solicitud { get; set; }

    public Crop? Cultivo { get; set; }
}