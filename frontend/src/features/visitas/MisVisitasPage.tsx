import { useQuery } from '@tanstack/react-query'
import { CalendarCheck } from 'lucide-react'
import { useState } from 'react'
import { etiquetaEstado } from '@/shared/ui/Badge'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { PageHeader, Segmented } from '@/shared/ui/layout'
import { ESTADOS_VISITA, visitasApi, visitasKeys } from './api'
import type { EstadoVisita, Visita } from './api'
import { VisitaTicket } from './VisitaTicket'

type Filtro = EstadoVisita | 'Todas'

const OPCIONES: { valor: Filtro; etiqueta: string }[] = [
  { valor: 'Todas', etiqueta: 'Todas' },
  ...ESTADOS_VISITA.map((estado) => ({ valor: estado as Filtro, etiqueta: etiquetaEstado(estado) })),
]

function Grupo({ titulo, visitas }: { titulo: string; visitas: Visita[] }) {
  if (visitas.length === 0) return null

  return (
    <section>
      <h2 className="eyebrow mb-3 flex items-center gap-3">
        {titulo}
        <span className="h-px flex-1 bg-line" aria-hidden />
        <span>{visitas.length}</span>
      </h2>
      <div className="grid gap-3 md:grid-cols-2">
        {visitas.map((visita) => (
          <VisitaTicket key={visita.id} visita={visita} mostrar="productor" />
        ))}
      </div>
    </section>
  )
}

export function MisVisitasPage() {
  const [filtro, setFiltro] = useState<Filtro>('Todas')
  const [ahora] = useState(() => Date.now())
  const parametros = { estado: filtro === 'Todas' ? undefined : filtro }

  const visitas = useQuery({
    queryKey: visitasKeys.mias(parametros),
    queryFn: () => visitasApi.mias(parametros),
  })

  return (
    <>
      <PageHeader
        seccion="Técnico · Trabajo de campo"
        titulo="Mis visitas"
        descripcion="Las visitas que tienes asignadas, con la finca, la hora y su estado."
      />

      <div className="mb-6">
        <Segmented etiqueta="Filtrar por estado" opciones={OPCIONES} valor={filtro} alCambiar={setFiltro} />
      </div>

      <QueryView query={visitas}>
        {(lista) => {
          if (lista.length === 0) {
            return (
              <EmptyState
                icono={<CalendarCheck className="size-5" />}
                titulo="No tienes visitas aquí"
                descripcion="Cuando el administrador te asigne una visita aparecerá en esta lista."
              />
            )
          }

          const abiertas = lista.filter((visita) => visita.estado !== 'Completada' && visita.estado !== 'Cancelada')
          const proximas = abiertas.filter((visita) => new Date(visita.fechaProgramada).getTime() >= ahora)
          const atrasadas = abiertas.filter((visita) => new Date(visita.fechaProgramada).getTime() < ahora)
          const cerradas = lista.filter((visita) => !abiertas.includes(visita)).reverse()

          return (
            <div className="stagger flex flex-col gap-8">
              <Grupo titulo="Por atender" visitas={atrasadas} />
              <Grupo titulo="Próximas" visitas={proximas} />
              <Grupo titulo="Cerradas" visitas={cerradas} />
            </div>
          )
        }}
      </QueryView>
    </>
  )
}
