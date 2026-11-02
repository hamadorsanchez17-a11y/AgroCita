import type { Crop } from "../entities/Crop.js";

export interface CropRepository {
  create(crop: Omit<Crop, "id">): Promise<Crop>;

  findAllByFarmId(fincaId: number): Promise<Crop[]>;

  findByIdAndFarmId(
    cropId: number,
    fincaId: number
  ): Promise<Crop | null>;
}