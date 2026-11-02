
import bcrypt from "bcrypt";

import type { AuthRepository } from "../../domain/repositories/AuthRepository.js";
import type { RegisterProducerDTO } from "../dto/RegisterProducerDTO.js";

export class RegisterProducer {
  constructor(
    private readonly authRepository: AuthRepository
  ) {}

  async execute(
    input: RegisterProducerDTO
  ): Promise<{ userId: number; empresaId: number }> {

    // 1. Buscar la empresa mediante el código de invitación.
    const company =
      await this.authRepository.findCompanyByInvitationCode(
        input.codigoInvitacion
      );

    if (!company || !company.activa) {
      throw new Error(
        "El código de invitación no es válido o la empresa está inactiva."
      );
    }

    // 2. Comprobar que el correo no esté registrado.
    const emailExists =
      await this.authRepository.findUserByEmail(input.email);

    if (emailExists) {
      throw new Error(
        "El correo electrónico ya está registrado."
      );
    }

    // 3. Comprobar que el DNI no esté registrado en esa empresa.
    const dniExists =
      await this.authRepository.findProducerByDni(
        company.id,
        input.dni
      );

    if (dniExists) {
      throw new Error(
        "Ya existe un productor con ese DNI en la empresa."
      );
    }

    // 4. Generar el hash de la contraseña.
    const passwordHash = await bcrypt.hash(input.password, 12);

    // 5. Preparar los datos para crear la cuenta.
    const producerData = {
      empresaId: company.id,
      nombreCompleto: input.nombreCompleto,
      email: input.email,
      passwordHash,
      dni: input.dni,
      telefono: input.telefono,
      ...(input.direccion !== undefined
        ? { direccion: input.direccion }
        : {})
    };

    // 6. Crear el usuario y su perfil de productor.
    const userId =
      await this.authRepository.createProducerAccount(
        producerData
      );

    return {
      userId,
      empresaId: company.id
    };
  }
}
