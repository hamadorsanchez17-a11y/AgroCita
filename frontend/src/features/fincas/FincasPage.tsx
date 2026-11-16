import { useQuery } from '@tanstack/react-query'
import { ArrowUpRight, MapPin, Plus, Tractor } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { formato } from '@/shared/lib/format'
import { Button } from '@/shared/ui/Button'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { PageHeader } from '@/shared/ui/layout'
import { fincasApi, fincasKeys } from './api'
import { FincaForm } from './FincaForm'

export function FincasPage() {
  const fincas = useQuery({ queryKey: fincasKeys.todas, queryFn: fincasApi.listar })
  const [creando, setCreando] = useState(false)

  return (
    <>
      <PageHeader
        seccion="Productor · Propiedades"
        titulo="Mis fincas"
        descripcion="Registra tus propiedades y los cultivos de cada una para poder solicitar asistencia técnica."
        acciones={
          <Button icono={<Plus className="size-4" />} onClick={() => setCreando(true)}>
            Registrar finca
          </Button>
        }
      />

      <QueryView query={fincas}>
        {(lista) =>
          lista.length === 0 ? (
            <EmptyState
              icono={<Tractor className="size-5" />}
              titulo="Todavía no tienes fincas"
              descripcion="Registra tu primera finca para agregar sus cultivos y pedir una visita."
              accion={
                <Button variante="acento" icono={<Plus className="size-4" />} onClick={() => setCreando(true)}>
                  Registrar finca
                </Button>
              }
            />
          ) : (
            <div className="stagger grid gap-5 md:grid-cols-2 xl:grid-cols-3">
              {lista.map((finca, indice) => (
                <Link
                  key={finca.id}
                  to={`/fincas/${finca.id}`}
                  className="group relative flex flex-col overflow-hidden rounded-2xl border border-line bg-card shadow-sheet transition duration-200 hover:-translate-y-1 hover:border-forest/40 hover:shadow-lift"
                >
                  <div className="ruled flex items-start justify-between px-5 pt-5 pb-7">
                    <span className="font-mono text-xs text-ochre-deep">N.º {String(indice + 1).padStart(2, '0')}</span>
                    <ArrowUpRight className="size-5 text-ink-faint transition group-hover:translate-x-0.5 group-hover:-translate-y-0.5 group-hover:text-forest" aria-hidden />
                  </div>
                  <div className="flex flex-1 flex-col px-5 pb-5">
                    <h2 className="text-2xl leading-tight font-semibold">{finca.nombre}</h2>
                    <p className="mt-2 flex items-start gap-1.5 text-sm text-ink-soft">
                      <MapPin className="mt-0.5 size-3.5 shrink-0 text-moss" aria-hidden />
                      {finca.municipio}, {finca.departamento}
                    </p>
                    <dl className="mt-5 grid grid-cols-2 gap-4 border-t border-line-soft pt-4">
                      <div>
                        <dt className="eyebrow">Área</dt>
                        <dd className="mt-1 font-mono text-lg">{formato.area(finca.areaManzanas)}</dd>
                      </div>
                      <div>
                        <dt className="eyebrow">Cultivos</dt>
                        <dd className="mt-1 font-mono text-lg">{finca.cantidadCultivos}</dd>
                      </div>
                    </dl>
                  </div>
                </Link>
              ))}
            </div>
          )
        }
      </QueryView>

      {creando && <FincaForm alCerrar={() => setCreando(false)} />}
    </>
  )
}
