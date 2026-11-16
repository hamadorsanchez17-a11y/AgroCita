import { useQuery } from '@tanstack/react-query'
import { ListaInspecciones } from '@/features/calidad/Registros'
import { useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { FormError, Textarea } from '@/shared/ui/Field'
import { QueryView } from '@/shared/ui/feedback'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { informeSchema, informesApi, informesKeys } from './api'

export function InformeForm({ visitaId, alCerrar }: { visitaId: number; alCerrar: () => void }) {
  const form = useZodForm(informeSchema, { observaciones: '', diagnostico: '', recomendaciones: '' })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => informesApi.crear(visitaId, valores),
    invalidar: [['visitas'], ['informes']],
    mensaje: 'Informe entregado al productor.',
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
      ancho="lg"
      titulo="Elaborar informe de visita"
      descripcion="Solo puede existir un informe por visita. Incluirá las inspecciones que ya registraste."
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Textarea etiqueta="Observaciones" rows={4} placeholder="Lo que encontraste en la finca." error={errors.observaciones?.message} {...register('observaciones')} />
        <Textarea etiqueta="Diagnóstico" rows={3} placeholder="La causa o el estado identificado." error={errors.diagnostico?.message} {...register('diagnostico')} />
        <Textarea etiqueta="Recomendaciones" rows={4} placeholder="Lo que debe hacer el productor." error={errors.recomendaciones?.message} {...register('recomendaciones')} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" variante="acento" cargando={cargando} disabled={!isValid}>
            Entregar informe
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

function Apartado({ titulo, children }: { titulo: string; children: string }) {
  return (
    <section>
      <h3 className="eyebrow mb-2">{titulo}</h3>
      <p className="text-[15.5px] leading-relaxed whitespace-pre-line text-ink">{children}</p>
    </section>
  )
}

export function InformeView({ visitaId }: { visitaId: number }) {
  const informe = useQuery({ queryKey: informesKeys.deVisita(visitaId), queryFn: () => informesApi.obtener(visitaId) })

  return (
    <QueryView query={informe} filas={3}>
      {(datos) => (
        <article className="px-5 py-6 md:px-8">
          <header className="mb-6 flex flex-wrap items-baseline justify-between gap-2 border-b border-line pb-4">
            <p className="font-display text-2xl">Informe técnico</p>
            <p className="font-mono text-xs text-ink-soft">
              {formato.fechaHora(datos.fecha)} · {datos.tecnico.nombre}
            </p>
          </header>
          <div className="flex flex-col gap-6">
            <Apartado titulo="Observaciones">{datos.observaciones}</Apartado>
            <Apartado titulo="Diagnóstico">{datos.diagnostico}</Apartado>
            <Apartado titulo="Recomendaciones">{datos.recomendaciones}</Apartado>
            {datos.inspecciones.length > 0 && (
              <section>
                <h3 className="eyebrow mb-1">Inspecciones de la visita</h3>
                <ListaInspecciones items={datos.inspecciones} conCultivo />
              </section>
            )}
          </div>
        </article>
      )}
    </QueryView>
  )
}
