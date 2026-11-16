import { Droplets, FlaskConical, Microscope } from 'lucide-react'
import type { ReactNode } from 'react'
import { formato } from '@/shared/lib/format'
import { Badge } from '@/shared/ui/Badge'
import { EmptyState } from '@/shared/ui/feedback'
import type { Aplicacion, Inspeccion, Recomendacion } from './api'

function Registro({ fecha, children }: { fecha: string; children: ReactNode }) {
  return (
    <li className="grid grid-cols-[64px_1fr] gap-4 border-b border-line-soft py-4 last:border-0 sm:grid-cols-[84px_1fr]">
      <div className="text-right">
        <p className="font-display text-2xl leading-none">{formato.dia(fecha)}</p>
        <p className="mt-1 font-mono text-[11px] tracking-wider text-ink-faint uppercase">
          {formato.mes(fecha)} {formato.anio(fecha)}
        </p>
      </div>
      <div className="min-w-0 border-l border-line pl-4">{children}</div>
    </li>
  )
}

export function ListaInspecciones({ items, conCultivo = false }: { items: Inspeccion[]; conCultivo?: boolean }) {
  if (items.length === 0) {
    return (
      <EmptyState
        compacto
        icono={<Microscope className="size-5" />}
        titulo="Sin inspecciones"
        descripcion="Las inspecciones aparecen aquí cuando el técnico las registra durante una visita."
      />
    )
  }

  return (
    <ul>
      {items.map((inspeccion) => (
        <Registro key={inspeccion.id} fecha={inspeccion.fecha}>
          <div className="flex flex-wrap items-center gap-2">
            {conCultivo && <p className="font-semibold">{inspeccion.cultivo.nombre}</p>}
            {inspeccion.categoriaCalidad && <Badge tono="moss">{inspeccion.categoriaCalidad.nombre}</Badge>}
            <span className="text-[13px] text-ink-faint">por {inspeccion.tecnico.nombre}</span>
          </div>
          {inspeccion.observaciones && <p className="mt-2 text-[15px] leading-relaxed text-ink-soft">{inspeccion.observaciones}</p>}
          {inspeccion.resultados.length > 0 && (
            <dl className="mt-3 flex flex-wrap gap-2">
              {inspeccion.resultados.map((resultado) => (
                <div key={resultado.parametroId} className="rounded-lg border border-line bg-paper px-3 py-1.5">
                  <dt className="text-[11px] text-ink-faint">{resultado.parametro}</dt>
                  <dd className="font-mono text-sm">
                    {resultado.valor}
                    {resultado.unidad && <span className="ml-1 text-ink-faint">{resultado.unidad}</span>}
                  </dd>
                </div>
              ))}
            </dl>
          )}
        </Registro>
      ))}
    </ul>
  )
}

export function ListaRecomendaciones({ items }: { items: Recomendacion[] }) {
  if (items.length === 0) {
    return (
      <EmptyState
        compacto
        icono={<FlaskConical className="size-5" />}
        titulo="Sin recomendaciones"
        descripcion="Aquí verás lo que el técnico recomienda aplicar al cultivo."
      />
    )
  }

  return (
    <ul>
      {items.map((recomendacion) => (
        <Registro key={recomendacion.id} fecha={recomendacion.fecha}>
          <p className="font-semibold">
            {recomendacion.fertilizante.nombre}
            <span className="ml-2 font-mono text-sm font-normal text-ochre-deep">
              {recomendacion.dosis} {recomendacion.unidad}
            </span>
          </p>
          <p className="mt-1.5 text-[15px] leading-relaxed text-ink-soft">{recomendacion.indicaciones}</p>
          <p className="mt-2 text-[13px] text-ink-faint">
            {recomendacion.frecuencia && <>{recomendacion.frecuencia} · </>}
            {recomendacion.tecnico.nombre}
          </p>
        </Registro>
      ))}
    </ul>
  )
}

export function ListaAplicaciones({ items }: { items: Aplicacion[] }) {
  if (items.length === 0) {
    return (
      <EmptyState
        compacto
        icono={<Droplets className="size-5" />}
        titulo="Sin aplicaciones"
        descripcion="Aquí queda el registro de cada abono efectivamente aplicado."
      />
    )
  }

  return (
    <ul>
      {items.map((aplicacion) => (
        <Registro key={aplicacion.id} fecha={aplicacion.fechaAplicacion}>
          <div className="flex flex-wrap items-center gap-2">
            <p className="font-semibold">{aplicacion.fertilizante.nombre}</p>
            <span className="font-mono text-sm text-ochre-deep">
              {aplicacion.cantidad} {aplicacion.unidad}
            </span>
            {aplicacion.recomendacionId && <Badge tono="lake">Según recomendación</Badge>}
          </div>
          {aplicacion.observaciones && <p className="mt-1.5 text-[15px] leading-relaxed text-ink-soft">{aplicacion.observaciones}</p>}
          <p className="mt-2 text-[13px] text-ink-faint">{aplicacion.tecnico.nombre}</p>
        </Registro>
      ))}
    </ul>
  )
}
