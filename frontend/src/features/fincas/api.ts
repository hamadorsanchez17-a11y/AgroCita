import { http } from '@/shared/api/http'
import type { Referencia } from '@/shared/api/http'
import type { CultivoValues, FincaValues } from './schemas'

export interface Finca {
  id: number
  nombre: string
  departamento: string
  municipio: string
  direccion: string
  areaManzanas: number | null
  descripcion: string | null
  cantidadCultivos: number
}

export interface Cultivo {
  id: number
  fincaId: number
  nombre: string
  tipoCultivoId: number
  tipoCultivo: string
  variedad: string | null
  areaManzanas: number | null
  fechaSiembra: string | null
  activo: boolean
}

export interface FincaDetalle extends Finca {
  productor: Referencia
  cultivos: Cultivo[]
}

export interface TipoCultivo {
  id: number
  nombre: string
}

export const fincasKeys = {
  todas: ['fincas'] as const,
  detalle: (id: number) => ['fincas', id] as const,
  tipos: ['tipos-cultivo'] as const,
}

export const fincasApi = {
  listar: () => http.get<Finca[]>('/fincas').then((r) => r.data),
  obtener: (id: number) => http.get<FincaDetalle>(`/fincas/${id}`).then((r) => r.data),
  crear: (datos: FincaValues) => http.post<Finca>('/fincas', datos).then((r) => r.data),
  actualizar: (id: number, datos: FincaValues) => http.put<Finca>(`/fincas/${id}`, datos).then((r) => r.data),
  crearCultivo: (fincaId: number, datos: CultivoValues) =>
    http.post<Cultivo>(`/fincas/${fincaId}/cultivos`, datos).then((r) => r.data),
  actualizarCultivo: (id: number, datos: CultivoValues) =>
    http.put<Cultivo>(`/cultivos/${id}`, datos).then((r) => r.data),
  tipos: () => http.get<TipoCultivo[]>('/tipos-cultivo').then((r) => r.data),
  crearTipo: (datos: { nombre: string }) => http.post<TipoCultivo>('/tipos-cultivo', datos).then((r) => r.data),
}
