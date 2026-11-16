import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { rutaInicial, useAuth } from '@/shared/auth/AuthContext'
import type { Rol } from '@/shared/auth/session'

export function RequireAuth() {
  const { autenticado } = useAuth()
  const ubicacion = useLocation()

  if (!autenticado) {
    return <Navigate to="/login" replace state={{ desde: ubicacion.pathname + ubicacion.search }} />
  }

  return <Outlet />
}

export function RequireRole({ roles }: { roles: Rol[] }) {
  const { usuario } = useAuth()

  if (!usuario) return <Navigate to="/login" replace />

  if (!roles.includes(usuario.rol)) {
    return <Navigate to={rutaInicial(usuario.rol)} replace />
  }

  return <Outlet />
}

export function GuestOnly() {
  const { usuario } = useAuth()
  const ubicacion = useLocation()

  if (usuario) {
    const desde = (ubicacion.state as { desde?: string } | null)?.desde
    return <Navigate to={desde ?? rutaInicial(usuario.rol)} replace />
  }

  return <Outlet />
}
