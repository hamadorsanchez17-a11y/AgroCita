import { useQuery } from '@tanstack/react-query'
import { ArrowUpRight, CalendarX, Tractor } from 'lucide-react'
import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { ListaAplicaciones, ListaInspecciones, ListaRecomendaciones } from '@/features/calidad/Registros'
import { informesApi, informesKeys } from '@/features/informes/api'
import { VisitaTicket } from '@/features/visitas/VisitaTicket'
import { formato } from '@/shared/lib/format'
import { Select } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Dato, PageHeader, Segmented } from '@/shared/ui/layout'
import { productoresApi, productoresKeys } from './api'

type Pestana = 'visitas' | 'inspecciones' | 'recomendaciones' | 'aplicaciones'

function Historial({ productorId, fincas }: { productorId: number; fincas: { id: number; nombre: string }[] }) {
  const [pestana, setPestana] = useState<Pestana>('visitas')
  const [fincaId, setFincaId] = useState<number | undefined>()
  const filtro = { productorId, fincaId }

  const historial = useQuery({
    queryKey: informesKeys.historial(filtro),
    queryFn: () => informesApi.historial(filtro),
  })

  return (
    <Card>
      <CardHeader titulo="Historial de asistencia" numero="03" descripcion="Todo lo registrado en las fincas de este productor." />
      <div className="flex flex-col gap-4 border-b border-line-soft px-5 py-4 md:flex-row md:items-end md:justify-between md:px-6">
        <Segmented
          etiqueta="Tipo de registro"
          valor={pestana}
          alCambiar={setPestana}
          opciones={[
            { valor: 'visitas', etiqueta: `Visitas${historial.data ? ` · ${historial.data.visitas.length}` : ''}` },
            { valor: 'inspecciones', etiqueta: `Inspecciones${historial.data ? ` · ${historial.data.inspecciones.length}` : ''}` },
            { valor: 'recomendaciones', etiqueta: 'Recomendaciones' },
            { valor: 'aplicaciones', etiqueta: 'Aplicaciones' },
          ]}
        />
        <Select
          etiqueta="Finca"
          clase="md:w-56"
          value={fincaId ?? ''}
          onChange={(evento) => setFincaId(evento.target.value ? Number(evento.target.value) : undefined)}
        >
          <option value="">Todas las fincas</option>
          {fincas.map((finca) => (
            <option key={finca.id} value={finca.id}>
              {finca.nombre}
            </option>
          ))}
        </Select>
      </div>
      <div className="px-5 py-4 md:px-6">
        <QueryView query={historial} filas={3}>
          {(datos) => (
            <>
              {pestana === 'visitas' &&
                (datos.visitas.length === 0 ? (
                  <EmptyState compacto icono={<CalendarX className="size-5" />} titulo="Sin visitas registradas" />
                ) : (
                  <div className="grid gap-3 md:grid-cols-2">
                    {datos.visitas.map((visita) => (
                      <VisitaTicket key={visita.id} visita={visita} />
                    ))}
                  </div>
                ))}
              {pestana === 'inspecciones' && <ListaInspecciones items={datos.inspecciones} conCultivo />}
              {pestana === 'recomendaciones' && <ListaRecomendaciones items={datos.recomendaciones} />}
              {pestana === 'aplicaciones' && <ListaAplicaciones items={datos.aplicaciones} />}
            </>
          )}
        </QueryView>
      </div>
    </Card>
  )
}

export function ProductorDetallePage() {
  const id = Number(useParams().id)
  const productor = useQuery({ queryKey: productoresKeys.detalle(id), queryFn: () => productoresApi.obtener(id) })

  return (
    <QueryView query={productor}>
      {(datos) => (
        <>
          <PageHeader
            seccion="Productor"
            titulo={`${datos.nombre} ${datos.apellido}`}
            volver={{ a: '/productores', texto: 'Productores' }}
          />

          <div className="stagger flex flex-col gap-6">
            <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.5fr)]">
              <Card className="h-fit">
                <CardHeader titulo="Contacto" numero="01" />
                <dl className="grid gap-5 px-5 py-5 sm:grid-cols-2 md:px-6">
                  <Dato etiqueta="DNI" mono>
                    {datos.dni}
                  </Dato>
                  <Dato etiqueta="Teléfono" mono>
                    {datos.telefono}
                  </Dato>
                  <div className="sm:col-span-2">
                    <Dato etiqueta="Correo">{datos.email}</Dato>
                  </div>
                  <div className="sm:col-span-2">
                    <Dato etiqueta="Dirección">{datos.direccion ?? '—'}</Dato>
                  </div>
                </dl>
              </Card>

              <Card>
                <CardHeader titulo="Fincas" numero="02" descripcion={`${datos.fincas.length} registradas`} />
                {datos.fincas.length === 0 ? (
                  <div className="p-5 md:p-6">
                    <EmptyState compacto icono={<Tractor className="size-5" />} titulo="Aún no registra fincas" />
                  </div>
                ) : (
                  <ul>
                    {datos.fincas.map((finca) => (
                      <li key={finca.id} className="border-b border-line-soft last:border-0">
                        <Link to={`/fincas/${finca.id}`} className="group flex items-center gap-4 px-5 py-4 transition hover:bg-ochre-soft/25 md:px-6">
                          <div className="min-w-0 flex-1">
                            <p className="text-[17px] font-semibold">{finca.nombre}</p>
                            <p className="text-[13px] text-ink-soft">
                              {finca.municipio}, {finca.departamento}
                            </p>
                          </div>
                          <span className="font-mono text-[13px] text-ink-soft">{formato.area(finca.areaManzanas)}</span>
                          <span className="font-mono text-[13px] text-ink-soft">{finca.cantidadCultivos} cult.</span>
                          <ArrowUpRight className="size-4 text-ink-faint transition group-hover:text-forest" aria-hidden />
                        </Link>
                      </li>
                    ))}
                  </ul>
                )}
              </Card>
            </div>

            <Historial productorId={datos.id} fincas={datos.fincas} />
          </div>
        </>
      )}
    </QueryView>
  )
}
