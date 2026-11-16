import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { Building2, Plus } from 'lucide-react'
import { useState } from 'react'
import { ESTADOS_SUSCRIPCION } from '@/features/organizacion/api'
import type { Empresa, EstadoSuscripcion } from '@/features/organizacion/api'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useAccion, useFormMutation } from '@/shared/forms/mutations'
import { hoyISO } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Select } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, PageHeader } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { DataTable, Pagination } from '@/shared/ui/Table'
import { adminApi, adminKeys, asignarPlanSchema, empresaSchema } from './api'
import type { EmpresaResumen, Plan } from './api'

function OpcionesPlan({ planes }: { planes?: Plan[] }) {
  return (
    <>
      <option value="">Selecciona…</option>
      {planes
        ?.filter((plan) => plan.activo)
        .map((plan) => (
          <option key={plan.id} value={plan.id}>
            {plan.nombre}
          </option>
        ))}
    </>
  )
}

function EmpresaForm({ planes, alCerrar, alCrear }: { planes?: Plan[]; alCerrar: () => void; alCrear: (empresa: Empresa) => void }) {
  const form = useZodForm(empresaSchema, {
    nombre: '',
    rtn: '',
    email: '',
    telefono: '',
    direccion: undefined,
    planId: undefined,
    admin: { nombre: '', apellido: undefined, email: '', password: '' },
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: adminApi.crearEmpresa,
    invalidar: [adminKeys.todasEmpresas],
    alExito: alCrear,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal
      abierto
      alCerrar={alCerrar}
      ancho="lg"
      titulo="Registrar organización"
      descripcion="Se crea la organización, su suscripción y la cuenta de su primer administrador."
    >
      <form onSubmit={enviar} noValidate className="flex flex-col gap-5">
        <FormError mensaje={errorGeneral} />
        <fieldset className="grid gap-4 sm:grid-cols-2">
          <legend className="eyebrow mb-3">Organización</legend>
          <Input etiqueta="Nombre" clase="sm:col-span-2" error={errors.nombre?.message} {...register('nombre')} />
          <Input etiqueta="RTN" inputMode="numeric" maxLength={14} className="font-mono" error={errors.rtn?.message} {...register('rtn')} />
          <Input etiqueta="Teléfono" inputMode="numeric" maxLength={8} className="font-mono" error={errors.telefono?.message} {...register('telefono')} />
          <Input etiqueta="Correo de contacto" type="email" error={errors.email?.message} {...register('email')} />
          <Select etiqueta="Plan" error={errors.planId?.message} {...register('planId', comoNumero)}>
            <OpcionesPlan planes={planes} />
          </Select>
          <Input etiqueta="Dirección" opcional clase="sm:col-span-2" error={errors.direccion?.message} {...register('direccion', comoTextoOpcional)} />
        </fieldset>
        <fieldset className="grid gap-4 border-t border-line-soft pt-5 sm:grid-cols-2">
          <legend className="eyebrow mb-3">Primer administrador</legend>
          <Input etiqueta="Nombre" error={errors.admin?.nombre?.message} {...register('admin.nombre')} />
          <Input etiqueta="Apellido" opcional error={errors.admin?.apellido?.message} {...register('admin.apellido', comoTextoOpcional)} />
          <Input etiqueta="Correo" type="email" error={errors.admin?.email?.message} {...register('admin.email')} />
          <Input
            etiqueta="Contraseña"
            type="text"
            autoComplete="off"
            className="font-mono"
            ayuda="Mínimo 8 caracteres, con una mayúscula y un número."
            error={errors.admin?.password?.message}
            {...register('admin.password')}
          />
        </fieldset>
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Registrar organización
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

function AsignarPlan({ empresa, planes, alCerrar }: { empresa: EmpresaResumen; planes?: Plan[]; alCerrar: () => void }) {
  const form = useZodForm(asignarPlanSchema, { planId: undefined, fechaInicio: hoyISO(), fechaFin: undefined })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => adminApi.asignarPlan(empresa.id, valores),
    invalidar: [adminKeys.todasEmpresas],
    mensaje: 'Plan asignado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Asignar plan" descripcion={`${empresa.nombre} · plan actual: ${empresa.plan ?? 'ninguno'}`}>
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Select etiqueta="Plan" error={errors.planId?.message} {...register('planId', comoNumero)}>
          <OpcionesPlan planes={planes} />
        </Select>
        <div className="grid grid-cols-2 gap-4">
          <Input
            etiqueta="Inicio"
            type="date"
            className="font-mono"
            error={errors.fechaInicio?.message}
            {...register('fechaInicio', { deps: ['fechaFin'] })}
          />
          <Input etiqueta="Fin" type="date" opcional className="font-mono" error={errors.fechaFin?.message} {...register('fechaFin', comoTextoOpcional)} />
        </div>
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Asignar
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function EmpresasPage() {
  const [pagina, setPagina] = useState(1)
  const [creando, setCreando] = useState(false)
  const [asignando, setAsignando] = useState<EmpresaResumen | null>(null)
  const [creada, setCreada] = useState<Empresa | null>(null)

  const planes = useQuery({ queryKey: adminKeys.planes, queryFn: adminApi.planes })
  const empresas = useQuery({
    queryKey: adminKeys.empresas(pagina),
    queryFn: () => adminApi.empresas(pagina),
    placeholderData: keepPreviousData,
  })

  const cambiarEstado = useAccion({
    mutationFn: ({ id, estado }: { id: number; estado: EstadoSuscripcion }) => adminApi.cambiarEstadoSuscripcion(id, estado),
    invalidar: [adminKeys.todasEmpresas],
    mensaje: 'Estado de la suscripción actualizado.',
  })

  return (
    <>
      <PageHeader
        seccion="AgroCita · SaaS"
        titulo="Organizaciones"
        descripcion="Las organizaciones clientes, su plan y el estado de su suscripción."
        acciones={
          <Button icono={<Plus className="size-4" />} onClick={() => setCreando(true)}>
            Registrar organización
          </Button>
        }
      />

      <Card>
        <QueryView query={empresas}>
          {(datos) =>
            datos.items.length === 0 ? (
              <div className="p-6">
                <EmptyState compacto icono={<Building2 className="size-5" />} titulo="Todavía no hay organizaciones" />
              </div>
            ) : (
              <>
                <DataTable<EmpresaResumen>
                  filas={datos.items}
                  idDe={(fila) => fila.id}
                  columnas={[
                    { clave: 'nombre', titulo: 'Organización', celda: (fila) => <span className="font-semibold">{fila.nombre}</span> },
                    { clave: 'rtn', titulo: 'RTN', celda: (fila) => <span className="font-mono text-[13px]">{fila.rtn}</span> },
                    { clave: 'plan', titulo: 'Plan', celda: (fila) => fila.plan ?? '—' },
                    {
                      clave: 'estado',
                      titulo: 'Suscripción',
                      celda: (fila) => (fila.estadoSuscripcion ? <EstadoBadge estado={fila.estadoSuscripcion} /> : '—'),
                    },
                    {
                      clave: 'acciones',
                      titulo: 'Gestionar',
                      derecha: true,
                      celda: (fila) => (
                        <span className="inline-flex items-center gap-2">
                          {fila.suscripcionId && fila.estadoSuscripcion && (
                            <select
                              aria-label={`Estado de la suscripción de ${fila.nombre}`}
                              value={fila.estadoSuscripcion}
                              disabled={cambiarEstado.isPending}
                              onChange={(evento) =>
                                cambiarEstado.mutate({ id: fila.suscripcionId!, estado: evento.target.value as EstadoSuscripcion })
                              }
                              className="h-8 cursor-pointer rounded-lg border border-line bg-card px-2 text-[13px] font-medium hover:border-ink-faint"
                            >
                              {ESTADOS_SUSCRIPCION.map((estado) => (
                                <option key={estado}>{estado}</option>
                              ))}
                            </select>
                          )}
                          <Button tamano="sm" variante="secundario" onClick={() => setAsignando(fila)}>
                            Cambiar plan
                          </Button>
                        </span>
                      ),
                    },
                  ]}
                />
                <Pagination pagina={datos.pagina} tamano={datos.tamano} total={datos.total} alCambiar={setPagina} />
              </>
            )
          }
        </QueryView>
      </Card>

      {creando && (
        <EmpresaForm
          planes={planes.data}
          alCerrar={() => setCreando(false)}
          alCrear={(empresa) => {
            setCreando(false)
            setCreada(empresa)
          }}
        />
      )}
      {asignando && <AsignarPlan empresa={asignando} planes={planes.data} alCerrar={() => setAsignando(null)} />}

      <Modal
        abierto={Boolean(creada)}
        alCerrar={() => setCreada(null)}
        ancho="sm"
        titulo="Organización registrada"
        descripcion={`${creada?.nombre ?? ''} ya puede operar. Entrega este código a su administrador.`}
      >
        <p className="rounded-xl bg-forest px-4 py-5 text-center font-mono text-2xl tracking-[0.2em] text-paper">
          {creada?.codigoInvitacion}
        </p>
        <ModalActions>
          <Button onClick={() => setCreada(null)}>Entendido</Button>
        </ModalActions>
      </Modal>
    </>
  )
}
