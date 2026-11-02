import type { AssistanceRequest } from "../entities/AssistanceRequest.js";

export interface AssistanceRequestRepository {
  create(
    request: Omit<AssistanceRequest, "id" | "createdAt" | "estado">
  ): Promise<AssistanceRequest>;

  findById(id: number, empresaId: number): Promise<AssistanceRequest | null>;

  findAllByProducer(
    productorId: number,
    empresaId: number
  ): Promise<AssistanceRequest[]>;

  findAllByCompany(empresaId: number): Promise<AssistanceRequest[]>;

  updateStatus(
    id: number,
    empresaId: number,
    estado: AssistanceRequest["estado"]
  ): Promise<AssistanceRequest | null>;
}