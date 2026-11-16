import { keepPreviousData, useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import { ChevronLeft, ChevronRight } from 'lucide-react'
import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { tecnicosApi, tecnicosKeys } from '@/features/tecnicos/api'
import { claveDia, formato, hoyISO, sumarDias } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { Select } from '@/shared/ui/Field'
import { ErrorPanel } from '@/shared/ui/feedback'
import { PageHeader } from '@/shared/ui/layout'
import { visitasApi, visitasKeys } from './api'
import type { VisitaAgenda } from './api'

function lunesDe(fecha: string): string {
  const [anio, mes, dia] = fecha.split('-').map(Number)
  const diaSemana = new Date(anio, mes - 1, dia).getDay()

  return sumarDias(fecha, diaSemana === 0 ? -6 : 1 - diaSemana)
}

export function AgendaPage() {
  const hoy = hoyISO()
  const [inicio, setInicio] = useState(() => lunesDe(hoy))
  const [tecnicoId, setTecnicoId] = useState<number | undefined>()

  const dias = useMemo(() => Array.from({ length: 7 }, (_, indice) => sumarDias(inicio, indice)), [inicio])
  const filtro = { desde: sumarDias(inicio, -1), hasta: sumarDias(inicio, 7), tecnicoId }

  const tecnicos = useQuery({ queryKey: tecnicosKeys.todos, queryFn: tecnicosApi.listar })
  const agenda = useQuery({
    queryKey: visitasKeys.agenda(filtro),
    queryFn: () => visitasApi.agenda(filtro),
    placeholderData: keepPreviousData,
  })

  const porDia = useMemo(() => {
    const mapa = new Map<string, VisitaAgenda[]>()

    for (const visita of agenda.data ?? []) {
      const clave = claveDia(visita.fechaProgramada)
      mapa.set(clave, [...(mapa.get(clave) ?? []), visita])
    }

    return mapa
  }, [agenda.data])

  const total = dias.reduce((suma, dia) => suma + (porDia.get(dia)?.length ?? 0), 0)

  return (
    <>
      <PageHeader
        seccion="Administración · Agenda"
        titulo={
          <>
            Semana del {formato.dia(dias[0])} al {formato.dia(dias[6])}{' '}
            <span className="font-light text-ink-soft italic">de {formato.mes(dias[6])}</span>
          </>
        }
        descripcion={`${total} ${total === 1 ? 'visita programada' : 'visitas programadas'} en la semana. Las visitas se crean desde cada solicitud.`}
        acciones={
          <>
            <Button variante="secundario" tamano="md" aria-label="Semana anterior" onClick={() => setInicio(sumarDias(inicio, -7))}>
              <ChevronLeft className="size-4" />
            </Button>
            <Button variante="secundario" onClick={() => setInicio(lunesDe(hoy))}>
              Hoy
            </Button>
            <Button variante="secundario" tamano="md" aria-label="Semana siguiente" onClick={() => setInicio(sumarDias(inicio, 7))}>
              <ChevronRight className="size-4" />
            </Button>
          </>
        }
      />

      <div className="mb-5 max-w-xs">
        <Select
          etiqueta="Técnico"
          value={tecnicoId ?? ''}
          onChange={(evento) => setTecnicoId(evento.target.value ? Number(evento.target.value) : undefined)}
        >
          <option value="">Todos los técnicos</option>
          {tecnicos.data?.map((tecnico) => (
            <option key={tecnico.id} value={tecnico.id}>
              {tecnico.nombre} {tecnico.apellido}
            </option>
          ))}
        </Select>
      </div>

      {agenda.isError ? (
        <ErrorPanel error={agenda.error} alReintentar={() => void agenda.refetch()} />
      ) : (
        <div
          className={clsx(
            'animate-rise grid overflow-hidden rounded-2xl border border-line bg-card shadow-sheet lg:grid-cols-7',
            agenda.isFetching && 'opacity-70',
          )}
        >
          {dias.map((dia) => {
            const visitas = porDia.get(dia) ?? []
            const esHoy = dia === hoy

            return (
              <section
                key={dia}
                className={clsx(
                  'flex min-h-0 flex-col border-b border-line-soft last:border-0 lg:min-h-[420px] lg:border-r lg:border-b-0',
                  esHoy && 'bg-ochre-soft/25',
                )}
              >
                <header className="flex items-baseline gap-2 border-b border-line-soft px-3.5 py-3 lg:flex-col lg:gap-0">
                  <span className={clsx('eyebrow', esHoy && '!text-ochre-deep')}>{formato.diaSemana(dia).slice(0, 3)}</span>
                  <span className={clsx('font-display text-2xl leading-none', esHoy && 'text-ochre-deep')}>{formato.dia(dia)}</span>
                </header>
                <div className="flex flex-1 flex-col gap-2 p-2">
                  {visitas.length === 0 ? (
                    <p className="px-1.5 py-1 text-[12px] text-ink-faint lg:py-3">Sin visitas</p>
                  ) : (
                    visitas.map((visita) => (
                      <Link
                        key={visita.id}
                        to={`/visitas/${visita.id}`}
                        className={clsx(
                          'rounded-xl border border-line bg-paper px-2.5 py-2 transition hover:border-forest hover:bg-white hover:shadow-sm',
                          visita.estado === 'Cancelada' && 'opacity-55',
                        )}
                      >
                        <p className="font-mono text-[11.5px] text-ochre-deep">{formato.hora(visita.fechaProgramada)}</p>
                        <p className="mt-0.5 text-[13.5px] leading-tight font-semibold">{visita.finca.nombre}</p>
                        <p className="mt-0.5 truncate text-[12px] text-ink-soft">{visita.tecnico.nombre}</p>
                        <div className="mt-1.5">
                          <EstadoBadge estado={visita.estado} />
                        </div>
                      </Link>
                    ))
                  )}
                </div>
              </section>
            )
          })}
        </div>
      )}
    </>
  )
}
