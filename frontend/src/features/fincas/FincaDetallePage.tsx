import { useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import { ChevronDown, ClipboardPlus, Pencil, Plus, Wheat } from 'lucide-react'
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { HistorialCultivo } from '@/features/calidad/HistorialCultivo'
import { useAuth } from '@/shared/auth/AuthContext'
import { formato } from '@/shared/lib/format'
import { Badge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Dato, PageHeader } from '@/shared/ui/layout'
import { fincasApi, fincasKeys } from './api'
import type { Cultivo } from './api'
import { CultivoForm } from './CultivoForm'
import { FincaForm } from './FincaForm'

export function FincaDetallePage() {
  const id = Number(useParams().id)
  const { tieneRol } = useAuth()
  const esProductor = tieneRol('Productor')
  const finca = useQuery({ queryKey: fincasKeys.detalle(id), queryFn: () => fincasApi.obtener(id) })

  const [editando, setEditando] = useState(false)
  const [cultivoForm, setCultivoForm] = useState<{ cultivo?: Cultivo } | null>(null)
  const [abierto, setAbierto] = useState<number | null>(null)

  return (
    <QueryView query={finca}>
      {(datos) => (
        <>
          <PageHeader
            seccion={`Finca · ${datos.municipio}, ${datos.departamento}`}
            titulo={datos.nombre}
            descripcion={datos.descripcion ?? undefined}
            volver={
              esProductor
                ? { a: '/fincas', texto: 'Mis fincas' }
                : tieneRol('AdminEmpresa')
                  ? { a: `/productores/${datos.productor.id}`, texto: datos.productor.nombre }
                  : undefined
            }
            acciones={
              esProductor && (
                <>
                  <Button variante="secundario" icono={<Pencil className="size-4" />} onClick={() => setEditando(true)}>
                    Editar
                  </Button>
                  <Link to={`/solicitudes?finca=${datos.id}`}>
                    <Button variante="acento" icono={<ClipboardPlus className="size-4" />}>
                      Solicitar asistencia
                    </Button>
                  </Link>
                </>
              )
            }
          />

          <div className="stagger grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,2fr)]">
            <Card className="h-fit">
              <CardHeader titulo="Ficha" numero="01" />
              <dl className="grid gap-5 px-5 py-5 md:px-6">
                <Dato etiqueta="Productor">{datos.productor.nombre}</Dato>
                <Dato etiqueta="Ubicación">
                  {datos.direccion}
                  <span className="block text-sm text-ink-soft">
                    {datos.municipio}, {datos.departamento}
                  </span>
                </Dato>
                <Dato etiqueta="Área total" mono>
                  {formato.area(datos.areaManzanas)}
                </Dato>
              </dl>
            </Card>

            <Card>
              <CardHeader
                titulo="Cultivos"
                numero="02"
                descripcion="Abre un cultivo para ver sus inspecciones y fertilización."
                accion={
                  esProductor && (
                    <Button tamano="sm" icono={<Plus className="size-3.5" />} onClick={() => setCultivoForm({})}>
                      Agregar
                    </Button>
                  )
                }
              />
              {datos.cultivos.length === 0 ? (
                <div className="p-5 md:p-6">
                  <EmptyState
                    compacto
                    icono={<Wheat className="size-5" />}
                    titulo="Esta finca no tiene cultivos"
                    descripcion={esProductor ? 'Agrega al menos uno para poder solicitar asistencia.' : undefined}
                  />
                </div>
              ) : (
                <ul>
                  {datos.cultivos.map((cultivo) => {
                    const expandido = abierto === cultivo.id

                    return (
                      <li key={cultivo.id} className="border-b border-line-soft last:border-0">
                        <div className="flex items-center gap-3 px-5 py-4 md:px-6">
                          <button
                            type="button"
                            onClick={() => setAbierto(expandido ? null : cultivo.id)}
                            aria-expanded={expandido}
                            className="flex min-w-0 flex-1 cursor-pointer items-center gap-4 text-left"
                          >
                            <span className="grid size-10 shrink-0 place-items-center rounded-full bg-moss-soft text-forest-light">
                              <Wheat className="size-[18px]" aria-hidden />
                            </span>
                            <span className="min-w-0 flex-1">
                              <span className="block truncate text-[17px] font-semibold">{cultivo.nombre}</span>
                              <span className="mt-0.5 flex flex-wrap items-center gap-x-3 gap-y-1 text-[13px] text-ink-soft">
                                <Badge punto={false}>{cultivo.tipoCultivo}</Badge>
                                {cultivo.variedad && <span>{cultivo.variedad}</span>}
                                <span className="font-mono">{formato.area(cultivo.areaManzanas)}</span>
                                {cultivo.fechaSiembra && <span>Siembra: {formato.fecha(cultivo.fechaSiembra)}</span>}
                              </span>
                            </span>
                            <ChevronDown className={clsx('size-5 shrink-0 text-ink-faint transition', expandido && 'rotate-180')} aria-hidden />
                          </button>
                          {esProductor && (
                            <button
                              type="button"
                              onClick={() => setCultivoForm({ cultivo })}
                              aria-label={`Editar ${cultivo.nombre}`}
                              className="grid size-9 shrink-0 cursor-pointer place-items-center rounded-lg text-ink-soft transition hover:bg-paper-deep hover:text-ink"
                            >
                              <Pencil className="size-4" />
                            </button>
                          )}
                        </div>
                        {expandido && <HistorialCultivo cultivoId={cultivo.id} />}
                      </li>
                    )
                  })}
                </ul>
              )}
            </Card>
          </div>

          {editando && <FincaForm finca={datos} alCerrar={() => setEditando(false)} />}
          {cultivoForm && <CultivoForm fincaId={datos.id} cultivo={cultivoForm.cultivo} alCerrar={() => setCultivoForm(null)} />}
        </>
      )}
    </QueryView>
  )
}
