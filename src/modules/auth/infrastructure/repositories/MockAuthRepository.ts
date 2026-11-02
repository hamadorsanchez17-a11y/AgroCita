
import type { Company } from "../../domain/entities/Company.js";
import type { AuthUser } from "../../domain/entities/AuthUser.js";

import type {
  AuthRepository,
  RegisterProducerData
} from "../../domain/repositories/AuthRepository.js";

interface MockUser {
  id: number;
  empresaId: number;
  nombreCompleto: string;
  email: string;
  passwordHash: string;
  rol: string;
  activo: boolean
}

interface MockProducer {
  userId: number;
  empresaId: number;
  dni: string;
  telefono: string;
  direccion?: string;
}

export class MockAuthRepository implements AuthRepository {

  // Empresas simuladas
  private companies: Array<Company & {
    codigoInvitacion: string;
  }> = [
    {
      id: 1,
      activa: true,
      codigoInvitacion: "AGROTEST0001"
    },
    {
      id: 2,
      activa: false,
      codigoInvitacion: "AGROTEST0002"
    }
  ];

  async findUserCredentialsByEmail(
  email: string
): Promise<AuthUser | null> {

  const user = this.users.find(
    item => item.email.toLowerCase() === email.toLowerCase()
  );

  if (!user) {
    return null;
  }

  return {
    id: user.id,
    empresaId: user.empresaId,
    nombreCompleto: user.nombreCompleto,
    email: user.email,
    passwordHash: user.passwordHash,
    rol: user.rol,
    activo: user.activo
  };
}

  // Usuarios registrados durante la ejecución
  private users: MockUser[] = [];

  // Perfiles de productores registrados
  private producers: MockProducer[] = [];

  private nextUserId = 1;

  async findCompanyByInvitationCode(
    codigoInvitacion: string
  ): Promise<Company | null> {

    const company = this.companies.find(
      item => item.codigoInvitacion === codigoInvitacion
    );

    if (!company) {
      return null;
    }

    return {
      id: company.id,
      activa: company.activa
    };
  }

  async findUserByEmail(
    email: string
  ): Promise<boolean> {

    return this.users.some(
      user => user.email.toLowerCase() === email.toLowerCase()
    );
  }

  async findProducerByDni(
    empresaId: number,
    dni: string
  ): Promise<boolean> {

    return this.producers.some(
      producer =>
        producer.empresaId === empresaId &&
        producer.dni === dni
    );
  }

  async createProducerAccount(
    data: RegisterProducerData
  ): Promise<number> {

    const userId = this.nextUserId++;

    const user: MockUser = {
      id: userId,
      empresaId: data.empresaId,
      nombreCompleto: data.nombreCompleto,
      email: data.email,
      passwordHash: data.passwordHash,
      rol: "PRODUCTOR",
      activo: true
    };

    const producer: MockProducer = {
      userId,
      empresaId: data.empresaId,
      dni: data.dni,
      telefono: data.telefono,
      ...(data.direccion !== undefined
        ? { direccion: data.direccion }
        : {})
    };

    this.users.push(user);
    this.producers.push(producer);

    return userId;
  }
}
