import type { UseQueryResult } from '@tanstack/react-query'
import clsx from 'clsx'
import { CloudOff, RotateCw, Sprout } from 'lucide-react'
import { motion } from 'motion/react'
import type { ReactNode } from 'react'
import { extractApiError } from '@/shared/api/errors'
import { Button } from './Button'
import { Contours } from './layout'
import { Marca } from './Marca'

export function Skeleton({ className }: { className?: string }) {
  return (
    <div
      className={clsx(
        'animate-brillo rounded-xl bg-[linear-gradient(90deg,var(--color-paper-deep)_25%,var(--color-line-soft)_50%,var(--color-paper-deep)_75%)] bg-[length:200%_100%]',
        className,
      )}
    />
  )
}

export function Cargando({ filas = 4 }: { filas?: number }) {
  return (
    <div className="flex flex-col gap-3" aria-busy="true" aria-label="Cargando">
      {Array.from({ length: filas }, (_, indice) => (
        <Skeleton key={indice} className="h-16" />
      ))}
    </div>
  )
}

export function PantallaCargando() {
  return (
    <div className="grid min-h-dvh place-items-center bg-forest-deep">
      <motion.div
        className="flex flex-col items-center gap-5"
        initial={{ opacity: 0, scale: 0.96 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.5 }}
      >
        <Marca claro />
        <div className="h-0.5 w-32 overflow-hidden rounded-full bg-paper/10">
          <motion.div
            className="h-full w-1/2 rounded-full bg-ochre"
            animate={{ x: ['-100%', '200%'] }}
            transition={{ duration: 1.2, repeat: Infinity, ease: 'easeInOut' }}
          />
        </div>
      </motion.div>
    </div>
  )
}

export function ErrorPanel({ error, alReintentar }: { error: unknown; alReintentar?: () => void }) {
  const api = extractApiError(error)

  return (
    <div className="animate-pop flex flex-col items-center gap-3 rounded-[22px] border border-clay/25 bg-clay-soft/40 px-6 py-12 text-center">
      <span className="grid size-12 place-items-center rounded-full bg-card text-clay shadow-sheet">
        <CloudOff className="size-5" aria-hidden />
      </span>
      <div>
        <p className="font-display text-xl font-semibold tracking-tight">{api.title}</p>
        <p className="mt-1 text-sm text-ink-soft">{api.message}</p>
      </div>
      {alReintentar && (
        <Button variante="secundario" tamano="sm" icono={<RotateCw className="size-3.5" />} onClick={alReintentar}>
          Reintentar
        </Button>
      )}
    </div>
  )
}

interface EmptyStateProps {
  icono?: ReactNode
  titulo: string
  descripcion?: string
  accion?: ReactNode
  compacto?: boolean
}

export function EmptyState({ icono, titulo, descripcion, accion, compacto = false }: EmptyStateProps) {
  return (
    <div
      className={clsx(
        'animate-pop relative flex flex-col items-center overflow-hidden rounded-[20px] border border-dashed border-line bg-paper/40 text-center',
        compacto ? 'gap-2 px-5 py-9' : 'gap-3 px-6 py-16',
      )}
    >
      <Contours
        className={clsx(
          'pointer-events-none absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 text-moss/25',
          compacto ? 'size-[340px]' : 'size-[560px]',
        )}
      />
      <motion.div
        className="relative grid size-12 place-items-center rounded-2xl bg-forest text-ochre shadow-[0_10px_24px_-10px_rgb(8_23_15/0.6)]"
        animate={{ y: [0, -4, 0] }}
        transition={{ duration: 4, repeat: Infinity, ease: 'easeInOut' }}
      >
        {icono ?? <Sprout className="size-5" aria-hidden />}
      </motion.div>
      <div className="relative">
        <p className={clsx('font-display font-semibold tracking-[-0.03em]', compacto ? 'text-lg' : 'text-2xl')}>{titulo}</p>
        {descripcion && <p className="mx-auto mt-1 max-w-sm text-sm text-ink-soft">{descripcion}</p>}
      </div>
      {accion && <div className="relative mt-1">{accion}</div>}
    </div>
  )
}

interface QueryViewProps<T> {
  query: UseQueryResult<T>
  children: (datos: T) => ReactNode
  filas?: number
}

export function QueryView<T>({ query, children, filas }: QueryViewProps<T>) {
  if (query.isPending) return <Cargando filas={filas} />
  if (query.isError) return <ErrorPanel error={query.error} alReintentar={() => void query.refetch()} />

  return <>{children(query.data)}</>
}
