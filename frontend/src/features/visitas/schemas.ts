import { z } from 'zod'
import { reglas } from '@/shared/forms/zod'
import { ESTADOS_VISITA } from './api'

const fechaFutura = (obligatoria: string, futura: string) =>
  z
    .string({ error: obligatoria })
    .min(1, obligatoria)
    .refine((valor) => !valor || new Date(valor).getTime() > Date.now(), futura)

export const visitaSchema = z.object({
  tecnicoId: reglas.id('El técnico es obligatorio.'),
  fechaProgramada: fechaFutura('La fecha programada es obligatoria.', 'La fecha programada debe ser futura.'),
})

export type VisitaValues = z.infer<typeof visitaSchema>

export const estadoVisitaSchema = z
  .object({
    estado: z.enum(ESTADOS_VISITA, { error: 'El estado es obligatorio.' }),
    motivo: reglas.opcional(250, 'El motivo no puede superar los 250 caracteres.'),
  })
  .refine((valores) => valores.estado !== 'Cancelada' || Boolean(valores.motivo?.trim()), {
    path: ['motivo'],
    message: 'El motivo es obligatorio al cancelar una visita.',
  })

export type EstadoVisitaValues = z.infer<typeof estadoVisitaSchema>

export const reprogramarSchema = z.object({
  nuevaFecha: fechaFutura('La nueva fecha es obligatoria.', 'La nueva fecha debe ser futura.'),
  motivo: reglas.texto(
    'El motivo de la reprogramación es obligatorio.',
    250,
    'El motivo no puede superar los 250 caracteres.',
  ),
})

export type ReprogramarValues = z.infer<typeof reprogramarSchema>
