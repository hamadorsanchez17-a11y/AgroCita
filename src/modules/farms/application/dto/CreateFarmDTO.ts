export interface CreateFarmDTO {
  nombre: string;
  departamento: string;
  municipio: string;
  ubicacion: string;
  areaManzanas: number;
  descripcion?: string | undefined;
}