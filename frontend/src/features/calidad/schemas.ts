import { z } from 'zod'
import { reglas } from '@/shared/forms/zod'
import { hoyISO } from '@/shared/lib/format'

export const parametroSchema = z
  .object({
    nombre: reglas.texto('El nombre del parámetro es obligatorio.', 80, 'El nombre no puede superar los 80 caracteres.'),
    unidad: reglas.opcional(20, 'La unidad no puede superar los 20 caracteres.'),
    valorMinimo: z.number({ error: 'Ingresa un número válido.' }).optional(),
    valorMaximo: z.number({ error: 'Ingresa un número válido.' }).optional(),
  })
  .refine(
    (valores) =>
      valores.valorMinimo === undefined ||
      valores.valorMaximo === undefined ||
      valores.valorMaximo >= valores.valorMinimo,
    { path: ['valorMaximo'], message: 'El valor máximo debe ser mayor o igual que el mínimo.' },
  )

export type ParametroValues = z.infer<typeof parametroSchema>

export const categoriaSchema = z.object({
  nombre: reglas.texto('El nombre de la categoría es obligatorio.', 60, 'El nombre no puede superar los 60 caracteres.'),
  descripcion: reglas.opcional(250, 'La descripción no puede superar los 250 caracteres.'),
})

export type CategoriaValues = z.infer<typeof categoriaSchema>

export const fertilizanteSchema = z.object({
  nombre: reglas.texto('El nombre del fertilizante es obligatorio.', 100, 'El nombre no puede superar los 100 caracteres.'),
  tipo: reglas.texto('El tipo de fertilizante es obligatorio.', 40, 'El tipo no puede superar los 40 caracteres.'),
  unidadMedida: reglas.texto('La unidad de medida es obligatoria.', 20, 'La unidad de medida no puede superar los 20 caracteres.'),
  composicion: reglas.opcional(250, 'La composición no puede superar los 250 caracteres.'),
})

export type FertilizanteValues = z.infer<typeof fertilizanteSchema>

export const inspeccionSchema = z
  .object({
    cultivoId: reglas.id('El cultivo es obligatorio.'),
    categoriaCalidadId: z.number().optional(),
    observaciones: z.string().optional(),
    resultados: z.array(
      z.object({
        parametroId: z.number(),
        valor: z.string().max(100, 'El valor no puede superar los 100 caracteres.'),
      }),
    ),
  })
  .refine((valores) => valores.resultados.some((resultado) => resultado.valor.trim() !== ''), {
    path: ['resultados'],
    message: 'Debe registrar al menos un resultado.',
  })

export type InspeccionValues = z.infer<typeof inspeccionSchema>

export const recomendacionSchema = z.object({
  cultivoId: reglas.id('El cultivo es obligatorio.'),
  fertilizanteId: reglas.id('El fertilizante es obligatorio.'),
  dosis: z.number({ error: 'La dosis debe ser mayor que 0.' }).positive('La dosis debe ser mayor que 0.'),
  frecuencia: reglas.opcional(100, 'La frecuencia no puede superar los 100 caracteres.'),
  indicaciones: reglas.texto('Las indicaciones son obligatorias.', 500, 'Las indicaciones no pueden superar los 500 caracteres.'),
})

export type RecomendacionValues = z.infer<typeof recomendacionSchema>

export const aplicacionSchema = z.object({
  cultivoId: reglas.id('El cultivo es obligatorio.'),
  fertilizanteId: reglas.id('El fertilizante es obligatorio.'),
  recomendacionId: z.number().optional(),
  cantidad: z.number({ error: 'La cantidad debe ser mayor que 0.' }).positive('La cantidad debe ser mayor que 0.'),
  fechaAplicacion: z
    .string({ error: 'La fecha de aplicación es obligatoria.' })
    .min(1, 'La fecha de aplicación es obligatoria.')
    .refine((fecha) => !fecha || fecha <= hoyISO(), 'La fecha de aplicación no puede ser futura.'),
  observaciones: reglas.opcional(250, 'Las observaciones no pueden superar los 250 caracteres.'),
})

export type AplicacionValues = z.infer<typeof aplicacionSchema>
