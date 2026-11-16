import { useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import { Check } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import { fincasApi, fincasKeys } from '@/features/fincas/api'
import { comoNumero, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { hoyISO, sumarDias } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select, Textarea } from '@/shared/ui/Field'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { solicitudesApi, solicitudesKeys } from './api'
import { solicitudSchema } from './schemas'

interface SolicitudFormProps {
  fincaInicial?: number
  alCerrar: () => void
}

export function SolicitudForm({ fincaInicial, alCerrar }: SolicitudFormProps) {
  const navegar = useNavigate()
  const fincas = useQuery({ queryKey: fincasKeys.todas, queryFn: fincasApi.listar })

  const form = useZodForm(solicitudSchema, {
    fincaId: fincaInicial,
    cultivoIds: [],
    fechaDeseada: sumarDias(hoyISO(), 3),
    motivo: '',
  })

  const {
    register,
    watch,
    setValue,
    formState: { errors, isValid },
  } = form

  const fincaId = watch('fincaId')
  const seleccionados = watch('cultivoIds')

  const finca = useQuery({
    queryKey: fincasKeys.detalle(fincaId ?? 0),
    queryFn: () => fincasApi.obtener(fincaId),
    enabled: Boolean(fincaId),
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: solicitudesApi.crear,
    invalidar: [solicitudesKeys.todas],
    mensaje: 'Solicitud enviada a tu organización.',
    alExito: (solicitud) => {
      alCerrar()
      navegar(`/solicitudes/${solicitud.id}`)
    },
  })

  const alternar = (cultivoId: number) => {
    const siguiente = seleccionados.includes(cultivoId)
      ? seleccionados.filter((id) => id !== cultivoId)
      : [...seleccionados, cultivoId]

    setValue('cultivoIds', siguiente, { shouldValidate: true, shouldDirty: true })
  }

  return (
    <Modal
      abierto
      alCerrar={alCerrar}
      titulo="Solicitar asistencia técnica"
      descripcion="Cuéntale a tu organización qué necesitas y en qué cultivos."
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Select
          etiqueta="Finca"
          disabled={fincas.isPending}
          error={errors.fincaId?.message}
          {...register('fincaId', {
            ...comoNumero,
            onChange: () => setValue('cultivoIds', [], { shouldValidate: true }),
          })}
        >
          <option value="">Selecciona…</option>
          {fincas.data?.map((opcion) => (
            <option key={opcion.id} value={opcion.id}>
              {opcion.nombre}
            </option>
          ))}
        </Select>

        <fieldset>
          <legend className="mb-1.5 text-[13px] font-semibold text-ink-soft">Cultivos que necesitan atención</legend>
          {!fincaId ? (
            <p className="rounded-[10px] border border-dashed border-line px-3.5 py-3 text-sm text-ink-faint">
              Elige primero una finca.
            </p>
          ) : finca.isPending ? (
            <p className="text-sm text-ink-faint">Cargando cultivos…</p>
          ) : finca.data?.cultivos.length === 0 ? (
            <p className="rounded-[10px] border border-dashed border-line px-3.5 py-3 text-sm text-ink-faint">
              Esta finca no tiene cultivos registrados.
            </p>
          ) : (
            <div className="flex flex-wrap gap-2">
              {finca.data?.cultivos.map((cultivo) => {
                const activo = seleccionados.includes(cultivo.id)

                return (
                  <button
                    key={cultivo.id}
                    type="button"
                    role="checkbox"
                    aria-checked={activo}
                    onClick={() => alternar(cultivo.id)}
                    className={clsx(
                      'inline-flex cursor-pointer items-center gap-2 rounded-full border px-3.5 py-2 text-sm font-medium transition',
                      activo
                        ? 'border-forest bg-forest text-paper'
                        : 'border-line bg-white/70 text-ink-soft hover:border-ink-faint',
                    )}
                  >
                    <span
                      className={clsx(
                        'grid size-4 place-items-center rounded-full border',
                        activo ? 'border-ochre bg-ochre text-forest-deep' : 'border-line',
                      )}
                    >
                      {activo && <Check className="size-3" strokeWidth={3} aria-hidden />}
                    </span>
                    {cultivo.nombre}
                  </button>
                )
              })}
            </div>
          )}
          {errors.cultivoIds?.message && fincaId && (
            <p role="alert" className="mt-1.5 text-[13px] text-clay">
              {errors.cultivoIds.message}
            </p>
          )}
        </fieldset>

        <Input
          etiqueta="Fecha deseada"
          type="date"
          min={hoyISO()}
          clase="sm:max-w-[240px]"
          className="font-mono"
          error={errors.fechaDeseada?.message}
          {...register('fechaDeseada')}
        />
        <Textarea
          etiqueta="Motivo"
          placeholder="Describe lo que observas: plagas, hojas amarillas, baja producción…"
          error={errors.motivo?.message}
          {...register('motivo')}
        />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" variante="acento" cargando={cargando} disabled={!isValid}>
            Enviar solicitud
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}
