import type { AssistanceRequest } from "../../domain/entities/AssistanceRequest.js";
import type { AssistanceRequestRepository } from "../../domain/repositories/AssistanceRequestRepository.js";

export class GetAssistanceRequestsByCompany {
  constructor(
    private readonly requestRepository: AssistanceRequestRepository
  ) {}

  async execute(empresaId: number): Promise<AssistanceRequest[]> {
    if (empresaId <= 0) {
      throw new Error("La empresa no es válida.");
    }

    return await this.requestRepository.findAllByCompany(empresaId);
  }
}