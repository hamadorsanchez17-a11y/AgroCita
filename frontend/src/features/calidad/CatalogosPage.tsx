import { useQuery } from '@tanstack/react-query'
import clsx from 'clsx'
import { Plus } from 'lucide-react'
import { useState } from 'react'
import { fincasApi, fincasKeys } from '@/features/fincas/api'
import type { TipoCultivo } from '@/features/fincas/api'
import { tipoCultivoSchema } from '@/features/fincas/schemas'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Textarea } from '@/shared/ui/Field'
import { EmptyState, QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, PageHeader, Segmented } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { DataTable } from '@/shared/ui/Table'
import { calidadApi, calidadKeys } from './api'
import type { Fertilizante } from './api'
import { categoriaSchema, fertilizanteSchema, parametroSchema } from './schemas'

type Seccion = 'cultivos' | 'categorias' | 'fertilizantes'

function Acciones({ alCerrar, cargando, valido, texto }: { alCerrar: () => void; cargando: boolean; valido: boolean; texto: string }) {
  return (
    <ModalActions>
      <Button variante="fantasma" onClick={alCerrar}>
        Cancelar
      </Button>
      <Button type="submit" cargando={cargando} disabled={!valido}>
        {texto}
      </Button>
    </ModalActions>
  )
}

function TipoForm({ alCerrar }: { alCerrar: () => void }) {
  const form = useZodForm(tipoCultivoSchema, { nombre: '' })
  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: fincasApi.crearTipo,
    invalidar: [fincasKeys.tipos],
    mensaje: 'Tipo de cultivo agregado.',
    alExito: alCerrar,
  })

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Nuevo tipo de cultivo">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre" placeholder="Café" error={form.formState.errors.nombre?.message} {...form.register('nombre')} />
        <Acciones alCerrar={alCerrar} cargando={cargando} valido={form.formState.isValid} texto="Agregar" />
      </form>
    </Modal>
  )
}

function ParametroForm({ tipo, alCerrar }: { tipo: TipoCultivo; alCerrar: () => void }) {
  const form = useZodForm(parametroSchema, { nombre: '', unidad: undefined, valorMinimo: undefined, valorMaximo: undefined })
  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => calidadApi.crearParametro(tipo.id, valores),
    invalidar: [calidadKeys.parametros(tipo.id)],
    mensaje: 'Parámetro agregado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Nuevo parámetro" descripcion={`Se evaluará en las inspecciones de ${tipo.nombre}.`}>
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre" placeholder="Humedad del grano" error={errors.nombre?.message} {...register('nombre')} />
        <Input etiqueta="Unidad" opcional placeholder="%" error={errors.unidad?.message} {...register('unidad', comoTextoOpcional)} />
        <div className="grid grid-cols-2 gap-4">
          <Input
            etiqueta="Valor mínimo"
            type="number"
            step="0.01"
            opcional
            className="font-mono"
            error={errors.valorMinimo?.message}
            {...register('valorMinimo', { ...comoNumero, deps: ['valorMaximo'] })}
          />
          <Input
            etiqueta="Valor máximo"
            type="number"
            step="0.01"
            opcional
            className="font-mono"
            error={errors.valorMaximo?.message}
            {...register('valorMaximo', comoNumero)}
          />
        </div>
        <Acciones alCerrar={alCerrar} cargando={cargando} valido={isValid} texto="Agregar" />
      </form>
    </Modal>
  )
}

function CategoriaForm({ alCerrar }: { alCerrar: () => void }) {
  const form = useZodForm(categoriaSchema, { nombre: '', descripcion: undefined })
  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: calidadApi.crearCategoria,
    invalidar: [calidadKeys.categorias],
    mensaje: 'Categoría agregada.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Nueva categoría de calidad">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre" placeholder="Grado A" error={errors.nombre?.message} {...register('nombre')} />
        <Textarea etiqueta="Descripción" opcional rows={3} error={errors.descripcion?.message} {...register('descripcion', comoTextoOpcional)} />
        <Acciones alCerrar={alCerrar} cargando={cargando} valido={isValid} texto="Agregar" />
      </form>
    </Modal>
  )
}

function FertilizanteForm({ alCerrar }: { alCerrar: () => void }) {
  const form = useZodForm(fertilizanteSchema, { nombre: '', tipo: '', unidadMedida: '', composicion: undefined })
  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: calidadApi.crearFertilizante,
    invalidar: [calidadKeys.fertilizantes],
    mensaje: 'Fertilizante agregado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} ancho="sm" titulo="Nuevo fertilizante">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <Input etiqueta="Nombre" placeholder="Urea 46%" error={errors.nombre?.message} {...register('nombre')} />
        <div className="grid grid-cols-2 gap-4">
          <Input etiqueta="Tipo" placeholder="Nitrogenado" error={errors.tipo?.message} {...register('tipo')} />
          <Input etiqueta="Unidad de medida" placeholder="lb" className="font-mono" error={errors.unidadMedida?.message} {...register('unidadMedida')} />
        </div>
        <Input etiqueta="Composición" opcional placeholder="46-0-0" className="font-mono" error={errors.composicion?.message} {...register('composicion', comoTextoOpcional)} />
        <Acciones alCerrar={alCerrar} cargando={cargando} valido={isValid} texto="Agregar" />
      </form>
    </Modal>
  )
}

function Parametros({ tipo }: { tipo: TipoCultivo }) {
  const parametros = useQuery({ queryKey: calidadKeys.parametros(tipo.id), queryFn: () => calidadApi.parametros(tipo.id) })
  const [creando, setCreando] = useState(false)

  return (
    <Card>
      <CardHeader
        titulo={`Parámetros de ${tipo.nombre}`}
        descripcion="Lo que el técnico mide en cada inspección."
        accion={
          <Button tamano="sm" icono={<Plus className="size-3.5" />} onClick={() => setCreando(true)}>
            Parámetro
          </Button>
        }
      />
      <QueryView query={parametros} filas={2}>
        {(lista) =>
          lista.length === 0 ? (
            <div className="p-5 md:p-6">
              <EmptyState compacto titulo="Sin parámetros" descripcion="Sin parámetros no se pueden registrar inspecciones de este cultivo." />
            </div>
          ) : (
            <ul>
              {lista.map((parametro) => (
                <li key={parametro.id} className="flex items-center justify-between gap-4 border-b border-line-soft px-5 py-3.5 last:border-0 md:px-6">
                  <p className="font-semibold">{parametro.nombre}</p>
                  <p className="font-mono text-[13px] text-ink-soft">
                    {parametro.valorMinimo != null && parametro.valorMaximo != null
                      ? `${parametro.valorMinimo} – ${parametro.valorMaximo} `
                      : ''}
                    {parametro.unidad}
                  </p>
                </li>
              ))}
            </ul>
          )
        }
      </QueryView>
      {creando && <ParametroForm tipo={tipo} alCerrar={() => setCreando(false)} />}
    </Card>
  )
}

function SeccionCultivos() {
  const tipos = useQuery({ queryKey: fincasKeys.tipos, queryFn: fincasApi.tipos })
  const [seleccion, setSeleccion] = useState<number | null>(null)
  const [creando, setCreando] = useState(false)

  return (
    <QueryView query={tipos}>
      {(lista) => {
        const activo = lista.find((tipo) => tipo.id === seleccion) ?? lista[0]

        return (
          <div className="grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.6fr)]">
            <Card className="h-fit">
              <CardHeader
                titulo="Tipos de cultivo"
                accion={
                  <Button tamano="sm" icono={<Plus className="size-3.5" />} onClick={() => setCreando(true)}>
                    Tipo
                  </Button>
                }
              />
              {lista.length === 0 ? (
                <div className="p-5">
                  <EmptyState compacto titulo="Sin tipos de cultivo" descripcion="Los productores los necesitan para registrar sus cultivos." />
                </div>
              ) : (
                <ul className="p-2">
                  {lista.map((tipo) => (
                    <li key={tipo.id}>
                      <button
                        type="button"
                        onClick={() => setSeleccion(tipo.id)}
                        className={clsx(
                          'w-full cursor-pointer rounded-xl px-3.5 py-2.5 text-left text-[15px] font-medium transition',
                          activo?.id === tipo.id ? 'bg-forest text-paper' : 'hover:bg-paper-deep',
                        )}
                      >
                        {tipo.nombre}
                      </button>
                    </li>
                  ))}
                </ul>
              )}
            </Card>
            {activo && <Parametros key={activo.id} tipo={activo} />}
            {creando && <TipoForm alCerrar={() => setCreando(false)} />}
          </div>
        )
      }}
    </QueryView>
  )
}

function SeccionCategorias() {
  const categorias = useQuery({ queryKey: calidadKeys.categorias, queryFn: calidadApi.categorias })
  const [creando, setCreando] = useState(false)

  return (
    <Card>
      <CardHeader
        titulo="Categorías de calidad"
        descripcion="Con ellas el técnico clasifica cada cultivo inspeccionado."
        accion={
          <Button tamano="sm" icono={<Plus className="size-3.5" />} onClick={() => setCreando(true)}>
            Categoría
          </Button>
        }
      />
      <QueryView query={categorias}>
        {(lista) =>
          lista.length === 0 ? (
            <div className="p-6">
              <EmptyState compacto titulo="Sin categorías" />
            </div>
          ) : (
            <ul className="grid gap-px bg-line-soft sm:grid-cols-2 lg:grid-cols-3">
              {lista.map((categoria, indice) => (
                <li key={categoria.id} className="bg-card px-5 py-4 md:px-6">
                  <p className="font-mono text-[11px] text-ochre-deep">{String(indice + 1).padStart(2, '0')}</p>
                  <p className="mt-1 font-display text-xl">{categoria.nombre}</p>
                  {categoria.descripcion && <p className="mt-1 text-[13px] text-ink-soft">{categoria.descripcion}</p>}
                </li>
              ))}
            </ul>
          )
        }
      </QueryView>
      {creando && <CategoriaForm alCerrar={() => setCreando(false)} />}
    </Card>
  )
}

function SeccionFertilizantes() {
  const fertilizantes = useQuery({ queryKey: calidadKeys.fertilizantes, queryFn: calidadApi.fertilizantes })
  const [creando, setCreando] = useState(false)

  return (
    <Card>
      <CardHeader
        titulo="Fertilizantes"
        descripcion="El catálogo que usan los técnicos al recomendar y registrar aplicaciones."
        accion={
          <Button tamano="sm" icono={<Plus className="size-3.5" />} onClick={() => setCreando(true)}>
            Fertilizante
          </Button>
        }
      />
      <QueryView query={fertilizantes}>
        {(lista) =>
          lista.length === 0 ? (
            <div className="p-6">
              <EmptyState compacto titulo="Sin fertilizantes" />
            </div>
          ) : (
            <DataTable<Fertilizante>
              filas={lista}
              idDe={(fila) => fila.id}
              columnas={[
                { clave: 'nombre', titulo: 'Nombre', celda: (fila) => <span className="font-semibold">{fila.nombre}</span> },
                { clave: 'tipo', titulo: 'Tipo', celda: (fila) => fila.tipo },
                { clave: 'composicion', titulo: 'Composición', celda: (fila) => <span className="font-mono text-[13px]">{fila.composicion ?? '—'}</span> },
                { clave: 'unidad', titulo: 'Unidad', derecha: true, celda: (fila) => <span className="font-mono text-[13px]">{fila.unidadMedida}</span> },
              ]}
            />
          )
        }
      </QueryView>
      {creando && <FertilizanteForm alCerrar={() => setCreando(false)} />}
    </Card>
  )
}

export function CatalogosPage() {
  const [seccion, setSeccion] = useState<Seccion>('cultivos')

  return (
    <>
      <PageHeader
        seccion="Administración · Configuración"
        titulo="Catálogos"
        descripcion="Define con criterio agronómico lo que tu organización mide, clasifica y aplica."
      />
      <div className="mb-6">
        <Segmented
          etiqueta="Catálogo"
          valor={seccion}
          alCambiar={setSeccion}
          opciones={[
            { valor: 'cultivos', etiqueta: 'Cultivos y parámetros' },
            { valor: 'categorias', etiqueta: 'Categorías de calidad' },
            { valor: 'fertilizantes', etiqueta: 'Fertilizantes' },
          ]}
        />
      </div>
      <div className="animate-rise" key={seccion}>
        {seccion === 'cultivos' && <SeccionCultivos />}
        {seccion === 'categorias' && <SeccionCategorias />}
        {seccion === 'fertilizantes' && <SeccionFertilizantes />}
      </div>
    </>
  )
}
