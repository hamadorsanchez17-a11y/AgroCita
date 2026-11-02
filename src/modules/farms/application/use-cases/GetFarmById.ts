import type { FarmRepository } from "../../domain/repositories/FarmRepository.js";
import type { Farm } from "../../domain/entities/Farm.js";

export class GetFarmById {
  constructor(private readonly farmRepository: FarmRepository) {}

  async execute(farmId: number, productorId: number): Promise<Farm> {
    const farm = await this.farmRepository.findByIdAndProducerId(
      farmId,
      productorId
    );

    if (!farm) {
      throw new Error("No se encontró la finca solicitada.");
    }

    return farm;
  }
}