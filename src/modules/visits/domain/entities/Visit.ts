
export type VisitStatus =
  | "PENDIENTE"
  | "CONFIRMADA"
  | "EN_PROCESO"
  | "COMPLETADA"
  | "CANCELADA";

export interface Visit {
  id: number;
  empresaId: number;
  solicitudId: number;
  tecnicoId: number;
  fincaId: number;
  fechaProgramada: string;
  horaProgramada: string;
  estado: VisitStatus;
  createdAt: string;
  updatedAt: string;
}
