
export interface AuthUser {
  id: number;
  empresaId: number | null;
  nombreCompleto: string;
  email: string;
  passwordHash: string;
  rol: string;
  activo: boolean;
}
