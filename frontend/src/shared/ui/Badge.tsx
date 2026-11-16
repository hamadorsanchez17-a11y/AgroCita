import clsx from 'clsx'
import type { ReactNode } from 'react'

export type Tono = 'ochre' | 'amber' | 'lake' | 'forest' | 'moss' | 'clay' | 'neutro'

const tonos: Record<Tono, string> = {
  ochre: 'border-ochre-deep/25 bg-ochre-soft text-ochre-deep',
  amber: 'border-amber/35 bg-amber-soft text-amber-deep',
  lake: 'border-lake/25 bg-lake-soft text-lake',
  forest: 'border-forest bg-forest text-paper',
  moss: 'border-moss/30 bg-moss-soft text-forest-light',
  clay: 'border-clay/30 bg-clay-soft text-clay',
  neutro: 'border-line bg-paper-deep text-ink-soft',
}

const puntos: Record<Tono, string> = {
  ochre: 'bg-ochre-deep',
  amber: 'bg-amber',
  lake: 'bg-lake',
  forest: 'bg-ochre',
  moss: 'bg-moss',
  clay: 'bg-clay',
  neutro: 'bg-ink-faint',
}

const vivos: Tono[] = ['amber', 'lake', 'forest']

export function Badge({ tono = 'neutro', children, punto = true }: { tono?: Tono; children: ReactNode; punto?: boolean }) {
  return (
    <span
      className={clsx(
        'inline-flex items-center gap-1.5 rounded-full border px-2.5 py-[3px] font-mono text-[10.5px] font-medium tracking-[0.06em] whitespace-nowrap uppercase',
        tonos[tono],
      )}
    >
      {punto && (
        <span className="relative grid size-1.5 place-items-center" aria-hidden>
          {vivos.includes(tono) && <span className={clsx('absolute size-1.5 animate-ping rounded-full opacity-60', puntos[tono])} />}
          <span className={clsx('size-1.5 rounded-full', puntos[tono])} />
        </span>
      )}
      {children}
    </span>
  )
}

const estados: Record<string, { etiqueta: string; tono: Tono }> = {
  Pendiente: { etiqueta: 'Pendiente', tono: 'amber' },
  EnAtencion: { etiqueta: 'En atención', tono: 'lake' },
  Atendida: { etiqueta: 'Atendida', tono: 'moss' },
  Rechazada: { etiqueta: 'Rechazada', tono: 'clay' },
  Confirmada: { etiqueta: 'Confirmada', tono: 'lake' },
  EnProceso: { etiqueta: 'En proceso', tono: 'forest' },
  Completada: { etiqueta: 'Completada', tono: 'moss' },
  Cancelada: { etiqueta: 'Cancelada', tono: 'clay' },
  Activa: { etiqueta: 'Activa', tono: 'moss' },
  Vencida: { etiqueta: 'Vencida', tono: 'amber' },
  Suspendida: { etiqueta: 'Suspendida', tono: 'clay' },
}

export function etiquetaEstado(estado: string): string {
  return estados[estado]?.etiqueta ?? estado
}

export function EstadoBadge({ estado }: { estado: string }) {
  const config = estados[estado] ?? { etiqueta: estado, tono: 'neutro' as Tono }

  return <Badge tono={config.tono}>{config.etiqueta}</Badge>
}
