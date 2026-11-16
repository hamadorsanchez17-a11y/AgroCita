import { http } from '@/shared/api/http'
import type { Referencia } from '@/shared/api/http'

export interface Parametro {
  id: number
  tipoCultivoId: number
  nombre: string
  unidad: string | null
  valorMinimo: number | null
  valorMaximo: number | null
}

export interface Categoria {
  id: number
  nombre: string
  descripcion: string | null
}

export interface Inspeccion {
  id: number
  visitaId: number
  cultivo: Referencia
  tecnico: Referencia
  fecha: string
  categoriaCalidad: Referencia | null
  observaciones: string | null
  resultados: { parametroId: number; parametro: string; unidad: string | null; valor: string }[]
}

export interface Fertilizante {
  id: number
  nombre: string
  tipo: string
  unidadMedida: string
  composicion: string | null
}

export interface Recomendacion {
  id: number
  cultivoId: number
  visitaId: number
  fertilizante: Referencia
  dosis: number
  unidad: string
  frecuencia: string | null
  indicaciones: string
  fecha: string
  tecnico: Referencia
}

export interface Aplicacion {
  id: number
  cultivoId: number
  recomendacionId: number | null
  fertilizante: Referencia
  cantidad: number
  unidad: string
  fechaAplicacion: string
  observaciones: string | null
  tecnico: Referencia
}

export const calidadKeys = {
  parametros: (tipoCultivoId: number) => ['parametros', tipoCultivoId] as const,
  categorias: ['categorias-calidad'] as const,
  fertilizantes: ['fertilizantes'] as const,
  inspecciones: (cultivoId: number) => ['historial', 'inspecciones', cultivoId] as const,
  recomendaciones: (cultivoId: number) => ['historial', 'recomendaciones', cultivoId] as const,
  aplicaciones: (cultivoId: number) => ['historial', 'aplicaciones', cultivoId] as const,
}

export const calidadApi = {
  parametros: (tipoCultivoId: number) =>
    http.get<Parametro[]>(`/tipos-cultivo/${tipoCultivoId}/parametros`).then((r) => r.data),
  crearParametro: (tipoCultivoId: number, datos: object) =>
    http.post<Parametro>(`/tipos-cultivo/${tipoCultivoId}/parametros`, datos).then((r) => r.data),
  categorias: () => http.get<Categoria[]>('/categorias-calidad').then((r) => r.data),
  crearCategoria: (datos: object) => http.post<Categoria>('/categorias-calidad', datos).then((r) => r.data),
  fertilizantes: () => http.get<Fertilizante[]>('/fertilizantes').then((r) => r.data),
  crearFertilizante: (datos: object) => http.post<Fertilizante>('/fertilizantes', datos).then((r) => r.data),
  crearInspeccion: (visitaId: number, datos: object) =>
    http.post<Inspeccion>(`/visitas/${visitaId}/inspecciones`, datos).then((r) => r.data),
  inspecciones: (cultivoId: number) =>
    http.get<Inspeccion[]>(`/cultivos/${cultivoId}/inspecciones`).then((r) => r.data),
  crearRecomendacion: (cultivoId: number, datos: object) =>
    http.post<Recomendacion>(`/cultivos/${cultivoId}/recomendaciones`, datos).then((r) => r.data),
  recomendaciones: (cultivoId: number) =>
    http.get<Recomendacion[]>(`/cultivos/${cultivoId}/recomendaciones`).then((r) => r.data),
  crearAplicacion: (cultivoId: number, datos: object) =>
    http.post<Aplicacion>(`/cultivos/${cultivoId}/aplicaciones`, datos).then((r) => r.data),
  aplicaciones: (cultivoId: number) =>
    http.get<Aplicacion[]>(`/cultivos/${cultivoId}/aplicaciones`).then((r) => r.data),
}
