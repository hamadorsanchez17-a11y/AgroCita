
export interface ProducerProfileDTO {
  id: number;
  usuarioId: number;
  empresaId: number;
  nombreCompleto: string;
  email: string;
  dni: string;
  telefono: string;
  direccion: string | null;
}
