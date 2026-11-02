import type { Producer } from "../../domain/entities/Producer.js";

import type {
  ProducerRepository
} from "../../domain/repositories/ProducerRepository.js";

export class MockProducerRepository implements ProducerRepository {

  private producers: Producer[] = [
    {
      id: 1,
      empresaId: 1,
      usuarioId: 1,
      nombreCompleto: "Juan Pérez",
      email: "juan@example.com",
      dni: "0801199012345",
      telefono: "99998888",
      direccion: "Tegucigalpa, Honduras"
    }
  ];

  async findProfileByUserId(
    userId: number,
    empresaId: number
  ): Promise<Producer | null> {

    const producer = this.producers.find(
      item =>
        item.usuarioId === userId &&
        item.empresaId === empresaId
    );

    return producer ?? null;
  }
}