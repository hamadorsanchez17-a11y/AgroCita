import type { Farm } from "../../domain/entities/Farm.js";

import type {
  FarmRepository,
  UpdateFarmData
} from "../../domain/repositories/FarmRepository.js";

export class UpdateFarm {
  constructor(private readonly farmRepository: FarmRepository) {}

  async execute(
    farmId: number,
    productorId: number,
    input: UpdateFarmData
  ): Promise<Farm> {

    const existingFarm = await this.farmRepository.findByIdAndProducerId(
      farmId,
      productorId
    );

    if (!existingFarm) {
      throw new Error("No se encontró la finca solicitada.");
    }

    if (input.nombre !== undefined) {
      const farms = await this.farmRepository.findAllByProducerId(productorId);

      const duplicateName = farms.some(
        farm =>
          farm.id !== farmId &&
          farm.nombre.toLowerCase() === input.nombre!.toLowerCase()
      );

      if (duplicateName) {
        throw new Error("Ya tienes registrada otra finca con ese nombre.");
      }
    }

    const updatedFarm = await this.farmRepository.update(
      farmId,
      productorId,
      input
    );

    if (!updatedFarm) {
      throw new Error("No se pudo actualizar la finca.");
    }

    return updatedFarm;
  }
}