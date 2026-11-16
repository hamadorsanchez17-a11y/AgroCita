import { useMutation } from '@tanstack/react-query'
import { ArrowRight } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { applyServerErrors } from '@/shared/api/errors'
import { useAuth } from '@/shared/auth/AuthContext'
import { useZodForm } from '@/shared/forms/zod'
import { Button } from '@/shared/ui/Button'
import { FormError, Input } from '@/shared/ui/Field'
import { authApi } from './api'
import { AuthShell } from './AuthShell'
import { loginSchema } from './schemas'
import type { LoginValues } from './schemas'

const CUENTAS_DEMO = [
  { rol: 'Productor', email: 'productor@agroconsulta.hn', password: 'Productor123*' },
  { rol: 'Técnico', email: 'tecnico@agroconsulta.hn', password: 'Tecnico123*' },
  { rol: 'Administrador', email: 'admin@agroconsulta.hn', password: 'Admin123*' },
  { rol: 'SuperAdmin', email: 'superadmin@agrocita.hn', password: 'SuperAdmin123*' },
]

export function LoginPage() {
  const { iniciar } = useAuth()
  const [errorGeneral, setErrorGeneral] = useState<string | null>(null)

  const {
    register,
    handleSubmit,
    setError,
    reset,
    formState: { errors, isValid },
  } = useZodForm(loginSchema, { email: '', password: '' })

  const login = useMutation({
    mutationFn: authApi.login,
    onSuccess: iniciar,
    onError: (error) => setErrorGeneral(applyServerErrors(error, setError, ['email', 'password'])),
  })

  const enviar = (valores: LoginValues) => {
    setErrorGeneral(null)
    login.mutate(valores)
  }

  return (
    <AuthShell
      frase={
        <>
          La asistencia técnica, <em className="text-ochre">anotada</em> y a tiempo.
        </>
      }
      nota="Solicitudes, visitas, inspecciones e informes de cada finca en un solo registro que no se pierde ni se sobrescribe."
    >
      <div className="stagger">
        <p className="eyebrow">Acceso</p>
        <h2 className="mt-3 text-[2.6rem] leading-none font-semibold">Inicia <em>sesión</em></h2>
        <p className="mt-2 text-[15px] text-ink-soft">Entra con el correo de tu cuenta en la organización.</p>

        <form onSubmit={handleSubmit(enviar)} noValidate className="mt-8 flex flex-col gap-5">
          <FormError mensaje={errorGeneral} />
          <Input
            etiqueta="Correo electrónico"
            type="email"
            autoComplete="email"
            placeholder="nombre@organizacion.hn"
            error={errors.email?.message}
            {...register('email')}
          />
          <Input
            etiqueta="Contraseña"
            type="password"
            autoComplete="current-password"
            placeholder="••••••••"
            error={errors.password?.message}
            {...register('password')}
          />
          <Button
            type="submit"
            tamano="lg"
            ancho
            cargando={login.isPending}
            disabled={!isValid}
            className="mt-1 flex-row-reverse"
            icono={<ArrowRight className="size-4" />}
          >
            Entrar
          </Button>
        </form>

        <p className="mt-6 text-sm text-ink-soft">
          ¿Eres productor y tienes un código de invitación?{' '}
          <Link to="/registro" className="font-semibold text-forest underline decoration-ochre decoration-2 underline-offset-4 hover:text-forest-light">
            Crea tu cuenta
          </Link>
        </p>

        {import.meta.env.DEV && (
          <div className="mt-10 rounded-2xl border border-dashed border-line p-4">
            <p className="eyebrow mb-3">Cuentas de demostración</p>
            <div className="flex flex-wrap gap-2">
              {CUENTAS_DEMO.map((cuenta) => (
                <button
                  key={cuenta.email}
                  type="button"
                  onClick={() => {
                    setErrorGeneral(null)
                    reset({ email: cuenta.email, password: cuenta.password })
                    login.mutate({ email: cuenta.email, password: cuenta.password })
                  }}
                  className="cursor-pointer rounded-lg border border-line bg-card px-3 py-1.5 text-[13px] font-semibold text-ink-soft transition hover:border-forest hover:text-forest"
                >
                  {cuenta.rol}
                </button>
              ))}
            </div>
          </div>
        )}
      </div>
    </AuthShell>
  )
}
