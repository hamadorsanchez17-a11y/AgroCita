import type { Farm } from "../../domain/entities/Farm.js";

import type {
  FarmRepository,
  CreateFarmData,
  UpdateFarmData
} from "../../domain/repositories/FarmRepository.js";

export class MockFarmRepository implements FarmRepository {

  private farms: Farm[] = [];

  private nextFarmId = 1;

  async create(data: CreateFarmData): Promise<Farm> {
    const farm: Farm = {
      id: this.nextFarmId++,
      productorId: data.productorId,
      nombre: data.nombre,
      departamento: data.departamento,
      municipio: data.municipio,
      ubicacion: data.ubicacion,
      areaManzanas: data.areaManzanas,
      descripcion: data.descripcion ?? null
    };

    this.farms.push(farm);

    return farm;
  }

  async findAllByProducerId(
    productorId: number
  ): Promise<Farm[]> {
    return this.farms.filter(
      farm => farm.productorId === productorId
    );
  }

  async findByIdAndProducerId(
    farmId: number,
    productorId: number
  ): Promise<Farm | null> {
    const farm = this.farms.find(
      item =>
        item.id === farmId &&
        item.productorId === productorId
    );

    return farm ?? null;
  }

  async update(
    farmId: number,
    productorId: number,
    data: UpdateFarmData
  ): Promise<Farm | null> {
    const farm = await this.findByIdAndProducerId(farmId, productorId);

    if (!farm) return null;

    Object.assign(farm, data);

    return farm;
  }
}