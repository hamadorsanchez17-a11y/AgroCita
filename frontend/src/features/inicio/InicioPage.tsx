import { useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import { ArrowRight, CalendarCheck, ClipboardPlus } from 'lucide-react'
import { useEffect, useState } from 'react'
import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { fincasApi, fincasKeys } from '@/features/fincas/api'
import { informesApi, informesKeys } from '@/features/informes/api'
import { productoresApi, productoresKeys } from '@/features/productores/api'
import { solicitudesApi, solicitudesKeys } from '@/features/solicitudes/api'
import { tecnicosApi, tecnicosKeys } from '@/features/tecnicos/api'
import { visitasApi, visitasKeys } from '@/features/visitas/api'
import { VisitaTicket } from '@/features/visitas/VisitaTicket'
import { useUsuario } from '@/shared/auth/AuthContext'
import { claveDia, formato, hoyISO, sumarDias } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { EmptyState, QueryView, Skeleton } from '@/shared/ui/feedback'
import { Card, CardHeader, Contours, PageHeader, seguirCursor } from '@/shared/ui/layout'

function saludo(): string {
  const hora = new Date().getHours()

  if (hora < 12) return 'Buenos días'
  if (hora < 19) return 'Buenas tardes'
  return 'Buenas noches'
}

function Encabezado({ seccion, descripcion, acciones }: { seccion: string; descripcion: string; acciones?: ReactNode }) {
  const usuario = useUsuario()

  return (
    <PageHeader
      seccion={`${seccion} · ${formato.fechaLarga(hoyISO())}`}
      titulo={
        <>
          {saludo()}, <span className="italic text-forest-light">{usuario.nombre.split(' ')[0]}</span>
        </>
      }
      descripcion={descripcion}
      acciones={acciones}
    />
  )
}

function useConteo(valor?: number): number | undefined {
  const [animado, setAnimado] = useState(0)

  useEffect(() => {
    if (!valor) return

    const duracion = window.matchMedia('(prefers-reduced-motion: reduce)').matches ? 1 : 700
    const inicio = performance.now()
    let cuadro = 0

    const avanzar = (ahora: number) => {
      const progreso = Math.min(1, (ahora - inicio) / duracion)
      setAnimado(Math.round(valor * (1 - (1 - progreso) ** 3)))
      if (progreso < 1) cuadro = requestAnimationFrame(avanzar)
    }

    cuadro = requestAnimationFrame(avanzar)
    return () => cancelAnimationFrame(cuadro)
  }, [valor])

  if (valor === undefined) return undefined

  return valor === 0 ? 0 : Math.min(animado, valor)
}

interface IndicadorProps {
  etiqueta: string
  valor?: number
  a: string
  nota?: string
  destacado?: boolean
}

function Indicador({ etiqueta, valor, a, nota, destacado = false }: IndicadorProps) {
  const mostrado = useConteo(valor)

  return (
    <Link
      to={a}
      onMouseMove={seguirCursor}
      className={clsx(
        'group relative flex flex-col justify-between overflow-hidden rounded-[22px] border p-5 transition duration-300 ease-[var(--ease-salida)] hover:-translate-y-1 hover:shadow-lift md:p-6',
        destacado
          ? 'col-span-2 border-forest-deep bg-forest-deep text-paper shadow-lift sm:col-span-1 sm:row-span-1'
          : 'border-line/80 bg-card shadow-sheet hover:border-forest/25',
      )}
    >
      <div className="foco pointer-events-none absolute inset-0 opacity-0 transition-opacity duration-300 group-hover:opacity-100" aria-hidden />
      {destacado && (
        <>
          <div className="reticula pointer-events-none absolute inset-0" aria-hidden />
          <div className="pointer-events-none absolute -top-16 -right-10 size-48 rounded-full bg-ochre/20 blur-3xl" aria-hidden />
          <Contours className="pointer-events-none absolute -right-24 -bottom-28 size-[300px] text-ochre/25 transition duration-1000 group-hover:scale-110 group-hover:rotate-12" />
        </>
      )}
      <div className="relative flex items-center justify-between">
        <p className={clsx('eyebrow', destacado && '!text-ochre')}>{etiqueta}</p>
        <span
          className={clsx(
            'grid size-8 place-items-center rounded-full border transition duration-300 group-hover:-rotate-45',
            destacado ? 'border-paper/15 bg-paper/5 text-paper/70 group-hover:bg-ochre group-hover:text-forest-deep' : 'border-line text-ink-faint group-hover:border-forest group-hover:bg-forest group-hover:text-paper',
          )}
          aria-hidden
        >
          <ArrowRight className="size-4" />
        </span>
      </div>
      <div className="relative mt-6">
        {mostrado === undefined ? (
          <Skeleton className={clsx('h-14 w-20', destacado && 'opacity-20')} />
        ) : (
          <p className="font-display text-[3rem] leading-none font-semibold tracking-[-0.05em] tabular-nums sm:text-[3.6rem]">{mostrado}</p>
        )}
      </div>
      <p className={clsx('relative mt-2 min-h-5 text-[13px]', destacado ? 'text-paper/65' : 'text-ink-soft')}>{nota}</p>
    </Link>
  )
}

function InicioProductor() {
  const fincas = useQuery({ queryKey: fincasKeys.todas, queryFn: fincasApi.listar })
  const solicitudes = useQuery({ queryKey: solicitudesKeys.mias(), queryFn: () => solicitudesApi.mias() })
  const historial = useQuery({ queryKey: informesKeys.historial({}), queryFn: () => informesApi.miHistorial({}) })

  const abiertas = solicitudes.data?.filter((solicitud) => solicitud.estado === 'Pendiente' || solicitud.estado === 'EnAtencion')
  const proximas = historial.data?.visitas
    .filter((visita) => visita.estado !== 'Completada' && visita.estado !== 'Cancelada')
    .sort((a, b) => a.fechaProgramada.localeCompare(b.fechaProgramada))

  return (
    <>
      <Encabezado
        seccion="Productor"
        descripcion="El estado de tus fincas y de la asistencia técnica que has pedido."
        acciones={
          <Link to="/solicitudes?finca=0">
            <Button variante="acento" icono={<ClipboardPlus className="size-4" />}>
              Solicitar asistencia
            </Button>
          </Link>
        }
      />
      <div className="stagger flex flex-col gap-6">
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3">
          <Indicador destacado etiqueta="Fincas registradas" valor={fincas.data?.length} a="/fincas" />
          <Indicador etiqueta="Solicitudes abiertas" valor={abiertas?.length} a="/solicitudes" />
          <Indicador etiqueta="Visitas por recibir" valor={proximas?.length} a="/solicitudes" />
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <Card>
            <CardHeader titulo="Próximas visitas" descripcion="Los técnicos que llegarán a tus fincas." />
            <div className="flex flex-col gap-3 p-5 md:p-6">
              <QueryView query={historial} filas={2}>
                {() =>
                  proximas && proximas.length > 0 ? (
                    proximas.slice(0, 4).map((visita) => <VisitaTicket key={visita.id} visita={visita} />)
                  ) : (
                    <EmptyState compacto icono={<CalendarCheck className="size-5" />} titulo="No tienes visitas programadas" />
                  )
                }
              </QueryView>
            </div>
          </Card>

          <Card>
            <CardHeader
              titulo="Solicitudes recientes"
              accion={
                <Link to="/solicitudes" className="text-[13px] font-semibold text-forest hover:underline">
                  Ver todas
                </Link>
              }
            />
            <QueryView query={solicitudes} filas={2}>
              {(lista) =>
                lista.length === 0 ? (
                  <div className="p-5 md:p-6">
                    <EmptyState compacto titulo="Aún no has pedido asistencia" descripcion="Registra una finca con sus cultivos y envía tu primera solicitud." />
                  </div>
                ) : (
                  <ul>
                    {lista.slice(0, 5).map((solicitud) => (
                      <li key={solicitud.id} className="border-b border-line-soft last:border-0">
                        <Link to={`/solicitudes/${solicitud.id}`} className="flex items-center gap-4 px-5 py-3.5 transition hover:bg-ochre-soft/25 md:px-6">
                          <div className="min-w-0 flex-1">
                            <p className="truncate font-semibold">{solicitud.finca.nombre}</p>
                            <p className="truncate text-[13px] text-ink-soft">{solicitud.motivo}</p>
                          </div>
                          <EstadoBadge estado={solicitud.estado} />
                        </Link>
                      </li>
                    ))}
                  </ul>
                )
              }
            </QueryView>
          </Card>
        </div>
      </div>
    </>
  )
}

function InicioAdmin() {
  const hoy = hoyISO()
  const filtroAgenda = { desde: sumarDias(hoy, -1), hasta: sumarDias(hoy, 7) }
  const filtroPendientes = { estado: 'Pendiente' as const, pagina: 1, tamano: 5 }

  const pendientes = useQuery({ queryKey: solicitudesKeys.lista(filtroPendientes), queryFn: () => solicitudesApi.listar(filtroPendientes) })
  const agenda = useQuery({ queryKey: visitasKeys.agenda(filtroAgenda), queryFn: () => visitasApi.agenda(filtroAgenda) })
  const productores = useQuery({ queryKey: productoresKeys.lista({ tamano: 1 }), queryFn: () => productoresApi.listar({ tamano: 1 }) })
  const tecnicos = useQuery({ queryKey: tecnicosKeys.todos, queryFn: tecnicosApi.listar })

  const semana = agenda.data?.filter((visita) => claveDia(visita.fechaProgramada) >= hoy && visita.estado !== 'Cancelada')
  const deHoy = semana?.filter((visita) => claveDia(visita.fechaProgramada) === hoy)

  return (
    <>
      <Encabezado seccion="Administración" descripcion="Lo que espera atención hoy en tu organización." />
      <div className="stagger flex flex-col gap-6">
        <div className="grid grid-cols-2 gap-4 xl:grid-cols-4">
          <Indicador destacado etiqueta="Solicitudes pendientes" valor={pendientes.data?.total} a="/solicitudes" nota="Esperan técnico y fecha" />
          <Indicador etiqueta="Visitas hoy" valor={deHoy?.length} a="/agenda" nota={`${semana?.length ?? 0} en los próximos 7 días`} />
          <Indicador etiqueta="Productores" valor={productores.data?.total} a="/productores" />
          <Indicador
            etiqueta="Técnicos activos"
            valor={tecnicos.data?.filter((tecnico) => tecnico.activo).length}
            a="/tecnicos"
          />
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <Card>
            <CardHeader
              titulo="Por programar"
              descripcion="Solicitudes que todavía no tienen visita."
              accion={
                <Link to="/solicitudes" className="text-[13px] font-semibold text-forest hover:underline">
                  Ver todas
                </Link>
              }
            />
            <QueryView query={pendientes} filas={3}>
              {(datos) =>
                datos.items.length === 0 ? (
                  <div className="p-5 md:p-6">
                    <EmptyState compacto titulo="Todo al día" descripcion="No hay solicitudes pendientes de programar." />
                  </div>
                ) : (
                  <ul>
                    {datos.items.map((solicitud) => (
                      <li key={solicitud.id} className="border-b border-line-soft last:border-0">
                        <Link to={`/solicitudes/${solicitud.id}`} className="group flex items-center gap-4 px-5 py-3.5 transition hover:bg-ochre-soft/25 md:px-6">
                          <div className="min-w-0 flex-1">
                            <p className="truncate font-semibold">
                              {solicitud.productor.nombre} <span className="font-normal text-ink-soft">· {solicitud.finca.nombre}</span>
                            </p>
                            <p className="truncate text-[13px] text-ink-soft">{solicitud.motivo}</p>
                          </div>
                          <span className="font-mono text-xs whitespace-nowrap text-ink-soft">{formato.fecha(solicitud.fechaDeseada)}</span>
                          <ArrowRight className="size-4 text-ink-faint transition group-hover:translate-x-0.5 group-hover:text-forest" aria-hidden />
                        </Link>
                      </li>
                    ))}
                  </ul>
                )
              }
            </QueryView>
          </Card>

          <Card>
            <CardHeader titulo="Próximas visitas" descripcion="Los siguientes siete días." />
            <div className="flex flex-col gap-3 p-5 md:p-6">
              <QueryView query={agenda} filas={3}>
                {() =>
                  semana && semana.length > 0 ? (
                    semana.slice(0, 5).map((visita) => <VisitaTicket key={visita.id} visita={visita} />)
                  ) : (
                    <EmptyState compacto icono={<CalendarCheck className="size-5" />} titulo="Sin visitas esta semana" />
                  )
                }
              </QueryView>
            </div>
          </Card>
        </div>
      </div>
    </>
  )
}

function InicioTecnico() {
  const hoy = hoyISO()
  const visitas = useQuery({ queryKey: visitasKeys.mias({}), queryFn: () => visitasApi.mias({}) })

  const abiertas = visitas.data?.filter((visita) => visita.estado !== 'Completada' && visita.estado !== 'Cancelada')
  const deHoy = abiertas?.filter((visita) => claveDia(visita.fechaProgramada) === hoy)
  const siguientes = abiertas?.filter((visita) => claveDia(visita.fechaProgramada) > hoy)
  const enProceso = abiertas?.filter((visita) => visita.estado === 'EnProceso')

  return (
    <>
      <Encabezado seccion="Técnico" descripcion="Tu jornada de campo y las visitas que vienen." />
      <div className="stagger flex flex-col gap-6">
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3">
          <Indicador destacado etiqueta="Visitas hoy" valor={deHoy?.length} a="/visitas" />
          <Indicador etiqueta="En proceso" valor={enProceso?.length} a="/visitas" nota="Pendientes de informe o cierre" />
          <Indicador etiqueta="Próximas" valor={siguientes?.length} a="/visitas" />
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <Card>
            <CardHeader titulo="Hoy" descripcion={formato.diaSemana(hoy)} />
            <div className="flex flex-col gap-3 p-5 md:p-6">
              <QueryView query={visitas} filas={2}>
                {() =>
                  deHoy && deHoy.length > 0 ? (
                    deHoy.map((visita) => <VisitaTicket key={visita.id} visita={visita} mostrar="productor" />)
                  ) : (
                    <EmptyState compacto icono={<CalendarCheck className="size-5" />} titulo="No tienes visitas hoy" />
                  )
                }
              </QueryView>
            </div>
          </Card>
          <Card>
            <CardHeader titulo="Lo que viene" />
            <div className="flex flex-col gap-3 p-5 md:p-6">
              <QueryView query={visitas} filas={2}>
                {() =>
                  siguientes && siguientes.length > 0 ? (
                    siguientes.slice(0, 5).map((visita) => <VisitaTicket key={visita.id} visita={visita} mostrar="productor" />)
                  ) : (
                    <EmptyState compacto titulo="Sin visitas próximas" />
                  )
                }
              </QueryView>
            </div>
          </Card>
        </div>
      </div>
    </>
  )
}

export function InicioPage() {
  const usuario = useUsuario()

  if (usuario.rol === 'Productor') return <InicioProductor />
  if (usuario.rol === 'Tecnico') return <InicioTecnico />

  return <InicioAdmin />
}
