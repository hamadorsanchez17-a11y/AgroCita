import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select, Textarea } from '@/shared/ui/Field'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { fincasApi, fincasKeys } from './api'
import type { Finca } from './api'
import { DEPARTAMENTOS, fincaSchema } from './schemas'

interface FincaFormProps {
  finca?: Finca
  alCerrar: () => void
}

export function FincaForm({ finca, alCerrar }: FincaFormProps) {
  const form = useZodForm(fincaSchema, {
    nombre: finca?.nombre ?? '',
    departamento: finca?.departamento ?? '',
    municipio: finca?.municipio ?? '',
    direccion: finca?.direccion ?? '',
    areaManzanas: finca?.areaManzanas ?? undefined,
    descripcion: finca?.descripcion ?? undefined,
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => (finca ? fincasApi.actualizar(finca.id, valores) : fincasApi.crear(valores)),
    invalidar: [fincasKeys.todas],
    mensaje: finca ? 'Finca actualizada.' : 'Finca registrada.',
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
      titulo={finca ? 'Editar finca' : 'Registrar finca'}
      descripcion="Los datos de ubicación ayudan al técnico a llegar a tu propiedad."
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre de la finca" placeholder="Finca El Paraíso" error={errors.nombre?.message} {...register('nombre')} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Select etiqueta="Departamento" error={errors.departamento?.message} {...register('departamento')}>
            <option value="">Selecciona…</option>
            {DEPARTAMENTOS.map((departamento) => (
              <option key={departamento}>{departamento}</option>
            ))}
          </Select>
          <Input etiqueta="Municipio" placeholder="Valle de Ángeles" error={errors.municipio?.message} {...register('municipio')} />
        </div>
        <Input
          etiqueta="Dirección"
          placeholder="Aldea, caserío o referencia para llegar"
          error={errors.direccion?.message}
          {...register('direccion')}
        />
        <Input
          etiqueta="Área (manzanas)"
          type="number"
          step="0.01"
          min="0"
          opcional
          clase="sm:max-w-[220px]"
          className="font-mono"
          error={errors.areaManzanas?.message}
          {...register('areaManzanas', comoNumero)}
        />
        <Textarea
          etiqueta="Descripción"
          opcional
          rows={3}
          error={errors.descripcion?.message}
          {...register('descripcion', comoTextoOpcional)}
        />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            {finca ? 'Guardar cambios' : 'Registrar finca'}
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}
