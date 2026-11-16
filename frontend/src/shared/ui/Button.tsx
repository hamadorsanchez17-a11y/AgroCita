import clsx from 'clsx'
import { LoaderCircle } from 'lucide-react'
import type { ButtonHTMLAttributes, ReactNode } from 'react'

type Variante = 'primario' | 'acento' | 'secundario' | 'fantasma' | 'peligro'
type Tamano = 'sm' | 'md' | 'lg'

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variante?: Variante
  tamano?: Tamano
  cargando?: boolean
  icono?: ReactNode
  ancho?: boolean
}

const variantes: Record<Variante, string> = {
  primario:
    'bg-forest text-paper shadow-[inset_0_1px_0_rgb(255_255_255/0.12),0_1px_2px_rgb(8_23_15/0.3),0_8px_20px_-10px_rgb(8_23_15/0.6)] hover:bg-forest-light',
  acento:
    'bg-ochre text-forest-deep shadow-[inset_0_1px_0_rgb(255_255_255/0.5),0_1px_2px_rgb(8_23_15/0.15)] hover:shadow-glow',
  secundario:
    'bg-card text-ink border border-line shadow-[0_1px_2px_rgb(13_27_20/0.05)] hover:border-ink-faint/60 hover:bg-paper',
  fantasma: 'text-ink-soft hover:bg-paper-deep hover:text-ink',
  peligro:
    'bg-clay text-white shadow-[inset_0_1px_0_rgb(255_255_255/0.15),0_8px_20px_-10px_rgb(185_58_38/0.7)] hover:brightness-110',
}

const tamanos: Record<Tamano, string> = {
  sm: 'h-8 px-3 text-[13px] gap-1.5 rounded-lg',
  md: 'h-10 px-4 text-sm gap-2 rounded-xl',
  lg: 'h-12 px-6 text-[15px] gap-2.5 rounded-xl',
}

export function Button({
  variante = 'primario',
  tamano = 'md',
  cargando = false,
  icono,
  ancho = false,
  className,
  children,
  disabled,
  type = 'button',
  ...props
}: ButtonProps) {
  return (
    <button
      type={type}
      disabled={disabled || cargando}
      className={clsx(
        'group/boton relative isolate inline-flex shrink-0 cursor-pointer items-center justify-center overflow-hidden font-semibold tracking-[-0.01em] whitespace-nowrap select-none',
        'transition-[background-color,box-shadow,border-color,color,transform,filter] duration-200 ease-[var(--ease-salida)] active:scale-[0.97]',
        'disabled:cursor-not-allowed disabled:opacity-50 disabled:active:scale-100',
        variantes[variante],
        tamanos[tamano],
        ancho && 'w-full',
        className,
      )}
      {...props}
    >
      {(variante === 'primario' || variante === 'acento') && (
        <span
          className="pointer-events-none absolute inset-y-0 -left-full -z-10 w-1/2 -skew-x-12 bg-gradient-to-r from-transparent via-white/25 to-transparent transition-[left] duration-700 ease-[var(--ease-salida)] group-hover/boton:left-[150%]"
          aria-hidden
        />
      )}
      {cargando ? (
        <LoaderCircle className="size-4 animate-spin" aria-hidden />
      ) : (
        icono && <span className="inline-flex transition-transform duration-300 ease-[var(--ease-salida)] group-hover/boton:scale-110">{icono}</span>
      )}
      {children}
    </button>
  )
}
