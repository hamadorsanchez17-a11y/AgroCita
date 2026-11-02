
export interface TokenPayload {
  sub: number;
  empresaId: number | null;
  rol: string;
}

export interface TokenService {
  generate(payload: TokenPayload): string;
}
