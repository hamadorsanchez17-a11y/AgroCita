import clsx from 'clsx'
import { X } from 'lucide-react'
import { motion } from 'motion/react'
import { useEffect } from 'react'
import type { ReactNode } from 'react'
import { createPortal } from 'react-dom'

interface ModalProps {
  abierto: boolean
  alCerrar: () => void
  titulo: string
  descripcion?: string
  ancho?: 'sm' | 'md' | 'lg'
  children: ReactNode
}

const anchos = { sm: 'max-w-md', md: 'max-w-xl', lg: 'max-w-3xl' }

export function Modal({ abierto, alCerrar, titulo, descripcion, ancho = 'md', children }: ModalProps) {
  useEffect(() => {
    if (!abierto) return

    const alPulsar = (evento: KeyboardEvent) => {
      if (evento.key === 'Escape') alCerrar()
    }

    document.addEventListener('keydown', alPulsar)
    document.body.style.overflow = 'hidden'

    return () => {
      document.removeEventListener('keydown', alPulsar)
      document.body.style.overflow = ''
    }
  }, [abierto, alCerrar])

  if (!abierto) return null

  return createPortal(
    <div className="fixed inset-0 z-50 flex items-end justify-center sm:items-center sm:p-6">
      <motion.div
        className="absolute inset-0 bg-forest-deep/50 backdrop-blur-md"
        onClick={alCerrar}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.25 }}
        aria-hidden
      />
      <motion.div
        role="dialog"
        aria-modal="true"
        aria-label={titulo}
        initial={{ opacity: 0, y: 28, scale: 0.96 }}
        animate={{ opacity: 1, y: 0, scale: 1 }}
        transition={{ type: 'spring', stiffness: 380, damping: 32 }}
        className={clsx(
          'relative flex max-h-[92dvh] w-full flex-col overflow-hidden rounded-t-[28px] border border-line bg-card shadow-lift sm:rounded-[28px]',
          anchos[ancho],
        )}
      >
        <span className="absolute inset-x-0 top-0 h-1 bg-gradient-to-r from-forest via-moss to-ochre" aria-hidden />
        <header className="flex items-start justify-between gap-4 border-b border-line-soft px-6 pt-7 pb-4">
          <div>
            <h2 className="text-[26px] leading-tight font-semibold">{titulo}</h2>
            {descripcion && <p className="mt-1 text-sm text-ink-soft">{descripcion}</p>}
          </div>
          <button
            type="button"
            onClick={alCerrar}
            aria-label="Cerrar"
            className="-mt-1 -mr-2 grid size-9 shrink-0 cursor-pointer place-items-center rounded-full text-ink-soft transition duration-300 hover:rotate-90 hover:bg-paper-deep hover:text-ink"
          >
            <X className="size-5" />
          </button>
        </header>
        <div className="overflow-y-auto px-6 py-5">{children}</div>
      </motion.div>
    </div>,
    document.body,
  )
}

export function ModalActions({ children }: { children: ReactNode }) {
  return <div className="mt-6 flex flex-col-reverse gap-2.5 sm:flex-row sm:justify-end">{children}</div>
}
