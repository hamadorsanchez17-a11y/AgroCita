import type { VisitRepository } from "../../domain/repositories/VisitRepository.js";
import type { Visit } from "../../domain/entities/Visit.js";

export class GetVisitsByTechnician {
  constructor(
    private readonly visitRepository: VisitRepository
  ) {}

  async execute(
    tecnicoId: number,
    empresaId: number
  ): Promise<Visit[]> {
    if (tecnicoId <= 0 || empresaId <= 0) {
      throw new Error("Los identificadores deben ser positivos.");
    }

    return this.visitRepository.findAllByTechnician(
      tecnicoId,
      empresaId
    );
  }
}