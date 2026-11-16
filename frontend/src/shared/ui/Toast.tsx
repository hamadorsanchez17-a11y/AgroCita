import clsx from 'clsx'
import { CircleAlert, CircleCheck, X } from 'lucide-react'
import { AnimatePresence, motion } from 'motion/react'
import { createContext, useCallback, useContext, useMemo, useRef, useState } from 'react'
import type { ReactNode } from 'react'
import { createPortal } from 'react-dom'
import { extractErrorMessage } from '@/shared/api/errors'

interface Aviso {
  id: number
  tipo: 'ok' | 'error'
  mensaje: string
}

interface ToastApi {
  ok: (mensaje: string) => void
  error: (error: unknown) => void
}

const ToastContext = createContext<ToastApi | null>(null)

export function ToastProvider({ children }: { children: ReactNode }) {
  const [avisos, setAvisos] = useState<Aviso[]>([])
  const contador = useRef(0)

  const quitar = useCallback((id: number) => {
    setAvisos((actuales) => actuales.filter((aviso) => aviso.id !== id))
  }, [])

  const agregar = useCallback(
    (tipo: Aviso['tipo'], mensaje: string) => {
      const id = ++contador.current
      setAvisos((actuales) => [...actuales.slice(-3), { id, tipo, mensaje }])
      window.setTimeout(() => quitar(id), tipo === 'ok' ? 3800 : 6500)
    },
    [quitar],
  )

  const api = useMemo<ToastApi>(
    () => ({
      ok: (mensaje) => agregar('ok', mensaje),
      error: (error) => agregar('error', typeof error === 'string' ? error : extractErrorMessage(error)),
    }),
    [agregar],
  )

  return (
    <ToastContext.Provider value={api}>
      {children}
      {createPortal(
        <div className="pointer-events-none fixed inset-x-4 bottom-4 z-[60] flex flex-col items-end gap-2 sm:left-auto sm:w-[380px]" aria-live="polite">
          <AnimatePresence initial={false}>
          {avisos.map((aviso) => (
            <motion.div
              key={aviso.id}
              role="status"
              layout
              initial={{ opacity: 0, y: 24, scale: 0.94 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, x: 40, scale: 0.96 }}
              transition={{ type: 'spring', stiffness: 420, damping: 34 }}
              className={clsx(
                'pointer-events-auto flex w-full items-start gap-3 rounded-2xl border px-4 py-3.5 text-sm shadow-lift backdrop-blur',
                aviso.tipo === 'ok' ? 'border-forest-light bg-forest/95 text-paper' : 'border-clay/40 bg-card/95 text-ink',
              )}
            >
              {aviso.tipo === 'ok' ? (
                <CircleCheck className="mt-0.5 size-4 shrink-0 text-ochre" aria-hidden />
              ) : (
                <CircleAlert className="mt-0.5 size-4 shrink-0 text-clay" aria-hidden />
              )}
              <p className="flex-1 leading-snug">{aviso.mensaje}</p>
              <button type="button" onClick={() => quitar(aviso.id)} aria-label="Descartar" className="cursor-pointer opacity-60 transition hover:opacity-100">
                <X className="size-4" />
              </button>
            </motion.div>
          ))}
          </AnimatePresence>
        </div>,
        document.body,
      )}
    </ToastContext.Provider>
  )
}

export function useToast(): ToastApi {
  const contexto = useContext(ToastContext)

  if (!contexto) throw new Error('useToast debe usarse dentro de ToastProvider')

  return contexto
}
