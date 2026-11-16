import { http, limpiarParams } from '@/shared/api/http'
import type { Paginado, Referencia } from '@/shared/api/http'
import type { Visita } from '@/features/visitas/api'

export const ESTADOS_SOLICITUD = ['Pendiente', 'EnAtencion', 'Atendida', 'Rechazada'] as const
export type EstadoSolicitud = (typeof ESTADOS_SOLICITUD)[number]

export interface Solicitud {
  id: number
  finca: Referencia
  productor: Referencia
  cultivos: Referencia[]
  fechaDeseada: string
  motivo: string
  estado: EstadoSolicitud
  comentario: string | null
  fechaCreacion: string
}

export interface SolicitudDetalle extends Solicitud {
  visitas: Visita[]
}

export interface FiltroSolicitudes {
  estado?: EstadoSolicitud
  desde?: string
  hasta?: string
  pagina?: number
  tamano?: number
}

export const solicitudesKeys = {
  todas: ['solicitudes'] as const,
  mias: (estado?: EstadoSolicitud) => ['solicitudes', 'mias', estado ?? 'todas'] as const,
  lista: (filtro: FiltroSolicitudes) => ['solicitudes', 'lista', filtro] as const,
  detalle: (id: number) => ['solicitudes', 'detalle', id] as const,
}

export const solicitudesApi = {
  crear: (datos: { fincaId: number; cultivoIds: number[]; fechaDeseada: string; motivo: string }) =>
    http.post<Solicitud>('/solicitudes', datos).then((r) => r.data),
  mias: (estado?: EstadoSolicitud) =>
    http.get<Solicitud[]>('/solicitudes/mias', { params: limpiarParams({ estado }) }).then((r) => r.data),
  listar: (filtro: FiltroSolicitudes) =>
    http.get<Paginado<Solicitud>>('/solicitudes', { params: limpiarParams({ ...filtro }) }).then((r) => r.data),
  obtener: (id: number) => http.get<SolicitudDetalle>(`/solicitudes/${id}`).then((r) => r.data),
  cambiarEstado: (id: number, datos: { estado: EstadoSolicitud; comentario?: string }) =>
    http.patch<Solicitud>(`/solicitudes/${id}/estado`, datos).then((r) => r.data),
}
