import clsx from 'clsx'
import { CircleAlert } from 'lucide-react'
import { useId } from 'react'
import type { InputHTMLAttributes, ReactNode, Ref, SelectHTMLAttributes, TextareaHTMLAttributes } from 'react'

interface BaseProps {
  etiqueta: string
  error?: string
  ayuda?: string
  opcional?: boolean
  clase?: string
}

const control =
  'w-full rounded-xl border bg-card px-3.5 text-[15px] text-ink shadow-[0_1px_2px_rgb(13_27_20/0.04)] placeholder:text-ink-faint/70 transition duration-200 ' +
  'hover:border-ink-faint/60 focus:border-forest focus:bg-white focus:outline-none focus:ring-4 focus:ring-ochre/35 ' +
  'disabled:cursor-not-allowed disabled:bg-paper-deep disabled:text-ink-faint'

function Envoltura({
  id,
  etiqueta,
  error,
  ayuda,
  opcional,
  clase,
  children,
}: BaseProps & { id: string; children: ReactNode }) {
  return (
    <div className={clsx('flex flex-col gap-1.5', clase)}>
      <label htmlFor={id} className="flex items-baseline justify-between text-[13px] font-semibold text-ink-soft">
        <span>{etiqueta}</span>
        {opcional && <span className="font-mono text-[10px] tracking-wider text-ink-faint uppercase">Opcional</span>}
      </label>
      {children}
      {error ? (
        <p id={`${id}-error`} role="alert" className="flex items-start gap-1.5 text-[13px] leading-snug text-clay">
          <CircleAlert className="mt-px size-3.5 shrink-0" aria-hidden />
          {error}
        </p>
      ) : (
        ayuda && <p className="text-[13px] leading-snug text-ink-faint">{ayuda}</p>
      )}
    </div>
  )
}

function estado(error?: string) {
  return error ? 'border-clay bg-clay-soft/30 focus:border-clay focus:ring-clay-soft' : 'border-line'
}

type InputProps = BaseProps & InputHTMLAttributes<HTMLInputElement> & { ref?: Ref<HTMLInputElement> }

export function Input({ etiqueta, error, ayuda, opcional, clase, className, ...props }: InputProps) {
  const id = useId()

  return (
    <Envoltura id={id} etiqueta={etiqueta} error={error} ayuda={ayuda} opcional={opcional} clase={clase}>
      <input
        id={id}
        aria-invalid={Boolean(error)}
        aria-describedby={error ? `${id}-error` : undefined}
        className={clsx(control, 'h-11', estado(error), className)}
        {...props}
      />
    </Envoltura>
  )
}

type SelectProps = BaseProps & SelectHTMLAttributes<HTMLSelectElement> & { ref?: Ref<HTMLSelectElement> }

export function Select({ etiqueta, error, ayuda, opcional, clase, className, children, ...props }: SelectProps) {
  const id = useId()

  return (
    <Envoltura id={id} etiqueta={etiqueta} error={error} ayuda={ayuda} opcional={opcional} clase={clase}>
      <select
        id={id}
        aria-invalid={Boolean(error)}
        aria-describedby={error ? `${id}-error` : undefined}
        className={clsx(control, 'h-11 cursor-pointer appearance-none bg-no-repeat pr-10', estado(error), className)}
        style={{
          backgroundImage:
            "url(\"data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='none' stroke='%234b5849' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='m4 6 4 4 4-4'/%3E%3C/svg%3E\")",
          backgroundPosition: 'right 0.85rem center',
        }}
        {...props}
      >
        {children}
      </select>
    </Envoltura>
  )
}

type TextareaProps = BaseProps & TextareaHTMLAttributes<HTMLTextAreaElement> & { ref?: Ref<HTMLTextAreaElement> }

export function Textarea({ etiqueta, error, ayuda, opcional, clase, className, rows = 4, ...props }: TextareaProps) {
  const id = useId()

  return (
    <Envoltura id={id} etiqueta={etiqueta} error={error} ayuda={ayuda} opcional={opcional} clase={clase}>
      <textarea
        id={id}
        rows={rows}
        aria-invalid={Boolean(error)}
        aria-describedby={error ? `${id}-error` : undefined}
        className={clsx(control, 'resize-y py-2.5 leading-relaxed', estado(error), className)}
        {...props}
      />
    </Envoltura>
  )
}

export function FormError({ mensaje }: { mensaje?: string | null }) {
  if (!mensaje) return null

  return (
    <div
      role="alert"
      className="animate-fade flex items-start gap-2.5 rounded-[10px] border border-clay/40 bg-clay-soft/50 px-3.5 py-3 text-sm text-clay"
    >
      <CircleAlert className="mt-0.5 size-4 shrink-0" aria-hidden />
      <span>{mensaje}</span>
    </div>
  )
}
