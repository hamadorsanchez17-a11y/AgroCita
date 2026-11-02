
import type { Visit } from "../../domain/entities/Visit.js";
import type { VisitRepository } from "../../domain/repositories/VisitRepository.js";

export class MockVisitRepository implements VisitRepository {
  private visits: Visit[] = [];
  private nextId = 1;

  async create(
    data: Omit<Visit, "id" | "createdAt" | "updatedAt" | "estado">
  ): Promise<Visit> {
    const now = new Date().toISOString();

    const visit: Visit = {
      id: this.nextId++,
      ...data,
      estado: "PENDIENTE",
      createdAt: now,
      updatedAt: now,
    };

    this.visits.push(visit);

    return visit;
  }

  async findById(
    id: number,
    empresaId: number
  ): Promise<Visit | null> {
    return (
      this.visits.find(
        (visit) => visit.id === id && visit.empresaId === empresaId
      ) ?? null
    );
  }

  async findAllByTechnician(
    tecnicoId: number,
    empresaId: number
  ): Promise<Visit[]> {
    return this.visits.filter(
      (visit) =>
        visit.tecnicoId === tecnicoId &&
        visit.empresaId === empresaId
    );
  }

  async findAllByCompany(empresaId: number): Promise<Visit[]> {
    return this.visits.filter(
      (visit) => visit.empresaId === empresaId
    );
  }

  async update(
    id: number,
    empresaId: number,
    data: Partial<
      Pick<Visit, "tecnicoId" | "fechaProgramada" | "horaProgramada" | "estado">
    >
  ): Promise<Visit | null> {
    const visit = await this.findById(id, empresaId);

    if (!visit) {
      return null;
    }

    Object.assign(visit, data);
    visit.updatedAt = new Date().toISOString();

    return visit;
  }
}
