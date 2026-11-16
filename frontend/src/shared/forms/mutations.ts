import { useMutation, useQueryClient } from '@tanstack/react-query'
import type { QueryKey } from '@tanstack/react-query'
import { useState } from 'react'
import type { FieldValues, UseFormReturn } from 'react-hook-form'
import { applyServerErrors } from '@/shared/api/errors'
import { useToast } from '@/shared/ui/Toast'

interface OpcionesAccion<TEntrada, TResultado> {
  mutationFn: (entrada: TEntrada) => Promise<TResultado>
  invalidar?: QueryKey[]
  mensaje?: string
  alExito?: (resultado: TResultado) => void
}

export function useAccion<TEntrada = void, TResultado = unknown>({
  mutationFn,
  invalidar = [],
  mensaje,
  alExito,
}: OpcionesAccion<TEntrada, TResultado>) {
  const queryClient = useQueryClient()
  const toast = useToast()

  return useMutation({
    mutationFn,
    onSuccess: async (resultado) => {
      await Promise.all(invalidar.map((queryKey) => queryClient.invalidateQueries({ queryKey })))
      if (mensaje) toast.ok(mensaje)
      alExito?.(resultado)
    },
    onError: (error) => toast.error(error),
  })
}

export function useFormMutation<TValores extends FieldValues, TResultado>(
  form: UseFormReturn<TValores>,
  { mutationFn, invalidar = [], mensaje, alExito }: OpcionesAccion<TValores, TResultado>,
) {
  const queryClient = useQueryClient()
  const toast = useToast()
  const [errorGeneral, setErrorGeneral] = useState<string | null>(null)

  const mutation = useMutation({
    mutationFn,
    onSuccess: async (resultado) => {
      await Promise.all(invalidar.map((queryKey) => queryClient.invalidateQueries({ queryKey })))
      if (mensaje) toast.ok(mensaje)
      alExito?.(resultado)
    },
    onError: (error) =>
      setErrorGeneral(applyServerErrors(error, form.setError, Object.keys(form.getValues()))),
  })

  return {
    enviar: form.handleSubmit((valores) => {
      setErrorGeneral(null)
      mutation.mutate(valores)
    }),
    errorGeneral,
    cargando: mutation.isPending,
  }
}
