import { http } from '@/shared/api/http'
import type { Referencia } from '@/shared/api/http'

export interface Empresa {
  id: number
  nombre: string
  rtn: string
  email: string
  telefono: string
  direccion: string | null
  codigoInvitacion: string
  activa: boolean
}

export const ESTADOS_SUSCRIPCION = ['Activa', 'Vencida', 'Suspendida', 'Cancelada'] as const
export type EstadoSuscripcion = (typeof ESTADOS_SUSCRIPCION)[number]

export interface Suscripcion {
  id: number
  empresa: Referencia
  plan: Referencia
  fechaInicio: string
  fechaFin: string | null
  estado: EstadoSuscripcion
}

export const organizacionKeys = {
  empresa: ['empresa'] as const,
  suscripcion: ['empresa', 'suscripcion'] as const,
}

export const organizacionApi = {
  empresa: () => http.get<Empresa>('/empresa').then((r) => r.data),
  suscripcion: () => http.get<Suscripcion>('/empresa/suscripcion').then((r) => r.data),
  regenerarCodigo: () => http.post<Empresa>('/empresa/codigo-invitacion').then((r) => r.data),
}
