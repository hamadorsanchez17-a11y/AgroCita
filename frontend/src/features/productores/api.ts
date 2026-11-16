import { z } from 'zod'
import { http, limpiarParams } from '@/shared/api/http'
import type { Paginado } from '@/shared/api/http'
import { reglas } from '@/shared/forms/zod'
import type { Finca } from '@/features/fincas/api'

export interface Productor {
  id: number
  nombre: string
  apellido: string
  dni: string
  telefono: string
  direccion: string | null
  email: string
}

export interface ProductorDetalle extends Productor {
  fincas: Finca[]
}

export interface ProductorResumen {
  id: number
  nombreCompleto: string
  dni: string
  telefono: string
  cantidadFincas: number
}

export const perfilSchema = z.object({
  nombre: reglas.texto('El nombre es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
  apellido: reglas.texto('El apellido es obligatorio.', 100, 'El apellido no puede superar los 100 caracteres.'),
  telefono: reglas.telefono(),
  direccion: reglas.opcional(250, 'La dirección no puede superar los 250 caracteres.'),
})

export type PerfilValues = z.infer<typeof perfilSchema>

export const productoresKeys = {
  perfil: ['productores', 'me'] as const,
  lista: (filtro: object) => ['productores', 'lista', filtro] as const,
  detalle: (id: number) => ['productores', 'detalle', id] as const,
}

export const productoresApi = {
  miPerfil: () => http.get<Productor>('/productores/me').then((r) => r.data),
  actualizarPerfil: (datos: PerfilValues) => http.put<Productor>('/productores/me', datos).then((r) => r.data),
  listar: (filtro: { buscar?: string; pagina?: number; tamano?: number }) =>
    http.get<Paginado<ProductorResumen>>('/productores', { params: limpiarParams(filtro) }).then((r) => r.data),
  obtener: (id: number) => http.get<ProductorDetalle>(`/productores/${id}`).then((r) => r.data),
}
