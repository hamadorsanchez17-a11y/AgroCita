import { useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import {
  Ban,
  CalendarClock,
  Check,
  ChevronDown,
  CircleCheckBig,
  Droplets,
  FileText,
  FlaskConical,
  Microscope,
  Play,
  Wheat,
} from 'lucide-react'
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { HistorialCultivo } from '@/features/calidad/HistorialCultivo'
import { AplicacionForm, InspeccionForm, RecomendacionForm } from '@/features/calidad/RegistroForms'
import { InformeForm, InformeView } from '@/features/informes/InformePanel'
import { useAuth } from '@/shared/auth/AuthContext'
import { useAccion } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Dato, PageHeader } from '@/shared/ui/layout'
import { visitasApi, visitasKeys } from './api'
import type { EstadoVisita, VisitaDetalle } from './api'
import { CancelarVisitaForm, ReprogramarForm } from './VisitaForms'

type ModalActivo = 'cancelar' | 'reprogramar' | 'inspeccion' | 'recomendacion' | 'aplicacion' | 'informe' | null

const ETAPAS: { estado: EstadoVisita; etiqueta: string }[] = [
  { estado: 'Pendiente', etiqueta: 'Pendiente' },
  { estado: 'Confirmada', etiqueta: 'Confirmada' },
  { estado: 'EnProceso', etiqueta: 'En proceso' },
  { estado: 'Completada', etiqueta: 'Completada' },
]

function Progreso({ estado }: { estado: EstadoVisita }) {
  const actual = ETAPAS.findIndex((etapa) => etapa.estado === estado)

  return (
    <ol className="flex items-center" aria-label="Avance de la visita">
      {ETAPAS.map((etapa, indice) => {
        const hecha = indice < actual || estado === 'Completada'
        const activa = indice === actual && !hecha

        return (
          <li key={etapa.estado} className={clsx('flex items-center', indice > 0 && 'flex-1')}>
            {indice > 0 && <span className={clsx('mx-2 h-px flex-1', indice <= actual ? 'bg-forest' : 'bg-line')} aria-hidden />}
            <span className="flex flex-col items-center gap-1.5">
              <span
                className={clsx(
                  'grid size-7 place-items-center rounded-full border font-mono text-[11px] transition',
                  hecha && 'border-forest bg-forest text-paper',
                  activa && 'border-ochre bg-ochre text-forest-deep ring-4 ring-ochre-soft',
                  !hecha && !activa && 'border-line bg-card text-ink-faint',
                )}
              >
                {hecha ? <Check className="size-3.5" strokeWidth={3} aria-hidden /> : indice + 1}
              </span>
              <span className={clsx('text-[11px] whitespace-nowrap', activa ? 'font-semibold text-ink' : 'text-ink-faint')}>
                {etapa.etiqueta}
              </span>
            </span>
          </li>
        )
      })}
    </ol>
  )
}

function Reprogramaciones({ visitaId }: { visitaId: number }) {
  const cambios = useQuery({
    queryKey: visitasKeys.reprogramaciones(visitaId),
    queryFn: () => visitasApi.reprogramaciones(visitaId),
  })

  if (!cambios.data || cambios.data.length === 0) return null

  return (
    <Card>
      <CardHeader titulo="Bitácora de reprogramaciones" descripcion="Cada cambio de fecha queda registrado." />
      <ul className="px-5 py-2 md:px-6">
        {cambios.data.map((cambio) => (
          <li key={cambio.fechaCambio} className="border-b border-line-soft py-3.5 last:border-0">
            <p className="font-mono text-[13px]">
              <span className="text-ink-faint line-through">{formato.fechaHora(cambio.fechaAnterior)}</span>
              <span className="mx-2 text-ochre-deep">→</span>
              {formato.fechaHora(cambio.fechaNueva)}
            </p>
            <p className="mt-1 text-[15px] text-ink-soft">{cambio.motivo}</p>
            <p className="mt-1 text-[13px] text-ink-faint">
              {cambio.realizadoPor} · {formato.fecha(cambio.fechaCambio)}
            </p>
          </li>
        ))}
      </ul>
    </Card>
  )
}

function Contenido({ visita }: { visita: VisitaDetalle }) {
  const { tieneRol } = useAuth()
  const esAdmin = tieneRol('AdminEmpresa')
  const esTecnico = tieneRol('Tecnico')
  const [modal, setModal] = useState<ModalActivo>(null)
  const [cultivoAbierto, setCultivoAbierto] = useState<number | null>(null)

  const cerrada = visita.estado === 'Completada' || visita.estado === 'Cancelada'
  const enCampo = visita.estado === 'EnProceso' || visita.estado === 'Completada'

  const avanzar = useAccion({
    mutationFn: (estado: EstadoVisita) => visitasApi.cambiarEstado(visita.id, { estado }),
    invalidar: [visitasKeys.todas, ['solicitudes']],
    mensaje: 'Estado de la visita actualizado.',
  })

  const volver = esAdmin
    ? { a: '/agenda', texto: 'Agenda' }
    : esTecnico
      ? { a: '/visitas', texto: 'Mis visitas' }
      : { a: `/solicitudes/${visita.solicitudId}`, texto: 'Solicitud' }

  return (
    <>
      <PageHeader
        seccion={`Visita V-${String(visita.id).padStart(4, '0')} · ${formato.diaSemana(visita.fechaProgramada)}`}
        titulo={visita.finca.nombre}
        volver={volver}
        descripcion={
          <span className="flex flex-wrap items-center gap-3">
            <EstadoBadge estado={visita.estado} />
            <span className="font-mono text-sm">{formato.fechaHora(visita.fechaProgramada)}</span>
          </span>
        }
        acciones={
          <>
            {esAdmin && !cerrada && (
              <Button variante="secundario" icono={<CalendarClock className="size-4" />} onClick={() => setModal('reprogramar')}>
                Reprogramar
              </Button>
            )}
            {(esAdmin || esTecnico) && visita.estado === 'Pendiente' && (
              <Button icono={<Check className="size-4" />} cargando={avanzar.isPending} onClick={() => avanzar.mutate('Confirmada')}>
                Confirmar
              </Button>
            )}
            {esTecnico && visita.estado === 'Confirmada' && (
              <Button variante="acento" icono={<Play className="size-4" />} cargando={avanzar.isPending} onClick={() => avanzar.mutate('EnProceso')}>
                Iniciar visita
              </Button>
            )}
            {esTecnico && visita.estado === 'EnProceso' && (
              <Button icono={<CircleCheckBig className="size-4" />} cargando={avanzar.isPending} onClick={() => avanzar.mutate('Completada')}>
                Completar visita
              </Button>
            )}
            {(esAdmin || esTecnico) && !cerrada && (
              <Button variante="fantasma" icono={<Ban className="size-4" />} onClick={() => setModal('cancelar')}>
                Cancelar
              </Button>
            )}
          </>
        }
      />

      <div className="stagger flex flex-col gap-6">
        {visita.estado === 'Cancelada' ? (
          <div className="rounded-2xl border border-clay/40 bg-clay-soft/40 px-5 py-4">
            <p className="eyebrow !text-clay">Visita cancelada</p>
            <p className="mt-1 text-[15px]">{visita.motivoCancelacion}</p>
          </div>
        ) : (
          <Card className="px-5 py-5 md:px-8">
            <Progreso estado={visita.estado} />
          </Card>
        )}

        <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.4fr)]">
          <Card className="h-fit">
            <CardHeader titulo="La cita" numero="01" />
            <dl className="grid gap-5 px-5 py-5 md:px-6">
              <Dato etiqueta="Técnico asignado">{visita.tecnico.nombre}</Dato>
              <Dato etiqueta="Productor">
                {esAdmin ? (
                  <Link to={`/productores/${visita.productor.id}`} className="font-semibold text-forest underline decoration-ochre decoration-2 underline-offset-4">
                    {visita.productor.nombre}
                  </Link>
                ) : (
                  visita.productor.nombre
                )}
              </Dato>
              <Dato etiqueta="Cómo llegar">
                {visita.ubicacion.direccion}
                <span className="block text-sm text-ink-soft">
                  {visita.ubicacion.municipio}, {visita.ubicacion.departamento}
                </span>
              </Dato>
              <Dato etiqueta="Motivo de la solicitud">
                <span className="font-display text-lg leading-snug italic">“{visita.motivo}”</span>
              </Dato>
            </dl>
          </Card>

          <Card>
            <CardHeader
              titulo="Cultivos a atender"
              numero="02"
              descripcion="Abre un cultivo para ver su historial de inspecciones y fertilización."
            />
            {esTecnico && enCampo && (
              <div className="flex flex-wrap gap-2 border-b border-line-soft bg-paper/50 px-5 py-3.5 md:px-6">
                <Button tamano="sm" variante="secundario" icono={<Microscope className="size-3.5" />} onClick={() => setModal('inspeccion')}>
                  Inspección
                </Button>
                <Button tamano="sm" variante="secundario" icono={<FlaskConical className="size-3.5" />} onClick={() => setModal('recomendacion')}>
                  Recomendación
                </Button>
                <Button tamano="sm" variante="secundario" icono={<Droplets className="size-3.5" />} onClick={() => setModal('aplicacion')}>
                  Aplicación
                </Button>
              </div>
            )}
            <ul>
              {visita.cultivos.map((cultivo) => {
                const expandido = cultivoAbierto === cultivo.id

                return (
                  <li key={cultivo.id} className="border-b border-line-soft last:border-0">
                    <button
                      type="button"
                      onClick={() => setCultivoAbierto(expandido ? null : cultivo.id)}
                      aria-expanded={expandido}
                      className="flex w-full cursor-pointer items-center gap-4 px-5 py-4 text-left md:px-6"
                    >
                      <span className="grid size-9 shrink-0 place-items-center rounded-full bg-moss-soft text-forest-light">
                        <Wheat className="size-4" aria-hidden />
                      </span>
                      <span className="flex-1 text-[16px] font-semibold">{cultivo.nombre}</span>
                      <ChevronDown className={clsx('size-5 text-ink-faint transition', expandido && 'rotate-180')} aria-hidden />
                    </button>
                    {expandido && <HistorialCultivo cultivoId={cultivo.id} />}
                  </li>
                )
              })}
            </ul>
          </Card>
        </div>

        <Card className="overflow-hidden">
          <CardHeader
            titulo="Informe de la visita"
            numero="03"
            accion={
              esTecnico &&
              enCampo &&
              !visita.tieneInforme && (
                <Button tamano="sm" variante="acento" icono={<FileText className="size-3.5" />} onClick={() => setModal('informe')}>
                  Elaborar informe
                </Button>
              )
            }
          />
          {visita.tieneInforme ? (
            <InformeView visitaId={visita.id} />
          ) : (
            <div className="p-5 md:p-6">
              <EmptyState
                compacto
                icono={<FileText className="size-5" />}
                titulo="Aún no hay informe"
                descripcion={
                  esTecnico
                    ? 'Podrás elaborarlo cuando la visita esté en proceso o completada.'
                    : 'El técnico lo entrega al terminar la visita.'
                }
              />
            </div>
          )}
        </Card>

        {esAdmin && <Reprogramaciones visitaId={visita.id} />}
      </div>

      {modal === 'cancelar' && <CancelarVisitaForm visita={visita} alCerrar={() => setModal(null)} />}
      {modal === 'reprogramar' && <ReprogramarForm visita={visita} alCerrar={() => setModal(null)} />}
      {modal === 'inspeccion' && <InspeccionForm visita={visita} alCerrar={() => setModal(null)} />}
      {modal === 'recomendacion' && <RecomendacionForm visita={visita} alCerrar={() => setModal(null)} />}
      {modal === 'aplicacion' && <AplicacionForm visita={visita} alCerrar={() => setModal(null)} />}
      {modal === 'informe' && <InformeForm visitaId={visita.id} alCerrar={() => setModal(null)} />}
    </>
  )
}

export function VisitaDetallePage() {
  const id = Number(useParams().id)
  const visita = useQuery({ queryKey: visitasKeys.detalle(id), queryFn: () => visitasApi.obtener(id) })

  return <QueryView query={visita}>{(datos) => <Contenido visita={datos} />}</QueryView>
}
