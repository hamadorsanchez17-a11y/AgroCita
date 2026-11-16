import { useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { aISOLocal, formato } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Textarea } from '@/shared/ui/Field'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { visitasApi, visitasKeys } from './api'
import type { Visita } from './api'
import { estadoVisitaSchema, reprogramarSchema } from './schemas'

interface Props {
  visita: Visita
  alCerrar: () => void
}

const INVALIDAR = [visitasKeys.todas, ['solicitudes']]

export function CancelarVisitaForm({ visita, alCerrar }: Props) {
  const form = useZodForm(estadoVisitaSchema, { estado: 'Cancelada', motivo: '' })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => visitasApi.cambiarEstado(visita.id, valores),
    invalidar: INVALIDAR,
    mensaje: 'Visita cancelada.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Cancelar visita" descripcion="Una visita cancelada no se puede reabrir.">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Textarea etiqueta="Motivo de la cancelación" rows={3} error={errors.motivo?.message} {...register('motivo')} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Volver
          </Button>
          <Button type="submit" variante="peligro" cargando={cargando} disabled={!isValid}>
            Cancelar visita
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function ReprogramarForm({ visita, alCerrar }: Props) {
  const form = useZodForm(reprogramarSchema, { nuevaFecha: aISOLocal(visita.fechaProgramada), motivo: '' })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) =>
      visitasApi.reprogramar(visita.id, {
        nuevaFecha: new Date(valores.nuevaFecha).toISOString(),
        motivo: valores.motivo,
      }),
    invalidar: INVALIDAR,
    mensaje: 'Visita reprogramada.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal
      abierto
      alCerrar={alCerrar}
      ancho="sm"
      titulo="Reprogramar visita"
      descripcion={`Fecha actual: ${formato.fechaHora(visita.fechaProgramada)}. La solicitud original se conserva.`}
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nueva fecha y hora" type="datetime-local" className="font-mono" error={errors.nuevaFecha?.message} {...register('nuevaFecha')} />
        <Textarea etiqueta="Motivo" rows={3} placeholder="Lluvias, cambio de disponibilidad…" error={errors.motivo?.message} {...register('motivo')} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Reprogramar
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}
