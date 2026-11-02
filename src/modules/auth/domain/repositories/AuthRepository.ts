
import type { Company } from "../entities/Company.js";
import type {AuthUser} from "../entities/AuthUser.js";

export interface RegisterProducerData {
  empresaId: number;
  nombreCompleto: string;
  email: string;
  passwordHash: string;
  dni: string;
  telefono: string;
  direccion?: string;
}

export interface AuthRepository {
  findCompanyByInvitationCode(
    codigoInvitacion: string
  ): Promise<Company | null>;

  findUserCredentialsByEmail(
    email: string
  ): Promise<AuthUser | null>;

  findUserByEmail(
    email: string
  ): Promise<boolean>;

  findProducerByDni(
    empresaId: number,
    dni: string
  ): Promise<boolean>;

  createProducerAccount(
    data: RegisterProducerData
  ): Promise<number>;
}
