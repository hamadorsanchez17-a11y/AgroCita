
export interface AuthUserDTO {
  id: number;
  empresaId: number | null;
  nombreCompleto: string;
  email: string;
  rol: string;
}

export interface AuthResponseDTO {
  accessToken: string;
  usuario: AuthUserDTO;
}
