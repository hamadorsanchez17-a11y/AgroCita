import { useQuery } from '@tanstack/react-query'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { hoyISO } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select } from '@/shared/ui/Field'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { fincasApi, fincasKeys } from './api'
import type { Cultivo } from './api'
import { cultivoSchema } from './schemas'

interface CultivoFormProps {
  fincaId: number
  cultivo?: Cultivo
  alCerrar: () => void
}

export function CultivoForm({ fincaId, cultivo, alCerrar }: CultivoFormProps) {
  const tipos = useQuery({ queryKey: fincasKeys.tipos, queryFn: fincasApi.tipos })

  const form = useZodForm(cultivoSchema, {
    tipoCultivoId: cultivo?.tipoCultivoId,
    nombre: cultivo?.nombre ?? undefined,
    variedad: cultivo?.variedad ?? undefined,
    areaManzanas: cultivo?.areaManzanas ?? undefined,
    fechaSiembra: cultivo?.fechaSiembra ?? undefined,
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) =>
      cultivo ? fincasApi.actualizarCultivo(cultivo.id, valores) : fincasApi.crearCultivo(fincaId, valores),
    invalidar: [fincasKeys.todas],
    mensaje: cultivo ? 'Cultivo actualizado.' : 'Cultivo registrado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} titulo={cultivo ? 'Editar cultivo' : 'Registrar cultivo'}>
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Select
          etiqueta="Tipo de cultivo"
          disabled={tipos.isPending}
          error={errors.tipoCultivoId?.message}
          ayuda={tipos.data?.length === 0 ? 'Tu organización todavía no ha definido tipos de cultivo.' : undefined}
          {...register('tipoCultivoId', comoNumero)}
        >
          <option value="">Selecciona…</option>
          {tipos.data?.map((tipo) => (
            <option key={tipo.id} value={tipo.id}>
              {tipo.nombre}
            </option>
          ))}
        </Select>
        <Input
          etiqueta="Nombre o lote"
          opcional
          placeholder="Café lote 1"
          ayuda="Si lo dejas vacío se usa el nombre del tipo de cultivo."
          error={errors.nombre?.message}
          {...register('nombre', comoTextoOpcional)}
        />
        <Input etiqueta="Variedad" opcional placeholder="Lempira" error={errors.variedad?.message} {...register('variedad', comoTextoOpcional)} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Input
            etiqueta="Área sembrada (mz)"
            type="number"
            step="0.01"
            min="0"
            opcional
            className="font-mono"
            error={errors.areaManzanas?.message}
            {...register('areaManzanas', comoNumero)}
          />
          <Input
            etiqueta="Fecha de siembra"
            type="date"
            max={hoyISO()}
            opcional
            className="font-mono"
            error={errors.fechaSiembra?.message}
            {...register('fechaSiembra', comoTextoOpcional)}
          />
        </div>
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            {cultivo ? 'Guardar cambios' : 'Registrar cultivo'}
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}
