import { useQuery } from '@tanstack/react-query'
import { CalendarPlus, CalendarX, RefreshCw } from 'lucide-react'
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { tecnicosApi, tecnicosKeys } from '@/features/tecnicos/api'
import { visitasApi, visitasKeys } from '@/features/visitas/api'
import { visitaSchema } from '@/features/visitas/schemas'
import { VisitaTicket } from '@/features/visitas/VisitaTicket'
import { useAuth } from '@/shared/auth/AuthContext'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { Badge, EstadoBadge, etiquetaEstado } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select, Textarea } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Dato, PageHeader } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { ESTADOS_SOLICITUD, solicitudesApi, solicitudesKeys } from './api'
import type { SolicitudDetalle } from './api'
import { estadoSolicitudSchema } from './schemas'

function ProgramarVisita({ solicitud, alCerrar }: { solicitud: SolicitudDetalle; alCerrar: () => void }) {
  const tecnicos = useQuery({ queryKey: tecnicosKeys.todos, queryFn: tecnicosApi.listar })
  const form = useZodForm(visitaSchema, { tecnicoId: undefined, fechaProgramada: `${solicitud.fechaDeseada}T08:00` })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) =>
      visitasApi.crear({
        solicitudId: solicitud.id,
        tecnicoId: valores.tecnicoId,
        fechaProgramada: new Date(valores.fechaProgramada).toISOString(),
      }),
    invalidar: [solicitudesKeys.todas, visitasKeys.todas],
    mensaje: 'Visita programada.',
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
      titulo="Programar visita"
      descripcion={`${solicitud.finca.nombre} · el productor la pidió para el ${formato.fecha(solicitud.fechaDeseada)}.`}
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Select etiqueta="Técnico" disabled={tecnicos.isPending} error={errors.tecnicoId?.message} {...register('tecnicoId', comoNumero)}>
          <option value="">Selecciona…</option>
          {tecnicos.data
            ?.filter((tecnico) => tecnico.activo)
            .map((tecnico) => (
              <option key={tecnico.id} value={tecnico.id}>
                {tecnico.nombre} {tecnico.apellido}
                {tecnico.especialidad ? ` · ${tecnico.especialidad}` : ''}
              </option>
            ))}
        </Select>
        <Input
          etiqueta="Fecha y hora"
          type="datetime-local"
          className="font-mono"
          ayuda="Un técnico no puede tener dos visitas con menos de una hora de diferencia."
          error={errors.fechaProgramada?.message}
          {...register('fechaProgramada')}
        />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Programar
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

function CambiarEstado({ solicitud, alCerrar }: { solicitud: SolicitudDetalle; alCerrar: () => void }) {
  const form = useZodForm(estadoSolicitudSchema, { estado: solicitud.estado, comentario: solicitud.comentario ?? undefined })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => solicitudesApi.cambiarEstado(solicitud.id, valores),
    invalidar: [solicitudesKeys.todas],
    mensaje: 'Estado de la solicitud actualizado.',
    alExito: alCerrar,
  })

  const {
    register,
    watch,
    formState: { errors, isValid },
  } = form

  const rechazando = watch('estado') === 'Rechazada'

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Cambiar estado">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Select etiqueta="Estado" error={errors.estado?.message} {...register('estado', { deps: ['comentario'] })}>
          {ESTADOS_SOLICITUD.map((estado) => (
            <option key={estado} value={estado}>
              {etiquetaEstado(estado)}
            </option>
          ))}
        </Select>
        <Textarea
          etiqueta={rechazando ? 'Motivo del rechazo' : 'Comentario'}
          opcional={!rechazando}
          rows={3}
          error={errors.comentario?.message}
          {...register('comentario', comoTextoOpcional)}
        />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" variante={rechazando ? 'peligro' : 'primario'} cargando={cargando} disabled={!isValid}>
            Guardar
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function SolicitudDetallePage() {
  const id = Number(useParams().id)
  const { tieneRol } = useAuth()
  const esAdmin = tieneRol('AdminEmpresa')
  const solicitud = useQuery({ queryKey: solicitudesKeys.detalle(id), queryFn: () => solicitudesApi.obtener(id) })
  const [modal, setModal] = useState<'visita' | 'estado' | null>(null)

  return (
    <QueryView query={solicitud}>
      {(datos) => {
        const cerrada = datos.estado === 'Rechazada' || datos.estado === 'Atendida'

        return (
          <>
            <PageHeader
              seccion={`Solicitud SOL-${String(datos.id).padStart(4, '0')}`}
              titulo={datos.finca.nombre}
              volver={{ a: '/solicitudes', texto: esAdmin ? 'Solicitudes' : 'Mis solicitudes' }}
              descripcion={
                <span className="flex flex-wrap items-center gap-3">
                  <EstadoBadge estado={datos.estado} />
                  <span>Enviada el {formato.fechaLarga(datos.fechaCreacion)}</span>
                </span>
              }
              acciones={
                esAdmin && (
                  <>
                    <Button variante="secundario" icono={<RefreshCw className="size-4" />} onClick={() => setModal('estado')}>
                      Cambiar estado
                    </Button>
                    {!cerrada && (
                      <Button icono={<CalendarPlus className="size-4" />} onClick={() => setModal('visita')}>
                        Programar visita
                      </Button>
                    )}
                  </>
                )
              }
            />

            <div className="stagger grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.25fr)]">
              <Card className="h-fit">
                <CardHeader titulo="Lo que pidió el productor" numero="01" />
                <div className="px-5 py-5 md:px-6">
                  <blockquote className="border-l-2 border-ochre pl-4 font-display text-xl leading-snug italic">
                    “{datos.motivo}”
                  </blockquote>
                  <dl className="mt-6 grid gap-5 sm:grid-cols-2">
                    <Dato etiqueta="Productor">
                      {esAdmin ? (
                        <Link to={`/productores/${datos.productor.id}`} className="font-semibold text-forest underline decoration-ochre decoration-2 underline-offset-4">
                          {datos.productor.nombre}
                        </Link>
                      ) : (
                        datos.productor.nombre
                      )}
                    </Dato>
                    <Dato etiqueta="Fecha deseada" mono>
                      {formato.fecha(datos.fechaDeseada)}
                    </Dato>
                    <div className="sm:col-span-2">
                      <dt className="eyebrow mb-2">Cultivos</dt>
                      <dd className="flex flex-wrap gap-2">
                        {datos.cultivos.map((cultivo) => (
                          <Badge key={cultivo.id} punto={false}>
                            {cultivo.nombre}
                          </Badge>
                        ))}
                      </dd>
                    </div>
                  </dl>
                  {datos.comentario && (
                    <div className="mt-6 rounded-xl border border-line bg-paper px-4 py-3">
                      <p className="eyebrow mb-1">Comentario de la organización</p>
                      <p className="text-[15px] text-ink-soft">{datos.comentario}</p>
                    </div>
                  )}
                </div>
              </Card>

              <Card>
                <CardHeader titulo="Visitas" numero="02" descripcion="Una solicitud puede generar una o varias visitas." />
                <div className="flex flex-col gap-3 p-5 md:p-6">
                  {datos.visitas.length === 0 ? (
                    <EmptyState
                      compacto
                      icono={<CalendarX className="size-5" />}
                      titulo="Todavía no hay visitas programadas"
                      descripcion={esAdmin ? 'Asigna un técnico y una fecha para atender esta solicitud.' : 'Tu organización revisará la solicitud y te asignará un técnico.'}
                    />
                  ) : (
                    datos.visitas.map((visita) => <VisitaTicket key={visita.id} visita={visita} />)
                  )}
                </div>
              </Card>
            </div>

            {modal === 'visita' && <ProgramarVisita solicitud={datos} alCerrar={() => setModal(null)} />}
            {modal === 'estado' && <CambiarEstado solicitud={datos} alCerrar={() => setModal(null)} />}
          </>
        )
      }}
    </QueryView>
  )
}
