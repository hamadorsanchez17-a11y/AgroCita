export interface Farm {
  id: number;
  productorId: number;
  nombre: string;
  departamento: string;
  municipio: string;
  ubicacion: string;
  areaManzanas: number;
  descripcion: string | null;
}