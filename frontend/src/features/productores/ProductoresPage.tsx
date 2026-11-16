import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { Search, Users } from 'lucide-react'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { formato } from '@/shared/lib/format'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, PageHeader } from '@/shared/ui/layout'
import { DataTable, Pagination } from '@/shared/ui/Table'
import { productoresApi, productoresKeys } from './api'
import type { ProductorResumen } from './api'

export function ProductoresPage() {
  const navegar = useNavigate()
  const [texto, setTexto] = useState('')
  const [buscar, setBuscar] = useState('')
  const [pagina, setPagina] = useState(1)

  useEffect(() => {
    const temporizador = window.setTimeout(() => {
      setBuscar(texto.trim())
      setPagina(1)
    }, 300)

    return () => window.clearTimeout(temporizador)
  }, [texto])

  const filtro = { buscar: buscar || undefined, pagina, tamano: 10 }

  const productores = useQuery({
    queryKey: productoresKeys.lista(filtro),
    queryFn: () => productoresApi.listar(filtro),
    placeholderData: keepPreviousData,
  })

  return (
    <>
      <PageHeader
        seccion="Administración · Directorio"
        titulo="Productores"
        descripcion="Los productores que se registraron con el código de invitación de tu organización."
      />

      <Card>
        <div className="border-b border-line-soft px-5 py-4 md:px-6">
          <label className="relative block max-w-md">
            <span className="sr-only">Buscar productor</span>
            <Search className="pointer-events-none absolute top-1/2 left-3.5 size-4 -translate-y-1/2 text-ink-faint" aria-hidden />
            <input
              type="search"
              value={texto}
              onChange={(evento) => setTexto(evento.target.value)}
              placeholder="Buscar por nombre o DNI"
              className="h-11 w-full rounded-[10px] border border-line bg-white/70 pr-3.5 pl-10 text-[15px] transition placeholder:text-ink-faint/70 hover:border-ink-faint focus:border-forest focus:bg-white focus:ring-4 focus:ring-moss-soft focus:outline-none"
            />
          </label>
        </div>

        <QueryView query={productores}>
          {(datos) =>
            datos.items.length === 0 ? (
              <div className="p-6">
                <EmptyState
                  compacto
                  icono={<Users className="size-5" />}
                  titulo={buscar ? 'Nadie coincide con la búsqueda' : 'Todavía no hay productores'}
                  descripcion={buscar ? undefined : 'Comparte el código de invitación desde Organización para que se registren.'}
                />
              </div>
            ) : (
              <>
                <DataTable<ProductorResumen>
                  filas={datos.items}
                  idDe={(fila) => fila.id}
                  alClic={(fila) => navegar(`/productores/${fila.id}`)}
                  columnas={[
                    {
                      clave: 'nombre',
                      titulo: 'Productor',
                      celda: (fila) => (
                        <span className="flex items-center gap-3">
                          <span className="grid size-9 shrink-0 place-items-center rounded-full bg-moss-soft font-display text-sm text-forest-light">
                            {formato.iniciales(fila.nombreCompleto)}
                          </span>
                          <span className="font-semibold">{fila.nombreCompleto}</span>
                        </span>
                      ),
                    },
                    { clave: 'dni', titulo: 'DNI', celda: (fila) => <span className="font-mono text-[13px]">{fila.dni}</span> },
                    { clave: 'telefono', titulo: 'Teléfono', celda: (fila) => <span className="font-mono text-[13px]">{fila.telefono}</span> },
                    { clave: 'fincas', titulo: 'Fincas', derecha: true, celda: (fila) => <span className="font-mono">{fila.cantidadFincas}</span> },
                  ]}
                />
                <Pagination pagina={datos.pagina} tamano={datos.tamano} total={datos.total} alCambiar={setPagina} />
              </>
            )
          }
        </QueryView>
      </Card>
    </>
  )
}
