import { useMutation } from '@tanstack/react-query'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { applyServerErrors } from '@/shared/api/errors'
import { useAuth } from '@/shared/auth/AuthContext'
import { useZodForm } from '@/shared/forms/zod'
import { Button } from '@/shared/ui/Button'
import { FormError, Input } from '@/shared/ui/Field'
import { authApi } from './api'
import { AuthShell } from './AuthShell'
import { registroSchema } from './schemas'
import type { RegistroValues } from './schemas'

const CAMPOS = ['codigoInvitacion', 'nombre', 'apellido', 'dni', 'telefono', 'email', 'password'] as const

export function RegistroPage() {
  const { iniciar } = useAuth()
  const [errorGeneral, setErrorGeneral] = useState<string | null>(null)

  const {
    register,
    handleSubmit,
    setError,
    formState: { errors, isValid },
  } = useZodForm(registroSchema, {
    codigoInvitacion: '',
    nombre: '',
    apellido: '',
    dni: '',
    telefono: '',
    email: '',
    password: '',
  })

  const registro = useMutation({
    mutationFn: authApi.registrarProductor,
    onSuccess: iniciar,
    onError: (error) => setErrorGeneral(applyServerErrors(error, setError, CAMPOS)),
  })

  const enviar = (valores: RegistroValues) => {
    setErrorGeneral(null)
    registro.mutate({ ...valores, codigoInvitacion: valores.codigoInvitacion.toUpperCase() })
  }

  return (
    <AuthShell
      frase={
        <>
          Tu finca, con <em className="text-ochre">seguimiento</em> técnico de verdad.
        </>
      }
      nota="Regístrate con el código de 12 caracteres que te entregó tu organización y quedarás vinculado a ella desde el primer momento."
    >
      <div className="stagger">
        <p className="eyebrow">Registro de productor</p>
        <h2 className="mt-3 text-[2.6rem] leading-none font-semibold">Crea tu <em>cuenta</em></h2>

        <form onSubmit={handleSubmit(enviar)} noValidate className="mt-7 flex flex-col gap-4">
          <FormError mensaje={errorGeneral} />
          <Input
            etiqueta="Código de invitación"
            placeholder="AGROCONSU001"
            maxLength={12}
            autoCapitalize="characters"
            className="font-mono tracking-[0.2em] uppercase"
            ayuda="Te lo entrega el administrador de tu organización."
            error={errors.codigoInvitacion?.message}
            {...register('codigoInvitacion')}
          />
          <div className="grid gap-4 sm:grid-cols-2">
            <Input etiqueta="Nombre" autoComplete="given-name" error={errors.nombre?.message} {...register('nombre')} />
            <Input etiqueta="Apellido" autoComplete="family-name" error={errors.apellido?.message} {...register('apellido')} />
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <Input
              etiqueta="DNI"
              inputMode="numeric"
              maxLength={13}
              placeholder="0801199012345"
              className="font-mono"
              error={errors.dni?.message}
              {...register('dni')}
            />
            <Input
              etiqueta="Teléfono"
              inputMode="numeric"
              maxLength={8}
              placeholder="99887766"
              className="font-mono"
              error={errors.telefono?.message}
              {...register('telefono')}
            />
          </div>
          <Input etiqueta="Correo electrónico" type="email" autoComplete="email" error={errors.email?.message} {...register('email')} />
          <Input
            etiqueta="Contraseña"
            type="password"
            autoComplete="new-password"
            ayuda="Mínimo 8 caracteres, con una mayúscula y un número."
            error={errors.password?.message}
            {...register('password')}
          />
          <Button type="submit" tamano="lg" ancho cargando={registro.isPending} disabled={!isValid} className="mt-2">
            Crear cuenta
          </Button>
        </form>

        <p className="mt-6 text-sm text-ink-soft">
          ¿Ya tienes cuenta?{' '}
          <Link to="/login" className="font-semibold text-forest underline decoration-ochre decoration-2 underline-offset-4 hover:text-forest-light">
            Inicia sesión
          </Link>
        </p>
      </div>
    </AuthShell>
  )
}
