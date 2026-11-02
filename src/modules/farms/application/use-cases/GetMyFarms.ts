import type { FarmRepository } from "../../domain/repositories/FarmRepository.js";
import type { Farm } from "../../domain/entities/Farm.js";

export class GetMyFarms {
  constructor(
    private readonly farmRepository: FarmRepository
  ) {}

  async execute(productorId: number): Promise<Farm[]> {
    return await this.farmRepository.findAllByProducerId(
      productorId
    );
  }
}