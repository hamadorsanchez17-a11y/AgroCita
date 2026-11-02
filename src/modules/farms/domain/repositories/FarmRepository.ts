import type { Farm } from "../entities/Farm.js";

export interface CreateFarmData {
  productorId: number;
  nombre: string;
  departamento: string;
  municipio: string;
  ubicacion: string;
  areaManzanas: number;
  descripcion?: string | undefined;
}

export interface UpdateFarmData {
  nombre?: string | undefined;
  departamento?: string | undefined;
  municipio?: string | undefined;
  ubicacion?: string | undefined;
  areaManzanas?: number | undefined;
  descripcion?: string | undefined;
}

export interface FarmRepository {

  create(data: CreateFarmData): Promise<Farm>;

  findAllByProducerId(
    productorId: number
  ): Promise<Farm[]>;

  findByIdAndProducerId(
    farmId: number,
    productorId: number
  ): Promise<Farm | null>;

  update(
    farmId: number,
    productorId: number,
    data: UpdateFarmData
  ): Promise<Farm | null>;
}