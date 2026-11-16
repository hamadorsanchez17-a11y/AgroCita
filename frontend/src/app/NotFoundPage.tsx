import { Link } from 'react-router-dom'
import { Button } from '@/shared/ui/Button'
import { Contours } from '@/shared/ui/layout'

export function NotFoundPage() {
  return (
    <div className="relative grid min-h-dvh place-items-center overflow-hidden px-6">
      <Contours className="pointer-events-none absolute size-[900px] text-moss/25" />
      <div className="animate-rise relative text-center">
        <p className="eyebrow">Error 404</p>
        <h1 className="mt-3 text-6xl font-light">
          Este lote <em>no existe</em>
        </h1>
        <p className="mx-auto mt-4 max-w-sm text-ink-soft">La página que buscas no está en el mapa o ya no está disponible.</p>
        <Link to="/" className="mt-8 inline-block">
          <Button tamano="lg">Volver al inicio</Button>
        </Link>
      </div>
    </div>
  )
}
