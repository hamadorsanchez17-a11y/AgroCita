import type { Crop } from "../../domain/entities/Crop.js";
import type { CropRepository } from "../../domain/repositories/CropRepository.js";

export class MockCropRepository implements CropRepository {
  private crops: Crop[] = [
    { id: 1, fincaId: 1, nombre: "Café" },
    { id: 2, fincaId: 1, nombre: "Maíz" },
    { id: 3, fincaId: 2, nombre: "Frijol" },
  ];

  private nextId = 4;

  async create(crop: Omit<Crop, "id">): Promise<Crop> {
    const newCrop: Crop = {
      id: this.nextId++,
      ...crop,
    };

    this.crops.push(newCrop);
    return newCrop;
  }

  async findAllByFarmId(fincaId: number): Promise<Crop[]> {
    return this.crops.filter((crop) => crop.fincaId === fincaId);
  }

  async findByIdAndFarmId(
    cropId: number,
    fincaId: number
  ): Promise<Crop | null> {
    return (
      this.crops.find(
        (crop) => crop.id === cropId && crop.fincaId === fincaId
      ) ?? null
    );
  }
}