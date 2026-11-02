import type { ProducerRepository } from "../../domain/repositories/ProducerRepository.js";
import type { ProducerProfileDTO } from "../dto/ProducerProfileDTO.js";

export class GetMyProducerProfile {
  constructor(
    private readonly producerRepository: ProducerRepository
  ) {}

  async execute(
    userId: number,
    empresaId: number | null
  ): Promise<ProducerProfileDTO> {

    // Un productor debe pertenecer a una empresa.
    if (empresaId === null) {
      throw new Error(
        "El usuario no pertenece a una empresa."
      );
    }

    // Buscar el perfil usando el usuario y su empresa.
    const producer =
      await this.producerRepository.findProfileByUserId(
        userId,
        empresaId
      );

    if (!producer) {
      throw new Error(
        "No se encontró el perfil del productor."
      );
    }

    return {
      id: producer.id,
      usuarioId: producer.usuarioId,
      empresaId: producer.empresaId,
      nombreCompleto: producer.nombreCompleto,
      email: producer.email,
      dni: producer.dni,
      telefono: producer.telefono,
      direccion: producer.direccion
    };
  }
}