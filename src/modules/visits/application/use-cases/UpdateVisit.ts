
import type { VisitRepository } from "../../domain/repositories/VisitRepository.js";
import type { Visit } from "../../domain/entities/Visit.js";

interface UpdateVisitDTO {
  id: number;
  empresaId: number;
  tecnicoId?: number;
  fechaProgramada?: string;
  horaProgramada?: string;
  estado?: Visit["estado"];
}

export class UpdateVisit {
  constructor(
    private readonly visitRepository: VisitRepository
  ) {}

  async execute(data: UpdateVisitDTO): Promise<Visit | null> {
    if (data.id <= 0 || data.empresaId <= 0) {
      throw new Error("Los identificadores deben ser positivos.");
    }

    if (data.tecnicoId !== undefined && data.tecnicoId <= 0) {
      throw new Error("El ID del técnico debe ser positivo.");
    }

    if (data.fechaProgramada !== undefined) {
      const fecha = new Date(`${data.fechaProgramada}T00:00:00Z`);

      if (
        !/^\d{4}-\d{2}-\d{2}$/.test(data.fechaProgramada) ||
        Number.isNaN(fecha.getTime()) ||
        fecha.toISOString().slice(0, 10) !== data.fechaProgramada
      ) {
        throw new Error("La fecha debe ser válida y tener el formato YYYY-MM-DD.");
      }
    }

    if (
      data.horaProgramada !== undefined &&
      !/^([01]\d|2[0-3]):[0-5]\d$/.test(data.horaProgramada)
    ) {
      throw new Error("La hora debe tener el formato HH:mm.");
    }

    const estadosPermitidos: Visit["estado"][] = [
      "PENDIENTE",
      "CONFIRMADA",
      "EN_PROCESO",
      "COMPLETADA",
      "CANCELADA"
    ];

    if (
      data.estado !== undefined &&
      !estadosPermitidos.includes(data.estado)
    ) {
      throw new Error("El estado de la visita no es válido.");
    }

    const hasChanges =
      data.tecnicoId !== undefined ||
      data.fechaProgramada !== undefined ||
      data.horaProgramada !== undefined ||
      data.estado !== undefined;

    if (!hasChanges) {
      throw new Error("Debe proporcionar al menos un campo para actualizar.");
    }

    const { id, empresaId, ...changes } = data;

    return this.visitRepository.update(id, empresaId, changes);
  }
}
