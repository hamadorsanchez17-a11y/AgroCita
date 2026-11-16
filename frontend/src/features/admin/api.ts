import { z } from 'zod'
import { http } from '@/shared/api/http'
import type { Paginado } from '@/shared/api/http'
import { reglas } from '@/shared/forms/zod'
import type { Empresa, EstadoSuscripcion, Suscripcion } from '@/features/organizacion/api'

export interface Plan {
  id: number
  nombre: string
  descripcion: string | null
  precioMensual: number
  maxTecnicos: number | null
  maxProductores: number | null
  activo: boolean
}

export interface EmpresaResumen {
  id: number
  nombre: string
  rtn: string
  plan: string | null
  estadoSuscripcion: EstadoSuscripcion | null
  suscripcionId: number | null
}

const limite = (mensaje: string) => z.number({ error: mensaje }).int(mensaje).positive(mensaje).optional()

export const planSchema = z.object({
  nombre: reglas.texto('El nombre del plan es obligatorio.', 60, 'El nombre no puede superar los 60 caracteres.'),
  descripcion: reglas.opcional(250, 'La descripción no puede superar los 250 caracteres.'),
  precioMensual: z
    .number({ error: 'El precio mensual debe ser mayor que 0.' })
    .positive('El precio mensual debe ser mayor que 0.'),
  maxTecnicos: limite('El máximo de técnicos debe ser mayor que 0.'),
  maxProductores: limite('El máximo de productores debe ser mayor que 0.'),
  activo: z.boolean(),
})

export type PlanValues = z.infer<typeof planSchema>

export const empresaSchema = z.object({
  nombre: reglas.texto('El nombre de la organización es obligatorio.', 150, 'El nombre no puede superar los 150 caracteres.'),
  rtn: z
    .string({ error: 'El RTN es obligatorio.' })
    .min(1, 'El RTN es obligatorio.')
    .regex(/^\d{14}$/, 'El RTN debe tener 14 dígitos.'),
  email: reglas.email(),
  telefono: reglas.telefono(),
  direccion: reglas.opcional(250, 'La dirección no puede superar los 250 caracteres.'),
  planId: reglas.id('El plan es obligatorio.'),
  admin: z.object({
    nombre: reglas.texto('El nombre del administrador es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
    apellido: reglas.opcional(100, 'El apellido no puede superar los 100 caracteres.'),
    email: reglas.email(),
    password: reglas.password(),
  }),
})

export type EmpresaValues = z.infer<typeof empresaSchema>

export const asignarPlanSchema = z
  .object({
    planId: reglas.id('El plan es obligatorio.'),
    fechaInicio: z.string({ error: 'La fecha de inicio es obligatoria.' }).min(1, 'La fecha de inicio es obligatoria.'),
    fechaFin: z.string().optional(),
  })
  .refine((valores) => !valores.fechaFin || !valores.fechaInicio || valores.fechaFin > valores.fechaInicio, {
    path: ['fechaFin'],
    message: 'La fecha de fin debe ser posterior a la fecha de inicio.',
  })

export type AsignarPlanValues = z.infer<typeof asignarPlanSchema>

export const adminKeys = {
  planes: ['admin', 'planes'] as const,
  empresas: (pagina: number) => ['admin', 'empresas', pagina] as const,
  todasEmpresas: ['admin', 'empresas'] as const,
}

export const adminApi = {
  planes: () => http.get<Plan[]>('/admin/planes').then((r) => r.data),
  crearPlan: (datos: PlanValues) => http.post<Plan>('/admin/planes', datos).then((r) => r.data),
  actualizarPlan: (id: number, datos: PlanValues) => http.put<Plan>(`/admin/planes/${id}`, datos).then((r) => r.data),
  empresas: (pagina: number) =>
    http.get<Paginado<EmpresaResumen>>('/admin/empresas', { params: { pagina, tamano: 10 } }).then((r) => r.data),
  crearEmpresa: (datos: EmpresaValues) => http.post<Empresa>('/admin/empresas', datos).then((r) => r.data),
  asignarPlan: (empresaId: number, datos: AsignarPlanValues) =>
    http.put<Suscripcion>(`/admin/empresas/${empresaId}/suscripcion`, datos).then((r) => r.data),
  cambiarEstadoSuscripcion: (id: number, estado: EstadoSuscripcion) =>
    http.patch<Suscripcion>(`/admin/suscripciones/${id}/estado`, { estado }).then((r) => r.data),
}
