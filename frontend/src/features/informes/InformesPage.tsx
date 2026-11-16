import { useQuery } from '@tanstack/react-query'
import { ArrowUpRight, FileText } from 'lucide-react'
import { Link } from 'react-router-dom'
import { formato } from '@/shared/lib/format'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { PageHeader } from '@/shared/ui/layout'
import { informesApi, informesKeys } from './api'

export function InformesPage() {
  const informes = useQuery({ queryKey: informesKeys.mios, queryFn: informesApi.mios })

  return (
    <>
      <PageHeader
        seccion="Productor · Seguimiento"
        titulo="Informes técnicos"
        descripcion="Los informes que los técnicos entregaron después de visitar tus fincas."
      />

      <QueryView query={informes}>
        {(lista) =>
          lista.length === 0 ? (
            <EmptyState
              icono={<FileText className="size-5" />}
              titulo="Todavía no tienes informes"
              descripcion="Aparecerán aquí cuando un técnico termine una visita a tu finca."
            />
          ) : (
            <ol className="stagger overflow-hidden rounded-2xl border border-line bg-card shadow-sheet">
              {lista.map((informe) => (
                <li key={informe.id} className="border-b border-line-soft last:border-0">
                  <Link
                    to={`/visitas/${informe.visitaId}`}
                    className="group grid grid-cols-[72px_1fr_auto] items-center gap-4 px-5 py-4 transition hover:bg-ochre-soft/25 md:px-6"
                  >
                    <div className="text-center">
                      <p className="font-display text-3xl leading-none">{formato.dia(informe.fecha)}</p>
                      <p className="mt-1 font-mono text-[11px] tracking-wider text-ink-faint uppercase">{formato.mes(informe.fecha)}</p>
                    </div>
                    <div className="min-w-0 border-l border-line pl-4">
                      <p className="truncate text-lg font-semibold">{informe.finca.nombre}</p>
                      <p className="text-sm text-ink-soft">Elaborado por {informe.tecnico.nombre}</p>
                    </div>
                    <ArrowUpRight className="size-5 text-ink-faint transition group-hover:text-forest" aria-hidden />
                  </Link>
                </li>
              ))}
            </ol>
          )
        }
      </QueryView>
    </>
  )
}
