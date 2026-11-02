import type { AssistanceRequest } from "../../domain/entities/AssistanceRequest.js";
import type { AssistanceRequestRepository } from "../../domain/repositories/AssistanceRequestRepository.js";
import type { CropRepository } from "../../../crops/domain/repositories/CropRepository.js";

export class CreateAssistanceRequest {
  constructor(
    private readonly requestRepository: AssistanceRequestRepository,
    private readonly cropRepository: CropRepository
  ) {}

  async execute(
    data: Omit<AssistanceRequest, "id" | "createdAt" | "estado">
  ): Promise<AssistanceRequest> {
    if (data.fincaId <= 0) {
      throw new Error("La finca no es válida.");
    }

    if (!data.cultivoIds || data.cultivoIds.length === 0) {
      throw new Error("Debe seleccionar al menos un cultivo.");
    }

    if (new Set(data.cultivoIds).size !== data.cultivoIds.length) {
      throw new Error("No puede seleccionar cultivos repetidos.");
    }

    if (!data.fechaDeseada) {
      throw new Error("La fecha deseada es obligatoria.");
    }

    if (!data.motivo || data.motivo.trim().length < 5) {
      throw new Error("El motivo debe tener al menos 5 caracteres.");
    }

    for (const cultivoId of data.cultivoIds) {
      const crop = await this.cropRepository.findByIdAndFarmId(
        cultivoId,
        data.fincaId
      );

      if (!crop) {
        throw new Error(
          `El cultivo con ID ${cultivoId} no pertenece a la finca seleccionada.`
        );
      }
    }

    return await this.requestRepository.create({
      ...data,
      motivo: data.motivo.trim(),
    });
  }
}