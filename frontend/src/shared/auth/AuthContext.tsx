import { useQueryClient } from '@tanstack/react-query'
import { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react'
import type { ReactNode } from 'react'
import { http } from '@/shared/api/http'
import { EVENTO_SESION_EXPIRADA, session } from './session'
import type { Rol, Sesion, UsuarioActual } from './session'

export interface AuthResponse {
  token: string
  expiraEn: number
  usuario: UsuarioActual
}

interface AuthState {
  usuario: UsuarioActual | null
  autenticado: boolean
  iniciar: (respuesta: AuthResponse) => void
  cerrar: () => void
  tieneRol: (...roles: Rol[]) => boolean
}

const AuthContext = createContext<AuthState | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const queryClient = useQueryClient()
  const [sesion, setSesion] = useState<Sesion | null>(() => session.get())

  const cerrar = useCallback(() => {
    session.clear()
    setSesion(null)
    queryClient.clear()
  }, [queryClient])

  const iniciar = useCallback(
    (respuesta: AuthResponse) => {
      queryClient.clear()
      setSesion(session.save(respuesta.token, respuesta.expiraEn, respuesta.usuario))
    },
    [queryClient],
  )

  useEffect(() => {
    const alExpirar = () => {
      setSesion(null)
      queryClient.clear()
    }

    window.addEventListener(EVENTO_SESION_EXPIRADA, alExpirar)
    return () => window.removeEventListener(EVENTO_SESION_EXPIRADA, alExpirar)
  }, [queryClient])

  const token = sesion?.token

  useEffect(() => {
    if (!token) return

    let vigente = true

    http
      .get<UsuarioActual>('/auth/me')
      .then(({ data }) => {
        if (!vigente) return
        session.updateUser(data)
        setSesion((actual) => (actual ? { ...actual, usuario: data } : actual))
      })
      .catch(() => undefined)

    return () => {
      vigente = false
    }
  }, [token])

  useEffect(() => {
    if (!sesion) return

    const restante = sesion.expiraEl - Date.now()
    const temporizador = window.setTimeout(cerrar, Math.min(Math.max(restante, 0), 2 ** 31 - 1))

    return () => window.clearTimeout(temporizador)
  }, [sesion, cerrar])

  const valor = useMemo<AuthState>(
    () => ({
      usuario: sesion?.usuario ?? null,
      autenticado: Boolean(sesion),
      iniciar,
      cerrar,
      tieneRol: (...roles) => Boolean(sesion && roles.includes(sesion.usuario.rol)),
    }),
    [sesion, iniciar, cerrar],
  )

  return <AuthContext.Provider value={valor}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthState {
  const contexto = useContext(AuthContext)

  if (!contexto) throw new Error('useAuth debe usarse dentro de AuthProvider')

  return contexto
}

export function useUsuario(): UsuarioActual {
  const { usuario } = useAuth()

  if (!usuario) throw new Error('No hay una sesión activa')

  return usuario
}

export function rutaInicial(rol: Rol): string {
  return rol === 'SuperAdmin' ? '/empresas' : '/'
}
