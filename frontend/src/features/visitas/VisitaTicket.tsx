import { Clock, MapPin, UserRound } from 'lucide-react'
import { Link } from 'react-router-dom'
import type { Referencia } from '@/shared/api/http'
import { formato } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { seguirCursor } from '@/shared/ui/layout'
import type { EstadoVisita } from './api'

interface VisitaTicketProps {
  visita: {
    id: number
    fechaProgramada: string
    finca: Referencia
    tecnico: Referencia
    estado: EstadoVisita
    productor?: Referencia
  }
  mostrar?: 'tecnico' | 'productor'
}

export function VisitaTicket({ visita, mostrar = 'tecnico' }: VisitaTicketProps) {
  const persona = mostrar === 'productor' && visita.productor ? visita.productor : visita.tecnico

  return (
    <Link
      to={`/visitas/${visita.id}`}
      onMouseMove={seguirCursor}
      className="group relative flex overflow-hidden rounded-[20px] border border-line/80 bg-card shadow-sheet transition duration-300 ease-[var(--ease-salida)] hover:-translate-y-1 hover:border-forest/25 hover:shadow-lift"
    >
      <div className="foco pointer-events-none absolute inset-0 opacity-0 transition-opacity duration-300 group-hover:opacity-100" aria-hidden />
      <span className="absolute -top-[7px] left-[73px] z-10 size-3.5 rounded-full border border-line bg-paper" aria-hidden />
      <span className="absolute -bottom-[7px] left-[73px] z-10 size-3.5 rounded-full border border-line bg-paper" aria-hidden />
      <div className="relative flex w-[80px] shrink-0 flex-col items-center justify-center border-r border-dashed border-line bg-paper-deep/60 px-2 py-4 transition-colors duration-300 group-hover:border-forest group-hover:bg-forest">
        <span className="font-mono text-[10.5px] tracking-[0.14em] text-ochre-deep uppercase transition-colors duration-300 group-hover:text-ochre">{formato.mes(visita.fechaProgramada)}</span>
        <span className="font-display text-[2rem] leading-none font-semibold tracking-[-0.04em] transition-colors duration-300 group-hover:text-paper">{formato.dia(visita.fechaProgramada)}</span>
        <span className="mt-1 text-[11px] text-ink-faint capitalize transition-colors duration-300 group-hover:text-paper/60">{formato.diaSemana(visita.fechaProgramada).slice(0, 3)}</span>
      </div>
      <div className="relative flex min-w-0 flex-1 flex-col justify-center gap-2 px-4 py-3.5">
        <div className="flex items-start justify-between gap-3">
          <p className="truncate text-[17px] leading-tight font-semibold">{visita.finca.nombre}</p>
          <EstadoBadge estado={visita.estado} />
        </div>
        <div className="flex flex-wrap gap-x-4 gap-y-1 text-[13px] text-ink-soft">
          <span className="inline-flex items-center gap-1.5 font-mono">
            <Clock className="size-3.5 text-moss" aria-hidden />
            {formato.hora(visita.fechaProgramada)}
          </span>
          <span className="inline-flex items-center gap-1.5">
            {mostrar === 'productor' ? (
              <MapPin className="size-3.5 text-moss" aria-hidden />
            ) : (
              <UserRound className="size-3.5 text-moss" aria-hidden />
            )}
            {persona.nombre}
          </span>
        </div>
      </div>
    </Link>
  )
}
