import type { VisitRepository } from "../../domain/repositories/VisitRepository.js";
import type { Visit } from "../../domain/entities/Visit.js";

export class GetVisitsByCompany {
  constructor(
    private readonly visitRepository: VisitRepository
  ) {}

  async execute(empresaId: number): Promise<Visit[]> {
    if (empresaId <= 0) {
      throw new Error("El ID de la empresa debe ser positivo.");
    }

    return this.visitRepository.findAllByCompany(empresaId);
  }
}