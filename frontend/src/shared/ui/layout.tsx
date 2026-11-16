import clsx from 'clsx'
import { ArrowLeft } from 'lucide-react'
import { motion } from 'motion/react'
import type { MouseEvent, ReactNode } from 'react'
import { useId } from 'react'
import { Link } from 'react-router-dom'

const salida = [0.22, 1, 0.36, 1] as const

export function seguirCursor(evento: MouseEvent<HTMLElement>) {
  const caja = evento.currentTarget.getBoundingClientRect()
  evento.currentTarget.style.setProperty('--x', `${evento.clientX - caja.left}px`)
  evento.currentTarget.style.setProperty('--y', `${evento.clientY - caja.top}px`)
}

interface PageHeaderProps {
  seccion: string
  titulo: ReactNode
  descripcion?: ReactNode
  acciones?: ReactNode
  volver?: { a: string; texto: string }
}

export function PageHeader({ seccion, titulo, descripcion, acciones, volver }: PageHeaderProps) {
  return (
    <header className="relative mb-10 flex flex-col gap-6 pb-7 md:flex-row md:items-end md:justify-between">
      <div className="min-w-0">
        {volver && (
          <motion.div initial={{ opacity: 0, x: -8 }} animate={{ opacity: 1, x: 0 }} transition={{ duration: 0.4, ease: salida }}>
            <Link
              to={volver.a}
              className="group mb-4 inline-flex items-center gap-1.5 rounded-full border border-line bg-card/70 py-1 pr-3 pl-2 text-[12.5px] font-semibold text-ink-soft backdrop-blur transition hover:border-forest/30 hover:text-forest"
            >
              <ArrowLeft className="size-3.5 transition-transform duration-300 group-hover:-translate-x-0.5" aria-hidden />
              {volver.texto}
            </Link>
          </motion.div>
        )}
        <motion.p
          className="eyebrow mb-3 flex items-center gap-2"
          initial={{ opacity: 0, y: 6 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, ease: salida }}
        >
          <span className="size-1.5 rounded-full bg-ochre shadow-[0_0_0_3px_rgb(197_240_90/0.3)]" aria-hidden />
          {seccion}
        </motion.p>
        <motion.h1
          className="text-[2.3rem] leading-[1] font-semibold text-balance md:text-[3.1rem]"
          initial={{ opacity: 0, y: 22, filter: 'blur(8px)' }}
          animate={{ opacity: 1, y: 0, filter: 'blur(0px)' }}
          transition={{ duration: 0.8, ease: salida, delay: 0.05 }}
        >
          {titulo}
        </motion.h1>
        {descripcion && (
          <motion.div
            className="mt-3.5 max-w-2xl text-[15.5px] leading-relaxed text-ink-soft"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, ease: salida, delay: 0.15 }}
          >
            {descripcion}
          </motion.div>
        )}
      </div>
      {acciones && (
        <motion.div
          className="flex shrink-0 flex-wrap items-center gap-2.5"
          initial={{ opacity: 0, y: 10 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, ease: salida, delay: 0.2 }}
        >
          {acciones}
        </motion.div>
      )}
      <motion.span
        className="absolute inset-x-0 bottom-0 h-px origin-left bg-gradient-to-r from-ink/25 via-line to-transparent"
        initial={{ scaleX: 0 }}
        animate={{ scaleX: 1 }}
        transition={{ duration: 1.1, ease: salida, delay: 0.1 }}
        aria-hidden
      />
    </header>
  )
}

export function Card({ children, className }: { children: ReactNode; className?: string }) {
  return (
    <section
      className={clsx(
        'rounded-[22px] border border-line/80 bg-card shadow-sheet ring-1 ring-white/60 ring-inset',
        className,
      )}
    >
      {children}
    </section>
  )
}

interface CardHeaderProps {
  titulo: string
  descripcion?: string
  accion?: ReactNode
  numero?: string
}

export function CardHeader({ titulo, descripcion, accion, numero }: CardHeaderProps) {
  return (
    <div className="flex items-start justify-between gap-4 border-b border-line-soft px-5 py-4 md:px-6">
      <div className="flex min-w-0 items-start gap-3">
        {numero && (
          <span className="mt-0.5 rounded-md bg-forest px-1.5 py-0.5 font-mono text-[10.5px] font-medium text-ochre">{numero}</span>
        )}
        <div className="min-w-0">
          <h2 className="text-[19px] leading-tight font-semibold tracking-[-0.025em]">{titulo}</h2>
          {descripcion && <p className="mt-0.5 text-[13px] text-ink-soft">{descripcion}</p>}
        </div>
      </div>
      {accion && <div className="shrink-0">{accion}</div>}
    </div>
  )
}

export function Dato({ etiqueta, children, mono = false }: { etiqueta: string; children: ReactNode; mono?: boolean }) {
  return (
    <div className="min-w-0">
      <dt className="eyebrow mb-1.5">{etiqueta}</dt>
      <dd className={clsx('text-[15px] break-words text-ink', mono && 'font-mono text-sm')}>{children}</dd>
    </div>
  )
}

interface SegmentedProps<T extends string> {
  opciones: { valor: T; etiqueta: string }[]
  valor: T
  alCambiar: (valor: T) => void
  etiqueta: string
}

export function Segmented<T extends string>({ opciones, valor, alCambiar, etiqueta }: SegmentedProps<T>) {
  const grupo = useId()

  return (
    <div role="tablist" aria-label={etiqueta} className="inline-flex max-w-full gap-1 overflow-x-auto rounded-full border border-line bg-card/80 p-1 shadow-[0_1px_2px_rgb(13_27_20/0.04)] backdrop-blur">
      {opciones.map((opcion) => {
        const activa = opcion.valor === valor

        return (
          <button
            key={opcion.valor}
            type="button"
            role="tab"
            aria-selected={activa}
            onClick={() => alCambiar(opcion.valor)}
            className={clsx(
              'relative cursor-pointer rounded-full px-4 py-1.5 text-[13px] font-semibold whitespace-nowrap transition-colors duration-200',
              activa ? 'text-paper' : 'text-ink-soft hover:text-ink',
            )}
          >
            {activa && (
              <motion.span
                layoutId={`segmento-${grupo}`}
                className="absolute inset-0 rounded-full bg-forest shadow-[0_6px_16px_-8px_rgb(8_23_15/0.7)]"
                transition={{ type: 'spring', stiffness: 420, damping: 34 }}
                aria-hidden
              />
            )}
            <span className="relative">{opcion.etiqueta}</span>
          </button>
        )
      })}
    </div>
  )
}

export function Contours({ className, animado = false }: { className?: string; animado?: boolean }) {
  return (
    <svg viewBox="0 0 600 600" fill="none" className={clsx(className, animado && 'animate-deriva')} aria-hidden>
      {[40, 78, 118, 160, 204, 250, 298].map((radio, indice) => (
        <path
          key={radio}
          d={`M ${300 - radio * 1.15} ${300 + indice * 4}
              C ${300 - radio * 1.1} ${300 - radio * 0.95}, ${300 - radio * 0.25} ${300 - radio * 1.05 - indice * 3}, ${300 + radio * 0.4} ${300 - radio * 0.8}
              S ${300 + radio * 1.25} ${300 - radio * 0.1}, ${300 + radio * 1.05} ${300 + radio * 0.55}
              S ${300 + radio * 0.1} ${300 + radio * 1.1 + indice * 2}, ${300 - radio * 0.55} ${300 + radio * 0.85}
              S ${300 - radio * 1.2} ${300 + radio * 0.35}, ${300 - radio * 1.15} ${300 + indice * 4} Z`}
          stroke="currentColor"
          strokeWidth="1"
          pathLength={animado ? 1 : undefined}
          strokeDasharray={animado ? 1 : undefined}
          className={animado ? 'animate-trazo' : undefined}
          style={animado ? { animationDelay: `${indice * 140}ms` } : undefined}
        />
      ))}
    </svg>
  )
}
