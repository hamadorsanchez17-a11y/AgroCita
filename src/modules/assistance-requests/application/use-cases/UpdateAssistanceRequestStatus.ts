import type { AssistanceRequest } from "../../domain/entities/AssistanceRequest.js";
import type { AssistanceRequestRepository } from "../../domain/repositories/AssistanceRequestRepository.js";

export class UpdateAssistanceRequestStatus {
  constructor(
    private readonly requestRepository: AssistanceRequestRepository
  ) {}

  async execute(
    id: number,
    empresaId: number,
    estado: AssistanceRequest["estado"]
  ): Promise<AssistanceRequest> {
    if (id <= 0) {
      throw new Error("El identificador de la solicitud no es válido.");
    }

    if (empresaId <= 0) {
      throw new Error("La empresa no es válida.");
    }

    const estadosPermitidos = [
      "PENDIENTE",
      "ACEPTADA",
      "RECHAZADA",
      "CANCELADA",
    ];

    if (!estadosPermitidos.includes(estado)) {
      throw new Error("El estado de la solicitud no es válido.");
    }

    const updatedRequest = await this.requestRepository.updateStatus(
      id,
      empresaId,
      estado
    );

    if (!updatedRequest) {
      throw new Error("No se encontró la solicitud solicitada.");
    }

    return updatedRequest;
  }
}