import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import type { DefaultValues, FieldValues } from 'react-hook-form'
import { z } from 'zod'

export function useZodForm<T extends FieldValues>(schema: z.ZodType<T, T>, defaultValues: DefaultValues<T>) {
  return useForm<T>({
    resolver: zodResolver(schema),
    mode: 'onChange',
    defaultValues,
  })
}

export const comoNumero = {
  setValueAs: (valor: unknown) => (valor === '' || valor == null ? undefined : Number(valor)),
}

export const comoTextoOpcional = {
  setValueAs: (valor: unknown) => (typeof valor === 'string' && valor.trim() === '' ? undefined : valor),
}

export const reglas = {
  texto: (obligatorio: string, maximo: number, mensajeMaximo: string) =>
    z.string({ error: obligatorio }).trim().min(1, obligatorio).max(maximo, mensajeMaximo),
  opcional: (maximo: number, mensajeMaximo: string) =>
    z.string().trim().max(maximo, mensajeMaximo).optional(),
  email: () =>
    z
      .string({ error: 'El correo electrónico es obligatorio.' })
      .trim()
      .min(1, 'El correo electrónico es obligatorio.')
      .max(120, 'El correo electrónico no puede superar los 120 caracteres.')
      .regex(/^[^@\s]+@[^@\s]+$/, 'El correo electrónico no tiene un formato válido.'),
  telefono: () =>
    z
      .string({ error: 'El teléfono es obligatorio.' })
      .min(1, 'El teléfono es obligatorio.')
      .regex(/^\d{8}$/, 'El teléfono debe tener 8 dígitos.'),
  password: () =>
    z
      .string({ error: 'La contraseña es obligatoria.' })
      .min(1, 'La contraseña es obligatoria.')
      .min(8, 'La contraseña debe tener al menos 8 caracteres.')
      .regex(/[A-Z]/, 'La contraseña debe incluir una mayúscula.')
      .regex(/\d/, 'La contraseña debe incluir un número.'),
  id: (mensaje: string) => z.number({ error: mensaje }).int().positive(mensaje),
  positivoOpcional: (mensaje: string) => z.number({ error: mensaje }).positive(mensaje).optional(),
}
