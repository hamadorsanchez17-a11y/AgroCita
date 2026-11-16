import { http, limpiarParams } from '@/shared/api/http'
import type { Referencia } from '@/shared/api/http'

export const ESTADOS_VISITA = ['Pendiente', 'Confirmada', 'EnProceso', 'Completada', 'Cancelada'] as const
export type EstadoVisita = (typeof ESTADOS_VISITA)[number]

export interface Visita {
  id: number
  solicitudId: number
  finca: Referencia
  productor: Referencia
  tecnico: Referencia
  fechaProgramada: string
  estado: EstadoVisita
  motivoCancelacion: string | null
}

export interface VisitaDetalle extends Visita {
  ubicacion: { departamento: string; municipio: string; direccion: string }
  cultivos: Referencia[]
  motivo: string
  tieneInforme: boolean
}

export interface VisitaAgenda {
  id: number
  fechaProgramada: string
  tecnico: Referencia
  finca: Referencia
  estado: EstadoVisita
}

export interface Reprogramacion {
  fechaAnterior: string
  fechaNueva: string
  motivo: string
  fechaCambio: string
  realizadoPor: string
}

interface FiltroFechas {
  desde?: string
  hasta?: string
}

export const visitasKeys = {
  todas: ['visitas'] as const,
  detalle: (id: number) => ['visitas', 'detalle', id] as const,
  reprogramaciones: (id: number) => ['visitas', 'reprogramaciones', id] as const,
  mias: (filtro: object) => ['visitas', 'mias', filtro] as const,
  agenda: (filtro: object) => ['visitas', 'agenda', filtro] as const,
  productor: (id: number) => ['visitas', 'productor', id] as const,
}

export const visitasApi = {
  crear: (datos: { solicitudId: number; tecnicoId: number; fechaProgramada: string }) =>
    http.post<Visita>('/visitas', datos).then((r) => r.data),
  obtener: (id: number) => http.get<VisitaDetalle>(`/visitas/${id}`).then((r) => r.data),
  cambiarEstado: (id: number, datos: { estado: EstadoVisita; motivo?: string }) =>
    http.patch<Visita>(`/visitas/${id}/estado`, datos).then((r) => r.data),
  reprogramar: (id: number, datos: { nuevaFecha: string; motivo: string }) =>
    http.put<Visita>(`/visitas/${id}/reprogramar`, datos).then((r) => r.data),
  reprogramaciones: (id: number) =>
    http.get<Reprogramacion[]>(`/visitas/${id}/reprogramaciones`).then((r) => r.data),
  mias: (filtro: FiltroFechas & { estado?: EstadoVisita }) =>
    http.get<Visita[]>('/visitas/mias', { params: limpiarParams(filtro) }).then((r) => r.data),
  agenda: (filtro: FiltroFechas & { tecnicoId?: number }) =>
    http.get<VisitaAgenda[]>('/visitas/agenda', { params: limpiarParams(filtro) }).then((r) => r.data),
  porProductor: (productorId: number) =>
    http.get<Visita[]>(`/productores/${productorId}/visitas`).then((r) => r.data),
}
