import { z } from 'zod'
import { http } from '@/shared/api/http'
import { reglas } from '@/shared/forms/zod'

export interface Tecnico {
  id: number
  nombre: string
  apellido: string
  email: string
  telefono: string
  especialidad: string | null
  activo: boolean
}

const base = {
  nombre: reglas.texto('El nombre es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
  apellido: reglas.texto('El apellido es obligatorio.', 100, 'El apellido no puede superar los 100 caracteres.'),
  telefono: reglas.telefono(),
  especialidad: reglas.opcional(100, 'La especialidad no puede superar los 100 caracteres.'),
}

export const tecnicoEditarSchema = z.object(base)

export const tecnicoCrearSchema = z.object({
  ...base,
  email: reglas.email(),
  passwordTemporal: z
    .string({ error: 'La contraseña temporal es obligatoria.' })
    .min(1, 'La contraseña temporal es obligatoria.')
    .min(8, 'La contraseña temporal debe tener al menos 8 caracteres.'),
})

export type TecnicoEditarValues = z.infer<typeof tecnicoEditarSchema>
export type TecnicoCrearValues = z.infer<typeof tecnicoCrearSchema>

export const tecnicosKeys = { todos: ['tecnicos'] as const }

export const tecnicosApi = {
  listar: () => http.get<Tecnico[]>('/tecnicos').then((r) => r.data),
  crear: (datos: TecnicoCrearValues) => http.post<Tecnico>('/tecnicos', datos).then((r) => r.data),
  actualizar: (id: number, datos: TecnicoEditarValues) =>
    http.put<Tecnico>(`/tecnicos/${id}`, datos).then((r) => r.data),
  cambiarEstado: (id: number, activo: boolean) => http.patch<void>(`/tecnicos/${id}/estado`, { activo }).then(() => undefined),
}
