import type { Crop } from "../../domain/entities/Crop.js";
import type { CropRepository } from "../../domain/repositories/CropRepository.js";

export class CreateCrop {
  constructor(private readonly cropRepository: CropRepository) {}

  async execute(fincaId: number, nombre: string): Promise<Crop> {
    if (!nombre || nombre.trim().length === 0) {
      throw new Error("El nombre del cultivo es obligatorio");
    }

    if (fincaId <= 0) {
      throw new Error("La finca no es válida");
    }

    const crop: Omit<Crop, "id"> = {
      fincaId,
      nombre: nombre.trim(),
    };

    return await this.cropRepository.create(crop);
  }
}