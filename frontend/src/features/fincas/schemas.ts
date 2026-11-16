import { z } from 'zod'
import { reglas } from '@/shared/forms/zod'
import { hoyISO } from '@/shared/lib/format'

export const DEPARTAMENTOS = [
  'Atlántida',
  'Choluteca',
  'Colón',
  'Comayagua',
  'Copán',
  'Cortés',
  'El Paraíso',
  'Francisco Morazán',
  'Gracias a Dios',
  'Intibucá',
  'Islas de la Bahía',
  'La Paz',
  'Lempira',
  'Ocotepeque',
  'Olancho',
  'Santa Bárbara',
  'Valle',
  'Yoro',
]

export const fincaSchema = z.object({
  nombre: reglas.texto('El nombre de la finca es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
  departamento: reglas.texto('El departamento es obligatorio.', 60, 'El departamento no puede superar los 60 caracteres.'),
  municipio: reglas.texto('El municipio es obligatorio.', 60, 'El municipio no puede superar los 60 caracteres.'),
  direccion: reglas.texto('La dirección es obligatoria.', 250, 'La dirección no puede superar los 250 caracteres.'),
  areaManzanas: reglas.positivoOpcional('El área debe ser mayor que 0.'),
  descripcion: reglas.opcional(500, 'La descripción no puede superar los 500 caracteres.'),
})

export type FincaValues = z.infer<typeof fincaSchema>

export const cultivoSchema = z.object({
  tipoCultivoId: reglas.id('El tipo de cultivo es obligatorio.'),
  nombre: reglas.opcional(100, 'El nombre no puede superar los 100 caracteres.'),
  variedad: reglas.opcional(60, 'La variedad no puede superar los 60 caracteres.'),
  areaManzanas: reglas.positivoOpcional('El área debe ser mayor que 0.'),
  fechaSiembra: z
    .string()
    .optional()
    .refine((fecha) => !fecha || fecha <= hoyISO(), 'La fecha de siembra no puede ser futura.'),
})

export type CultivoValues = z.infer<typeof cultivoSchema>

export const tipoCultivoSchema = z.object({
  nombre: reglas.texto('El nombre del tipo de cultivo es obligatorio.', 60, 'El nombre no puede superar los 60 caracteres.'),
})

export type TipoCultivoValues = z.infer<typeof tipoCultivoSchema>
