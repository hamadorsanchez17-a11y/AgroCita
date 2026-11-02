import type { Crop } from "../../domain/entities/Crop.js";
import type { CropRepository } from "../../domain/repositories/CropRepository.js";

export class GetCropsByFarm {
  constructor(private readonly cropRepository: CropRepository) {}

  async execute(fincaId: number): Promise<Crop[]> {
    if (fincaId <= 0) {
      throw new Error("La finca no es válida");
    }

    return await this.cropRepository.findAllByFarmId(fincaId);
  }
}