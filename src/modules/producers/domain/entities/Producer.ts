
export interface Producer {
  id: number;
  empresaId: number;
  usuarioId: number;
  nombreCompleto: string;
  email: string;
  dni: string;
  telefono: string;
  direccion: string | null;
}
