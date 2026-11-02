export type AssistanceRequestStatus =
  | "PENDIENTE"
  | "ACEPTADA"
  | "RECHAZADA"
  | "CANCELADA";

export interface AssistanceRequest {
  id: number;
  empresaId: number;
  productorId: number;
  fincaId: number;
  cultivoIds: number[];
  fechaDeseada: string;
  motivo: string;
  estado: AssistanceRequestStatus;
  createdAt: string;
}