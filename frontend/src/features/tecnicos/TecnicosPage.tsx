import { useQuery } from '@tanstack/react-query'
import { Pencil, Plus, Sprout } from 'lucide-react'
import { useState } from 'react'
import { comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useAccion, useFormMutation } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { Badge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { FormError, Input } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { PageHeader } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { tecnicoCrearSchema, tecnicoEditarSchema, tecnicosApi, tecnicosKeys } from './api'
import type { Tecnico } from './api'

function CrearTecnico({ alCerrar }: { alCerrar: () => void }) {
  const form = useZodForm(tecnicoCrearSchema, {
    nombre: '',
    apellido: '',
    email: '',
    telefono: '',
    especialidad: undefined,
    passwordTemporal: '',
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: tecnicosApi.crear,
    invalidar: [tecnicosKeys.todos],
    mensaje: 'Cuenta de técnico creada.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} titulo="Nuevo técnico" descripcion="El técnico entra con este correo y la contraseña temporal.">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Input etiqueta="Nombre" error={errors.nombre?.message} {...register('nombre')} />
          <Input etiqueta="Apellido" error={errors.apellido?.message} {...register('apellido')} />
        </div>
        <Input etiqueta="Correo electrónico" type="email" error={errors.email?.message} {...register('email')} />
        <div className="grid gap-4 sm:grid-cols-2">
          <Input etiqueta="Teléfono" inputMode="numeric" maxLength={8} className="font-mono" error={errors.telefono?.message} {...register('telefono')} />
          <Input etiqueta="Especialidad" opcional placeholder="Caficultura" error={errors.especialidad?.message} {...register('especialidad', comoTextoOpcional)} />
        </div>
        <Input
          etiqueta="Contraseña temporal"
          type="text"
          autoComplete="off"
          className="font-mono"
          ayuda="Mínimo 8 caracteres."
          error={errors.passwordTemporal?.message}
          {...register('passwordTemporal')}
        />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Crear cuenta
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

function EditarTecnico({ tecnico, alCerrar }: { tecnico: Tecnico; alCerrar: () => void }) {
  const form = useZodForm(tecnicoEditarSchema, {
    nombre: tecnico.nombre,
    apellido: tecnico.apellido,
    telefono: tecnico.telefono,
    especialidad: tecnico.especialidad ?? undefined,
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => tecnicosApi.actualizar(tecnico.id, valores),
    invalidar: [tecnicosKeys.todos],
    mensaje: 'Técnico actualizado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Editar técnico" descripcion={tecnico.email}>
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre" error={errors.nombre?.message} {...register('nombre')} />
        <Input etiqueta="Apellido" error={errors.apellido?.message} {...register('apellido')} />
        <Input etiqueta="Teléfono" inputMode="numeric" maxLength={8} className="font-mono" error={errors.telefono?.message} {...register('telefono')} />
        <Input etiqueta="Especialidad" opcional error={errors.especialidad?.message} {...register('especialidad', comoTextoOpcional)} />
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            Guardar cambios
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function TecnicosPage() {
  const tecnicos = useQuery({ queryKey: tecnicosKeys.todos, queryFn: tecnicosApi.listar })
  const [creando, setCreando] = useState(false)
  const [editando, setEditando] = useState<Tecnico | null>(null)

  const cambiarEstado = useAccion({
    mutationFn: (tecnico: Tecnico) => tecnicosApi.cambiarEstado(tecnico.id, !tecnico.activo),
    invalidar: [tecnicosKeys.todos],
    mensaje: 'Estado del técnico actualizado.',
  })

  return (
    <>
      <PageHeader
        seccion="Administración · Personal"
        titulo="Técnicos"
        descripcion="Crea las cuentas de tus técnicos y desactiva las que ya no deban entrar."
        acciones={
          <Button icono={<Plus className="size-4" />} onClick={() => setCreando(true)}>
            Nuevo técnico
          </Button>
        }
      />

      <QueryView query={tecnicos}>
        {(lista) =>
          lista.length === 0 ? (
            <EmptyState
              icono={<Sprout className="size-5" />}
              titulo="Tu organización aún no tiene técnicos"
              descripcion="Crea la primera cuenta para poder asignar visitas."
            />
          ) : (
            <ul className="stagger grid gap-4 md:grid-cols-2 xl:grid-cols-3">
              {lista.map((tecnico) => (
                <li key={tecnico.id} className="flex flex-col rounded-2xl border border-line bg-card p-5 shadow-sheet">
                  <div className="flex items-start gap-3.5">
                    <span className="grid size-12 shrink-0 place-items-center rounded-full bg-forest font-display text-lg text-paper">
                      {formato.iniciales(`${tecnico.nombre} ${tecnico.apellido}`)}
                    </span>
                    <div className="min-w-0 flex-1">
                      <p className="truncate text-lg leading-tight font-semibold">
                        {tecnico.nombre} {tecnico.apellido}
                      </p>
                      <p className="truncate text-[13px] text-ink-soft">{tecnico.especialidad ?? 'Sin especialidad'}</p>
                    </div>
                    <Badge tono={tecnico.activo ? 'moss' : 'neutro'}>{tecnico.activo ? 'Activo' : 'Inactivo'}</Badge>
                  </div>
                  <dl className="mt-4 grid gap-1 border-t border-line-soft pt-4 text-[13px]">
                    <div className="flex justify-between gap-3">
                      <dt className="text-ink-faint">Correo</dt>
                      <dd className="truncate">{tecnico.email}</dd>
                    </div>
                    <div className="flex justify-between gap-3">
                      <dt className="text-ink-faint">Teléfono</dt>
                      <dd className="font-mono">{tecnico.telefono}</dd>
                    </div>
                  </dl>
                  <div className="mt-4 flex gap-2">
                    <Button tamano="sm" variante="secundario" icono={<Pencil className="size-3.5" />} onClick={() => setEditando(tecnico)}>
                      Editar
                    </Button>
                    <Button
                      tamano="sm"
                      variante="fantasma"
                      disabled={cambiarEstado.isPending}
                      onClick={() => cambiarEstado.mutate(tecnico)}
                    >
                      {tecnico.activo ? 'Desactivar' : 'Reactivar'}
                    </Button>
                  </div>
                </li>
              ))}
            </ul>
          )
        }
      </QueryView>

      {creando && <CrearTecnico alCerrar={() => setCreando(false)} />}
      {editando && <EditarTecnico tecnico={editando} alCerrar={() => setEditando(null)} />}
    </>
  )
}
