import { useQuery } from '@tanstack/react-query'
import { Pencil, Plus } from 'lucide-react'
import { useState } from 'react'
import { comoNumero, comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { Badge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { FormError, Input, Textarea } from '@/shared/ui/Field'
import { QueryView } from '@/shared/ui/feedback'
import { PageHeader } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { adminApi, adminKeys, planSchema } from './api'
import type { Plan } from './api'

function PlanForm({ plan, alCerrar }: { plan?: Plan; alCerrar: () => void }) {
  const form = useZodForm(planSchema, {
    nombre: plan?.nombre ?? '',
    descripcion: plan?.descripcion ?? undefined,
    precioMensual: plan?.precioMensual,
    maxTecnicos: plan?.maxTecnicos ?? undefined,
    maxProductores: plan?.maxProductores ?? undefined,
    activo: plan?.activo ?? true,
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: (valores) => (plan ? adminApi.actualizarPlan(plan.id, valores) : adminApi.crearPlan(valores)),
    invalidar: [adminKeys.planes],
    mensaje: plan ? 'Plan actualizado.' : 'Plan creado.',
    alExito: alCerrar,
  })

  const {
    register,
    formState: { errors, isValid },
  } = form

  return (
    <Modal abierto alCerrar={alCerrar} titulo={plan ? 'Editar plan' : 'Nuevo plan'} descripcion="Deja un límite vacío para que sea ilimitado.">
      <form onSubmit={enviar} noValidate className="flex flex-col gap-4">
        <FormError mensaje={errorGeneral} />
        <div className="grid gap-4 sm:grid-cols-[1fr_180px]">
          <Input etiqueta="Nombre" error={errors.nombre?.message} {...register('nombre')} />
          <Input
            etiqueta="Precio mensual (L)"
            type="number"
            step="0.01"
            min="0"
            className="font-mono"
            error={errors.precioMensual?.message}
            {...register('precioMensual', comoNumero)}
          />
        </div>
        <Textarea etiqueta="Descripción" opcional rows={2} error={errors.descripcion?.message} {...register('descripcion', comoTextoOpcional)} />
        <div className="grid grid-cols-2 gap-4">
          <Input
            etiqueta="Máx. técnicos"
            type="number"
            min="1"
            opcional
            className="font-mono"
            error={errors.maxTecnicos?.message}
            {...register('maxTecnicos', comoNumero)}
          />
          <Input
            etiqueta="Máx. productores"
            type="number"
            min="1"
            opcional
            className="font-mono"
            error={errors.maxProductores?.message}
            {...register('maxProductores', comoNumero)}
          />
        </div>
        <label className="flex cursor-pointer items-center gap-3 rounded-xl border border-line bg-white/60 px-3.5 py-3 text-sm font-medium">
          <input type="checkbox" className="size-4 accent-forest" {...register('activo')} />
          Plan disponible para nuevas organizaciones
        </label>
        <ModalActions>
          <Button variante="fantasma" onClick={alCerrar}>
            Cancelar
          </Button>
          <Button type="submit" cargando={cargando} disabled={!isValid}>
            {plan ? 'Guardar cambios' : 'Crear plan'}
          </Button>
        </ModalActions>
      </form>
    </Modal>
  )
}

export function PlanesPage() {
  const planes = useQuery({ queryKey: adminKeys.planes, queryFn: adminApi.planes })
  const [formulario, setFormulario] = useState<{ plan?: Plan } | null>(null)

  return (
    <>
      <PageHeader
        seccion="AgroCita · SaaS"
        titulo="Planes de suscripción"
        descripcion="Precio y límites de técnicos y productores de cada plan."
        acciones={
          <Button icono={<Plus className="size-4" />} onClick={() => setFormulario({})}>
            Nuevo plan
          </Button>
        }
      />

      <QueryView query={planes}>
        {(lista) => (
          <ul className="stagger grid gap-5 md:grid-cols-2 xl:grid-cols-3">
            {lista.map((plan) => (
              <li key={plan.id} className="flex flex-col rounded-2xl border border-line bg-card shadow-sheet">
                <div className="ruled flex items-start justify-between px-6 pt-6 pb-8">
                  <div>
                    <p className="eyebrow">Plan</p>
                    <h2 className="mt-1 text-3xl font-semibold">{plan.nombre}</h2>
                  </div>
                  <Badge tono={plan.activo ? 'moss' : 'neutro'}>{plan.activo ? 'Disponible' : 'Retirado'}</Badge>
                </div>
                <div className="flex flex-1 flex-col px-6 pb-6">
                  <p className="font-display text-5xl leading-none font-semibold tracking-[-0.04em]">
                    {formato.moneda(plan.precioMensual)}
                    <span className="ml-1.5 font-sans text-sm text-ink-faint">/ mes</span>
                  </p>
                  {plan.descripcion && <p className="mt-3 text-sm text-ink-soft">{plan.descripcion}</p>}
                  <dl className="mt-5 grid grid-cols-2 gap-4 border-t border-line-soft pt-4">
                    <div>
                      <dt className="eyebrow">Técnicos</dt>
                      <dd className="mt-1 font-mono text-lg">{plan.maxTecnicos ?? '∞'}</dd>
                    </div>
                    <div>
                      <dt className="eyebrow">Productores</dt>
                      <dd className="mt-1 font-mono text-lg">{plan.maxProductores ?? '∞'}</dd>
                    </div>
                  </dl>
                  <div className="mt-5">
                    <Button tamano="sm" variante="secundario" icono={<Pencil className="size-3.5" />} onClick={() => setFormulario({ plan })}>
                      Editar
                    </Button>
                  </div>
                </div>
              </li>
            ))}
          </ul>
        )}
      </QueryView>

      {formulario && <PlanForm plan={formulario.plan} alCerrar={() => setFormulario(null)} />}
    </>
  )
}
