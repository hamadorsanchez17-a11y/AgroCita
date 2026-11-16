import { z } from 'zod'
import { http, limpiarParams } from '@/shared/api/http'
import type { Referencia } from '@/shared/api/http'
import { reglas } from '@/shared/forms/zod'
import type { Aplicacion, Inspeccion, Recomendacion } from '@/features/calidad/api'
import type { Visita } from '@/features/visitas/api'

export interface Informe {
  id: number
  visitaId: number
  fecha: string
  tecnico: Referencia
  observaciones: string
  diagnostico: string
  recomendaciones: string
  inspecciones: Inspeccion[]
}

export interface InformeResumen {
  id: number
  visitaId: number
  fecha: string
  finca: Referencia
  tecnico: Referencia
}

export interface Historial {
  finca: Referencia | null
  cultivo: Referencia | null
  visitas: Visita[]
  inspecciones: Inspeccion[]
  recomendaciones: Recomendacion[]
  aplicaciones: Aplicacion[]
}

export interface FiltroHistorial {
  productorId?: number
  fincaId?: number
  cultivoId?: number
}

export const informeSchema = z.object({
  observaciones: reglas.texto('Las observaciones son obligatorias.', 4000, 'Las observaciones no pueden superar los 4000 caracteres.'),
  diagnostico: reglas.texto('El diagnóstico es obligatorio.', 4000, 'El diagnóstico no puede superar los 4000 caracteres.'),
  recomendaciones: reglas.texto('Las recomendaciones son obligatorias.', 4000, 'Las recomendaciones no pueden superar los 4000 caracteres.'),
})

export type InformeValues = z.infer<typeof informeSchema>

export const informesKeys = {
  deVisita: (visitaId: number) => ['informes', 'visita', visitaId] as const,
  mios: ['informes', 'mios'] as const,
  historial: (filtro: FiltroHistorial) => ['historial', 'general', filtro] as const,
}

export const informesApi = {
  crear: (visitaId: number, datos: InformeValues) =>
    http.post<Informe>(`/visitas/${visitaId}/informe`, datos).then((r) => r.data),
  obtener: (visitaId: number) => http.get<Informe>(`/visitas/${visitaId}/informe`).then((r) => r.data),
  mios: () => http.get<InformeResumen[]>('/productores/me/informes').then((r) => r.data),
  miHistorial: (filtro: FiltroHistorial) =>
    http.get<Historial>('/productores/me/historial', { params: limpiarParams({ ...filtro }) }).then((r) => r.data),
  historial: (filtro: FiltroHistorial) =>
    http.get<Historial>('/historial', { params: limpiarParams({ ...filtro }) }).then((r) => r.data),
}
