import { useQuery } from '@tanstack/react-query'
import { useEffect } from 'react'
import { useFieldArray } from 'react-hook-form'
import { fincasApi, fincasKeys } from '@/features/fincas/api'
import type { VisitaDetalle } from '@/features/visitas/api'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { hoyISO } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select, Textarea } from '@/shared/ui/Field'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { calidadApi, calidadKeys } from './api'
import { aplicacionSchema, inspeccionSchema, recomendacionSchema } from './schemas'

interface RegistroProps {
  visita: VisitaDetalle
  alCerrar: () => void
}

const HISTORIAL = ['historial'] as const

function OpcionesCultivo({ visita }: { visita: VisitaDetalle }) {
  return (
    <>
      <option value="">Selecciona…</option>
      {visita.cultivos.map((cultivo) => (
        <option key={cultivo.id} value={cultivo.id}>
          {cultivo.nombre}
        </option>
      ))}
    </>
  )
}

export function InspeccionForm({ visita, alCerrar }: RegistroProps) {
  const form = useZodForm(inspeccionSchema, {
    cultivoId: visita.cultivos.length === 1 ? visita.cultivos[0].id : undefined,
    categoriaCalidadId: undefined,
    observaciones: undefined,
    resultados: [],
  })

  const {
    register,
    control,
    watch,
    formState: { errors, isValid },
  } = form

  const { fields, replace } = useFieldArray({ control, name: 'resultados' })
  const cultivoId = watch('cultivoId')

  const finca = useQuery({ queryKey: fincasKeys.detalle(visita.finca.id), queryFn: () => fincasApi.obtener(visita.finca.id) })
  const categorias = useQuery({ queryKey: calidadKeys.categorias, queryFn: calidadApi.categorias })
  const tipoCultivoId = finca.data?.cultivos.find((cultivo) => cultivo.id === cultivoId)?.tipoCultivoId

  const parametros = useQuery({
    queryKey: calidadKeys.parametros(tipoCultivoId ?? 0),
    queryFn: () => calidadApi.parametros(tipoCultivoId!),
    enabled: Boolean(tipoCultivoId),
  })

  useEffect(() => {
    replace((parametros.data ?? []).map((parametro) => ({ parametroId: parametro.id, valor: '' })))
  }, [parametros.data, replace])

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) =>
      calidadApi.crearInspeccion(visita.id, {
        ...valores,
        resultados: valores.resultados
          .filter((resultado) => resultado.valor.trim() !== '')
          .map((resultado) => ({ parametroId: resultado.parametroId, valor: resultado.valor.trim() })),
      }),
    invalidar: [HISTORIAL],
    mensaje: 'Inspección registrada.',
    alExito: alCerrar,
  })

  const errorResultados = errors.resultados?.root?.message ?? errors.resultados?.message

  return (
    <Modal
      abierto
      alCerrar={alCerrar}
      titulo="Registrar inspección"
      descripcion="Queda guardada en el historial del cultivo y no se puede modificar después."
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Select etiqueta="Cultivo inspeccionado" error={errors.cultivoId?.message} {...register('cultivoId', comoNumero)}>
            <OpcionesCultivo visita={visita} />
          </Select>
          <Select etiqueta="Categoría de calidad" opcional {...register('categoriaCalidadId', comoNumero)}>
            <option value="">Sin clasificar</option>
            {categorias.data?.map((categoria) => (
              <option key={categoria.id} value={categoria.id}>
                {categoria.nombre}
              </option>
            ))}
          </Select>
        </div>

        <fieldset className="rounded-xl border border-line bg-paper/60 p-4">
          <legend className="eyebrow px-1.5">Parámetros evaluados</legend>
          {!cultivoId ? (
            <p className="text-sm text-ink-faint">Elige un cultivo para ver sus parámetros.</p>
          ) : parametros.isPending || finca.isPending ? (
            <p className="text-sm text-ink-faint">Cargando parámetros…</p>
          ) : fields.length === 0 ? (
            <p className="text-sm text-clay">
              Este tipo de cultivo no tiene parámetros definidos. Pide al administrador que los agregue en Catálogos.
            </p>
          ) : (
            <div className="grid gap-3 sm:grid-cols-2">
              {fields.map((campo, indice) => {
                const parametro = parametros.data?.find((item) => item.id === campo.parametroId)
                const rango =
                  parametro?.valorMinimo != null && parametro.valorMaximo != null
                    ? `Referencia: ${parametro.valorMinimo}–${parametro.valorMaximo}`
                    : undefined

                return (
                  <Input
                    key={campo.id}
                    etiqueta={`${parametro?.nombre ?? 'Parámetro'}${parametro?.unidad ? ` (${parametro.unidad})` : ''}`}
                    className="font-mono"
                    ayuda={rango}
                    error={errors.resultados?.[indice]?.valor?.message}
                    {...register(`resultados.${indice}.valor`, { deps: ['resultados'] })}
                  />
                )
              })}
            </div>
          )}
          {errorResultados && fields.length > 0 && (
            <p role="alert" className="mt-3 text-[13px] text-clay">
              {errorResultados}
            </p>
          )}
        </fieldset>

        <Textarea etiqueta="Observaciones" opcional rows={3} {...register('observaciones', comoTextoOpcional)} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Guardar inspección
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function RecomendacionForm({ visita, alCerrar }: RegistroProps) {
  const fertilizantes = useQuery({ queryKey: calidadKeys.fertilizantes, queryFn: calidadApi.fertilizantes })

  const form = useZodForm(recomendacionSchema, {
    cultivoId: visita.cultivos.length === 1 ? visita.cultivos[0].id : undefined,
    fertilizanteId: undefined,
    dosis: undefined,
    frecuencia: undefined,
    indicaciones: '',
  })

  const {
    register,
    watch,
    formState: { errors, isValid },
  } = form

  const unidad = fertilizantes.data?.find((fertilizante) => fertilizante.id === watch('fertilizanteId'))?.unidadMedida

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: ({ cultivoId, ...valores }) => calidadApi.crearRecomendacion(cultivoId, { ...valores, visitaId: visita.id }),
    invalidar: [HISTORIAL],
    mensaje: 'Recomendación registrada.',
    alExito: alCerrar,
  })

  return (
    <Modal abierto alCerrar={alCerrar} titulo="Recomendar fertilización" descripcion="Lo que el productor debería aplicar al cultivo.">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Select etiqueta="Cultivo" error={errors.cultivoId?.message} {...register('cultivoId', comoNumero)}>
            <OpcionesCultivo visita={visita} />
          </Select>
          <Select etiqueta="Fertilizante" disabled={fertilizantes.isPending} error={errors.fertilizanteId?.message} {...register('fertilizanteId', comoNumero)}>
            <option value="">Selecciona…</option>
            {fertilizantes.data?.map((fertilizante) => (
              <option key={fertilizante.id} value={fertilizante.id}>
                {fertilizante.nombre}
              </option>
            ))}
          </Select>
        </div>
        <div className="grid gap-4 sm:grid-cols-2">
          <Input
            etiqueta={`Dosis${unidad ? ` (${unidad})` : ''}`}
            type="number"
            step="0.01"
            min="0"
            className="font-mono"
            error={errors.dosis?.message}
            {...register('dosis', comoNumero)}
          />
          <Input etiqueta="Frecuencia" opcional placeholder="Cada 30 días" error={errors.frecuencia?.message} {...register('frecuencia', comoTextoOpcional)} />
        </div>
        <Textarea etiqueta="Indicaciones" rows={3} error={errors.indicaciones?.message} {...register('indicaciones')} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Guardar recomendación
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function AplicacionForm({ visita, alCerrar }: RegistroProps) {
  const fertilizantes = useQuery({ queryKey: calidadKeys.fertilizantes, queryFn: calidadApi.fertilizantes })

  const form = useZodForm(aplicacionSchema, {
    cultivoId: visita.cultivos.length === 1 ? visita.cultivos[0].id : undefined,
    fertilizanteId: undefined,
    recomendacionId: undefined,
    cantidad: undefined,
    fechaAplicacion: hoyISO(),
    observaciones: undefined,
  })

  const {
    register,
    watch,
    formState: { errors, isValid },
  } = form

  const cultivoId = watch('cultivoId')
  const unidad = fertilizantes.data?.find((fertilizante) => fertilizante.id === watch('fertilizanteId'))?.unidadMedida

  const recomendaciones = useQuery({
    queryKey: calidadKeys.recomendaciones(cultivoId ?? 0),
    queryFn: () => calidadApi.recomendaciones(cultivoId),
    enabled: Boolean(cultivoId),
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: ({ cultivoId: cultivo, ...valores }) => calidadApi.crearAplicacion(cultivo, valores),
    invalidar: [HISTORIAL],
    mensaje: 'Aplicación registrada.',
    alExito: alCerrar,
  })

  return (
    <Modal abierto alCerrar={alCerrar} titulo="Registrar aplicación" descripcion="El abono que efectivamente se aplicó al cultivo.">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Select etiqueta="Cultivo" error={errors.cultivoId?.message} {...register('cultivoId', comoNumero)}>
            <OpcionesCultivo visita={visita} />
          </Select>
          <Select etiqueta="Fertilizante" disabled={fertilizantes.isPending} error={errors.fertilizanteId?.message} {...register('fertilizanteId', comoNumero)}>
            <option value="">Selecciona…</option>
            {fertilizantes.data?.map((fertilizante) => (
              <option key={fertilizante.id} value={fertilizante.id}>
                {fertilizante.nombre}
              </option>
            ))}
          </Select>
        </div>
        <div className="grid gap-4 sm:grid-cols-2">
          <Input
            etiqueta={`Cantidad${unidad ? ` (${unidad})` : ''}`}
            type="number"
            step="0.01"
            min="0"
            className="font-mono"
            error={errors.cantidad?.message}
            {...register('cantidad', comoNumero)}
          />
          <Input
            etiqueta="Fecha de aplicación"
            type="date"
            max={hoyISO()}
            className="font-mono"
            error={errors.fechaAplicacion?.message}
            {...register('fechaAplicacion')}
          />
        </div>
        <Select etiqueta="Recomendación que se siguió" opcional disabled={!cultivoId} {...register('recomendacionId', comoNumero)}>
          <option value="">Ninguna</option>
          {recomendaciones.data?.map((recomendacion) => (
            <option key={recomendacion.id} value={recomendacion.id}>
              {recomendacion.fertilizante.nombre} · {recomendacion.dosis} {recomendacion.unidad}
            </option>
          ))}
        </Select>
        <Textarea etiqueta="Observaciones" opcional rows={2} error={errors.observaciones?.message} {...register('observaciones', comoTextoOpcional)} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Guardar aplicación
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}
