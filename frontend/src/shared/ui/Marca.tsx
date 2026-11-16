import clsx from 'clsx'

export function Marca({ claro = false, compacto = false }: { claro?: boolean; compacto?: boolean }) {
  return (
    <div className="flex items-center gap-2.5">
      <svg viewBox="0 0 32 32" className={compacto ? 'size-8' : 'size-10'} aria-hidden>
        <rect width="32" height="32" rx="10" className={claro ? 'fill-ochre' : 'fill-forest'} />
        <path d="M16 25V13" className={claro ? 'stroke-forest-deep' : 'stroke-paper'} strokeWidth="2.4" strokeLinecap="round" />
        <path d="M16 15c0-4.4 3-7 7.5-7 0 4.6-3 7.4-7.5 7Z" className={claro ? 'fill-forest-deep' : 'fill-ochre'} />
        <path d="M16 19c0-3.4-2.4-5.4-6-5.4 0 3.6 2.4 5.8 6 5.4Z" className={claro ? 'fill-forest-light' : 'fill-moss'} />
      </svg>
      <span
        className={clsx(
          'font-display leading-none font-semibold tracking-[-0.04em]',
          compacto ? 'text-[22px]' : 'text-[26px]',
          claro ? 'text-paper' : 'text-forest',
        )}
      >
        Agro<span className="italic">Cita</span>
      </span>
    </div>
  )
}
