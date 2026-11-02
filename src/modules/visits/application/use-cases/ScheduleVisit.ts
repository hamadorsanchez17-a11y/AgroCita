
import type { VisitRepository } from "../../domain/repositories/VisitRepository.js";
import type { Visit } from "../../domain/entities/Visit.js";
import type { AssistanceRequestRepository } from "../../../assistance-requests/domain/repositories/AssistanceRequestRepository.js";

interface ScheduleVisitDTO {
  empresaId: number;
  solicitudId: number;
  tecnicoId: number;
  fincaId: number;
  fechaProgramada: string;
  horaProgramada: string;
}

export class ScheduleVisit {
  constructor(
    private readonly visitRepository: VisitRepository,
    private readonly assistanceRequestRepository: AssistanceRequestRepository
  ) {}

  async execute(data: ScheduleVisitDTO): Promise<Visit> {
    if (
      data.empresaId <= 0 ||
      data.solicitudId <= 0 ||
      data.tecnicoId <= 0 ||
      data.fincaId <= 0
    ) {
      throw new Error("Los identificadores deben ser positivos.");
    }

    const fecha = new Date(`${data.fechaProgramada}T00:00:00Z`);

    if (
      !/^\d{4}-\d{2}-\d{2}$/.test(data.fechaProgramada) ||
      Number.isNaN(fecha.getTime()) ||
      fecha.toISOString().slice(0, 10) !== data.fechaProgramada
    ) {
      throw new Error("La fecha debe ser válida y tener el formato YYYY-MM-DD.");
    }

    if (!/^([01]\d|2[0-3]):[0-5]\d$/.test(data.horaProgramada)) {
      throw new Error("La hora debe tener el formato HH:mm.");
    }

    const request = await this.assistanceRequestRepository.findById(
      data.solicitudId,
      data.empresaId
    );

    if (!request) {
      throw new Error("No se encontró la solicitud de asistencia.");
    }

    if (request.estado !== "ACEPTADA") {
      throw new Error(
        "Solo se pueden programar visitas para solicitudes aceptadas."
      );
    }

    if (request.fincaId !== data.fincaId) {
      throw new Error(
        "La finca no corresponde a la solicitud de asistencia."
      );
    }

    return this.visitRepository.create(data);
  }
}
