import { z } from 'zod'
import { reglas } from '@/shared/forms/zod'

export const loginSchema = z.object({
  email: reglas.email(),
  password: z.string({ error: 'La contraseña es obligatoria.' }).min(1, 'La contraseña es obligatoria.'),
})

export type LoginValues = z.infer<typeof loginSchema>

export const registroSchema = z.object({
  codigoInvitacion: z
    .string({ error: 'El código de invitación es obligatorio.' })
    .trim()
    .min(1, 'El código de invitación es obligatorio.')
    .length(12, 'El código de invitación debe tener 12 caracteres.'),
  nombre: reglas.texto('El nombre es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
  apellido: reglas.texto('El apellido es obligatorio.', 100, 'El apellido no puede superar los 100 caracteres.'),
  dni: z
    .string({ error: 'El DNI es obligatorio.' })
    .min(1, 'El DNI es obligatorio.')
    .regex(/^\d{13}$/, 'El DNI debe tener 13 dígitos.'),
  telefono: reglas.telefono(),
  email: reglas.email(),
  password: reglas.password(),
})

export type RegistroValues = z.infer<typeof registroSchema>
