import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { ClipboardList, Plus } from 'lucide-react'
import { useState } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '@/shared/auth/AuthContext'
import { formato } from '@/shared/lib/format'
import { EstadoBadge, etiquetaEstado } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { Input, Select } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, PageHeader, Segmented } from '@/shared/ui/layout'
import { DataTable, Pagination } from '@/shared/ui/Table'
import { ESTADOS_SOLICITUD, solicitudesApi, solicitudesKeys } from './api'
import type { EstadoSolicitud, Solicitud } from './api'
import { SolicitudForm } from './SolicitudForm'

type Filtro = EstadoSolicitud | 'Todas'

const OPCIONES: { valor: Filtro; etiqueta: string }[] = [
  { valor: 'Todas', etiqueta: 'Todas' },
  ...ESTADOS_SOLICITUD.map((estado) => ({ valor: estado as Filtro, etiqueta: etiquetaEstado(estado) })),
]

function SolicitudesProductor() {
  const [parametros, setParametros] = useSearchParams()
  const fincaInicial = Number(parametros.get('finca')) || undefined
  const [filtro, setFiltro] = useState<Filtro>('Todas')
  const [creando, setCreando] = useState(Boolean(fincaInicial))
  const estado = filtro === 'Todas' ? undefined : filtro

  const solicitudes = useQuery({
    queryKey: solicitudesKeys.mias(estado),
    queryFn: () => solicitudesApi.mias(estado),
  })

  const cerrar = () => {
    setCreando(false)
    if (fincaInicial) setParametros({}, { replace: true })
  }

  return (
    <>
      <PageHeader
        seccion="Productor · Asistencia"
        titulo="Mis solicitudes"
        descripcion="Pide una visita técnica para tus fincas y sigue su avance."
        acciones={
          <Button variante="acento" icono={<Plus className="size-4" />} onClick={() => setCreando(true)}>
            Nueva solicitud
          </Button>
        }
      />

      <div className="mb-5">
        <Segmented etiqueta="Filtrar por estado" opciones={OPCIONES} valor={filtro} alCambiar={setFiltro} />
      </div>

      <QueryView query={solicitudes}>
        {(lista) =>
          lista.length === 0 ? (
            <EmptyState
              icono={<ClipboardList className="size-5" />}
              titulo={filtro === 'Todas' ? 'Aún no has pedido asistencia' : 'No hay solicitudes con ese estado'}
              descripcion="Cuando envíes una solicitud, tu organización la revisa y programa la visita."
            />
          ) : (
            <ul className="stagger grid gap-4 md:grid-cols-2">
              {lista.map((solicitud) => (
                <li key={solicitud.id}>
                  <Link
                    to={`/solicitudes/${solicitud.id}`}
                    className="flex h-full flex-col rounded-2xl border border-line bg-card p-5 shadow-sheet transition duration-200 hover:-translate-y-0.5 hover:border-forest/40 hover:shadow-lift"
                  >
                    <div className="flex items-start justify-between gap-3">
                      <span className="font-mono text-xs text-ochre-deep">SOL-{String(solicitud.id).padStart(4, '0')}</span>
                      <EstadoBadge estado={solicitud.estado} />
                    </div>
                    <h2 className="mt-3 text-xl leading-tight font-semibold">{solicitud.finca.nombre}</h2>
                    <p className="mt-1.5 line-clamp-2 text-[15px] leading-relaxed text-ink-soft">{solicitud.motivo}</p>
                    <div className="mt-4 flex flex-wrap items-center gap-x-4 gap-y-1 border-t border-line-soft pt-3 text-[13px] text-ink-soft">
                      <span>Para el {formato.fecha(solicitud.fechaDeseada)}</span>
                      <span>{solicitud.cultivos.map((cultivo) => cultivo.nombre).join(' · ')}</span>
                    </div>
                  </Link>
                </li>
              ))}
            </ul>
          )
        }
      </QueryView>

      {creando && <SolicitudForm fincaInicial={fincaInicial} alCerrar={cerrar} />}
    </>
  )
}

function SolicitudesAdmin() {
  const navegar = useNavigate()
  const [estado, setEstado] = useState<EstadoSolicitud | ''>('')
  const [desde, setDesde] = useState('')
  const [hasta, setHasta] = useState('')
  const [pagina, setPagina] = useState(1)

  const filtro = { estado: estado || undefined, desde: desde || undefined, hasta: hasta || undefined, pagina, tamano: 10 }

  const solicitudes = useQuery({
    queryKey: solicitudesKeys.lista(filtro),
    queryFn: () => solicitudesApi.listar(filtro),
    placeholderData: keepPreviousData,
  })

  return (
    <>
      <PageHeader
        seccion="Administración · Asistencia"
        titulo="Solicitudes"
        descripcion="Revisa lo que piden los productores, programa las visitas y actualiza su estado."
      />

      <Card>
        <div className="grid gap-4 border-b border-line-soft px-5 py-4 sm:grid-cols-3 md:px-6">
          <Select
            etiqueta="Estado"
            value={estado}
            onChange={(evento) => {
              setEstado(evento.target.value as EstadoSolicitud | '')
              setPagina(1)
            }}
          >
            <option value="">Todos</option>
            {ESTADOS_SOLICITUD.map((opcion) => (
              <option key={opcion} value={opcion}>
                {etiquetaEstado(opcion)}
              </option>
            ))}
          </Select>
          <Input
            etiqueta="Fecha deseada desde"
            type="date"
            className="font-mono"
            value={desde}
            onChange={(evento) => {
              setDesde(evento.target.value)
              setPagina(1)
            }}
          />
          <Input
            etiqueta="Hasta"
            type="date"
            className="font-mono"
            value={hasta}
            min={desde || undefined}
            onChange={(evento) => {
              setHasta(evento.target.value)
              setPagina(1)
            }}
          />
        </div>

        <QueryView query={solicitudes}>
          {(datos) =>
            datos.items.length === 0 ? (
              <div className="p-6">
                <EmptyState compacto icono={<ClipboardList className="size-5" />} titulo="No hay solicitudes con esos filtros" />
              </div>
            ) : (
              <>
                <DataTable<Solicitud>
                  filas={datos.items}
                  idDe={(fila) => fila.id}
                  alClic={(fila) => navegar(`/solicitudes/${fila.id}`)}
                  columnas={[
                    {
                      clave: 'codigo',
                      titulo: 'N.º',
                      celda: (fila) => <span className="font-mono text-xs text-ochre-deep">SOL-{String(fila.id).padStart(4, '0')}</span>,
                    },
                    {
                      clave: 'productor',
                      titulo: 'Productor y finca',
                      celda: (fila) => (
                        <>
                          <p className="font-semibold">{fila.productor.nombre}</p>
                          <p className="text-[13px] text-ink-soft">{fila.finca.nombre}</p>
                        </>
                      ),
                    },
                    {
                      clave: 'motivo',
                      titulo: 'Motivo',
                      clase: 'max-w-[280px]',
                      celda: (fila) => <p className="line-clamp-2 text-ink-soft">{fila.motivo}</p>,
                    },
                    {
                      clave: 'fecha',
                      titulo: 'Fecha deseada',
                      celda: (fila) => <span className="font-mono text-[13px] whitespace-nowrap">{formato.fecha(fila.fechaDeseada)}</span>,
                    },
                    { clave: 'estado', titulo: 'Estado', celda: (fila) => <EstadoBadge estado={fila.estado} /> },
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

export function SolicitudesPage() {
  const { tieneRol } = useAuth()

  return tieneRol('Productor') ? <SolicitudesProductor /> : <SolicitudesAdmin />
}
