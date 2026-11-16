import { z } from 'zod'
import { reglas } from '@/shared/forms/zod'
import { hoyISO } from '@/shared/lib/format'
import { ESTADOS_SOLICITUD } from './api'

export const solicitudSchema = z.object({
  fincaId: reglas.id('La finca es obligatoria.'),
  cultivoIds: z.array(z.number()).min(1, 'Debe seleccionar al menos un cultivo.'),
  fechaDeseada: z
    .string({ error: 'La fecha deseada es obligatoria.' })
    .min(1, 'La fecha deseada es obligatoria.')
    .refine((fecha) => !fecha || fecha >= hoyISO(), 'La fecha deseada no puede ser pasada.'),
  motivo: reglas.texto('El motivo es obligatorio.', 500, 'El motivo no puede superar los 500 caracteres.'),
})

export type SolicitudValues = z.infer<typeof solicitudSchema>

export const estadoSolicitudSchema = z
  .object({
    estado: z.enum(ESTADOS_SOLICITUD, { error: 'El estado es obligatorio.' }),
    comentario: reglas.opcional(500, 'El comentario no puede superar los 500 caracteres.'),
  })
  .refine((valores) => valores.estado !== 'Rechazada' || Boolean(valores.comentario?.trim()), {
    path: ['comentario'],
    message: 'El comentario es obligatorio al rechazar una solicitud.',
  })

export type EstadoSolicitudValues = z.infer<typeof estadoSolicitudSchema>
