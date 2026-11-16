import clsx from 'clsx'
import { ChevronLeft, ChevronRight } from 'lucide-react'
import type { ReactNode } from 'react'

export interface Columna<T> {
  clave: string
  titulo: string
  celda: (fila: T) => ReactNode
  clase?: string
  derecha?: boolean
}

interface DataTableProps<T> {
  columnas: Columna<T>[]
  filas: T[]
  idDe: (fila: T) => string | number
  alClic?: (fila: T) => void
}

export function DataTable<T>({ columnas, filas, idDe, alClic }: DataTableProps<T>) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full min-w-[560px] border-collapse text-left text-[14.5px]">
        <thead>
          <tr className="border-b border-line bg-paper/60">
            {columnas.map((columna) => (
              <th
                key={columna.clave}
                scope="col"
                className={clsx('eyebrow px-5 py-3.5 md:px-6', columna.derecha && 'text-right')}
              >
                {columna.titulo}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {filas.map((fila) => (
            <tr
              key={idDe(fila)}
              onClick={alClic ? () => alClic(fila) : undefined}
              onKeyDown={
                alClic
                  ? (evento) => {
                      if (evento.key === 'Enter') alClic(fila)
                    }
                  : undefined
              }
              tabIndex={alClic ? 0 : undefined}
              className={clsx(
                'border-b border-line-soft transition-colors duration-200 last:border-0',
                alClic && 'cursor-pointer hover:bg-ochre-soft/50 focus-visible:bg-ochre-soft/50',
              )}
            >
              {columnas.map((columna) => (
                <td
                  key={columna.clave}
                  className={clsx('px-5 py-3.5 align-middle md:px-6', columna.derecha && 'text-right', columna.clase)}
                >
                  {columna.celda(fila)}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

interface PaginationProps {
  pagina: number
  tamano: number
  total: number
  alCambiar: (pagina: number) => void
}

export function Pagination({ pagina, tamano, total, alCambiar }: PaginationProps) {
  const paginas = Math.max(1, Math.ceil(total / tamano))
  const desde = total === 0 ? 0 : (pagina - 1) * tamano + 1
  const hasta = Math.min(total, pagina * tamano)

  const boton =
    'grid size-8 cursor-pointer place-items-center rounded-lg border border-line bg-card text-ink-soft transition hover:border-ink-faint hover:text-ink disabled:cursor-not-allowed disabled:opacity-40'

  return (
    <div className="flex items-center justify-between gap-4 border-t border-line-soft px-5 py-3 md:px-6">
      <p className="font-mono text-xs text-ink-faint">
        {desde}–{hasta} de {total}
      </p>
      <div className="flex items-center gap-2">
        <button type="button" className={boton} disabled={pagina <= 1} onClick={() => alCambiar(pagina - 1)} aria-label="Página anterior">
          <ChevronLeft className="size-4" />
        </button>
        <span className="font-mono text-xs text-ink-soft">
          {pagina} / {paginas}
        </span>
        <button type="button" className={boton} disabled={pagina >= paginas} onClick={() => alCambiar(pagina + 1)} aria-label="Página siguiente">
          <ChevronRight className="size-4" />
        </button>
      </div>
    </div>
  )
}
