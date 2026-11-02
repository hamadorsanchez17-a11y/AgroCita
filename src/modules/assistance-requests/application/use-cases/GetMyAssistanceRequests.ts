import type { AssistanceRequest } from "../../domain/entities/AssistanceRequest.js";
import type { AssistanceRequestRepository } from "../../domain/repositories/AssistanceRequestRepository.js";

export class GetMyAssistanceRequests {
  constructor(
    private readonly requestRepository: AssistanceRequestRepository
  ) {}

  async execute(
    productorId: number,
    empresaId: number
  ): Promise<AssistanceRequest[]> {
    if (productorId <= 0) {
      throw new Error("El productor no es válido.");
    }

    if (empresaId <= 0) {
      throw new Error("La empresa no es válida.");
    }

    return await this.requestRepository.findAllByProducer(
      productorId,
      empresaId
    );
  }
}