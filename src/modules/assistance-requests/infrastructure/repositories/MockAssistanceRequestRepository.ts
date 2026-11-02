import type { AssistanceRequest } from "../../domain/entities/AssistanceRequest.js";
import type { AssistanceRequestRepository } from "../../domain/repositories/AssistanceRequestRepository.js";

export class MockAssistanceRequestRepository
  implements AssistanceRequestRepository
{
  private requests: AssistanceRequest[] = [];
  private nextId = 1;

  async create(
    request: Omit<AssistanceRequest, "id" | "createdAt" | "estado">
  ): Promise<AssistanceRequest> {
    const newRequest: AssistanceRequest = {
      id: this.nextId++,
      ...request,
      estado: "PENDIENTE",
      createdAt: new Date().toISOString(),
    };

    this.requests.push(newRequest);
    return newRequest;
  }

  async findById(
    id: number,
    empresaId: number
  ): Promise<AssistanceRequest | null> {
    return (
      this.requests.find(
        (request) => request.id === id && request.empresaId === empresaId
      ) ?? null
    );
  }

  async findAllByProducer(
    productorId: number,
    empresaId: number
  ): Promise<AssistanceRequest[]> {
    return this.requests.filter(
      (request) =>
        request.productorId === productorId &&
        request.empresaId === empresaId
    );
  }

  async findAllByCompany(empresaId: number): Promise<AssistanceRequest[]> {
    return this.requests.filter(
      (request) => request.empresaId === empresaId
    );
  }

  async updateStatus(
    id: number,
    empresaId: number,
    estado: AssistanceRequest["estado"]
  ): Promise<AssistanceRequest | null> {
    const request = await this.findById(id, empresaId);

    if (!request) {
      return null;
    }

    request.estado = estado;
    return request;
  }
}