
import type { Visit } from "../entities/Visit.js";

export interface VisitRepository {
  create(
    visit: Omit<Visit, "id" | "createdAt" | "updatedAt" | "estado">
  ): Promise<Visit>;

  findById(
    id: number,
    empresaId: number
  ): Promise<Visit | null>;

  findAllByTechnician(
    tecnicoId: number,
    empresaId: number
  ): Promise<Visit[]>;

  findAllByCompany(
    empresaId: number
  ): Promise<Visit[]>;

  update(
    id: number,
    empresaId: number,
    data: Partial<
      Pick<
        Visit,
        "tecnicoId" | "fechaProgramada" | "horaProgramada" | "estado"
      >
    >
  ): Promise<Visit | null>;
}
