import { useQuery } from '@tanstack/react-query'
import { comoTextoOpcional, useZodForm } from '@/shared/forms/zod'
import { useFormMutation } from '@/shared/forms/mutations'
import { Button } from '@/shared/ui/Button'
import { FormError, Input } from '@/shared/ui/Field'
import { QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Dato, PageHeader } from '@/shared/ui/layout'
import { perfilSchema, productoresApi, productoresKeys } from './api'
import type { Productor } from './api'

function Formulario({ productor }: { productor: Productor }) {
  const form = useZodForm(perfilSchema, {
    nombre: productor.nombre,
    apellido: productor.apellido,
    telefono: productor.telefono,
    direccion: productor.direccion ?? undefined,
  })

  const { enviar, errorGeneral, cargando } = useFormMutation(form, {
    mutationFn: productoresApi.actualizarPerfil,
    invalidar: [productoresKeys.perfil],
    mensaje: 'Perfil actualizado.',
    alExito: (actualizado) =>
      form.reset({
        nombre: actualizado.nombre,
        apellido: actualizado.apellido,
        telefono: actualizado.telefono,
        direccion: actualizado.direccion ?? undefined,
      }),
  })

  const {
    register,
    formState: { errors, isValid, isDirty },
  } = form

  return (
    <div className="stagger grid gap-6 lg:grid-cols-[minmax(0,1.5fr)_minmax(0,1fr)]">
      <Card>
        <CardHeader titulo="Datos de contacto" numero="01" descripcion="Tu organización usa estos datos para coordinar las visitas." />
        <form onSubmit={enviar} noValidate className="flex flex-col gap-4 px-5 py-5 md:px-6">
          <FormError mensaje={errorGeneral} />
          <div className="grid gap-4 sm:grid-cols-2">
            <Input etiqueta="Nombre" error={errors.nombre?.message} {...register('nombre')} />
            <Input etiqueta="Apellido" error={errors.apellido?.message} {...register('apellido')} />
          </div>
          <Input
            etiqueta="Teléfono"
            inputMode="numeric"
            maxLength={8}
            clase="sm:max-w-[220px]"
            className="font-mono"
            error={errors.telefono?.message}
            {...register('telefono')}
          />
          <Input etiqueta="Dirección" opcional error={errors.direccion?.message} {...register('direccion', comoTextoOpcional)} />
          <div className="mt-2 flex justify-end">
            <Button type="submit" cargando={cargando} disabled={!isValid || !isDirty}>
              Guardar cambios
            </Button>
          </div>
        </form>
      </Card>

      <Card className="h-fit">
        <CardHeader titulo="Identificación" numero="02" descripcion="Estos datos no se pueden cambiar desde aquí." />
        <dl className="grid gap-5 px-5 py-5 md:px-6">
          <Dato etiqueta="DNI" mono>
            {productor.dni}
          </Dato>
          <Dato etiqueta="Correo electrónico">{productor.email}</Dato>
        </dl>
      </Card>
    </div>
  )
}

export function PerfilPage() {
  const perfil = useQuery({ queryKey: productoresKeys.perfil, queryFn: productoresApi.miPerfil })

  return (
    <>
      <PageHeader seccion="Productor · Cuenta" titulo="Mi perfil" descripcion="Mantén actualizada tu información personal." />
      <QueryView query={perfil}>{(productor) => <Formulario productor={productor} />}</QueryView>
    </>
  )
}
